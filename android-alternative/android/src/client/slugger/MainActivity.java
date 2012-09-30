package client.slugger;

import android.app.Activity;
import android.content.Context;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;
import android.os.StrictMode;
import android.os.Vibrator;
import android.util.Log;
import android.view.View;
import android.widget.EditText;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.Date;

public class MainActivity extends Activity implements View.OnClickListener {
    public static class Commands {
        public static final byte CMD_INIT = 1;
        public static final byte CMD_NAME = 2;
        public static final byte CMD_ACC_DATA = 0;
        public static final byte CMD_NOTIFY = 3;
    }

	final static String LogName = "Slugger";

    final int HOST_PORT = 80;
    final byte ANDROID_APP = 1;

	private Socket connection;
    private SensorManager mSensorManager;
    private Sensor mAccelerometer;
    private AccReader accReader = new AccReader();
    private DataOutputStream dataOutputStream;
    private DataInputStream dataInputStream;

    protected boolean isRunned = false;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);

        View startButton = findViewById(R.id.start_button);
        startButton.setOnClickListener(this);

        this.mSensorManager = (SensorManager) getSystemService(Context.SENSOR_SERVICE);
        this.mAccelerometer = mSensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);

        Log.d(LogName, "onCreate finished ok");
    }

    
    public void sendName(String name) throws IOException {
    	dataOutputStream.writeByte(Commands.CMD_NAME);
    	dataOutputStream.writeInt(name.length());

        ByteBuffer bb = ByteBuffer.allocate(2 * name.length()).order(ByteOrder.LITTLE_ENDIAN);
        for(int i = 0; i<name.length(); i++){
        	bb.putChar(name.charAt(i));
        }

    	dataOutputStream.write(bb.array());    	
    }
    
    public void initializeConnection() throws Exception {
        this.connection = new Socket();
        EditText hostAddress = (EditText)findViewById(R.id.server_ip);
        this.connection.connect(new InetSocketAddress(hostAddress.getText().toString(), HOST_PORT), 1000);
        this.connection.setTcpNoDelay(true);

        Log.d(LogName, "socket connected");

		dataOutputStream = new DataOutputStream(connection.getOutputStream());
        dataInputStream = new DataInputStream(connection.getInputStream());

		dataOutputStream.writeByte(Commands.CMD_INIT);
		dataOutputStream.writeInt(ANDROID_APP);

		mSensorManager.registerListener(accReader, mAccelerometer, SensorManager.SENSOR_DELAY_FASTEST);

		EditText playerName = (EditText)findViewById(R.id.player_name);
		this.sendName(playerName.getText().toString());

        Log.d(LogName, "finished initialize");
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.start_button:
                if (isRunned){
                    return;
                }

                startGame();
                break;
        }
    }

    private void startGame() {
        try {
            this.initializeConnection();
            EditText playerName = (EditText)findViewById(R.id.player_name);
            this.sendName(playerName.getText().toString());

            Thread thread = new Thread(new Runnable() {
                private Vibrator vibrator = ((Vibrator)getSystemService(VIBRATOR_SERVICE));

                @Override
                public void run() {
                    while (true){
                        try{
                            int command = dataInputStream.readInt();
                            vibrator.vibrate(300);
                        }
                        catch (Exception e){
                             Log.e(LogName, "error: " + e.toString());
                        }
                    }
                }
            });

            thread.start();
            this.isRunned = true;
        }
        catch (Exception e){
            Log.e(LogName, "Error while connecting: " + e.toString());
        }
    }

    private void onExit(){
        try{
            this.connection.close();
            this.mSensorManager.unregisterListener(this.accReader);
            finish();
        }
        catch (Exception e){
        }
        finally {
            Log.d(LogName, "onExit");
        }
    }

    class AccReader implements SensorEventListener {
        long lastDate = 0;

		public void onSensorChanged(SensorEvent event) {
			if (event.sensor.getType() == Sensor.TYPE_ACCELEROMETER) {
                  Date date = new Date();
                  try {
                      ByteBuffer bb = ByteBuffer.allocate(16).order(ByteOrder.LITTLE_ENDIAN);
                      bb.putFloat(event.values[0]/SensorManager.GRAVITY_EARTH);
                      bb.putFloat(event.values[1]/SensorManager.GRAVITY_EARTH);
                      bb.putFloat(event.values[2]/SensorManager.GRAVITY_EARTH);

                      // Delta time
                      if (lastDate != 0){
                          bb.putFloat(date.getTime() - lastDate);
                      } else {
                          bb.putFloat(0);
                      }

                      lastDate = date.getTime();

                      dataOutputStream.writeInt(Commands.CMD_ACC_DATA);
                      dataOutputStream.write(bb.array());

                      //dataOutputStream.flush();
                      /*if (connection.getTcpNoDelay())
                      Log.d("tcp no delay", "true");*/

                  } catch (IOException e) {
                      Log.e(LogName, "Error while sending ACC_DATA: " + e.toString());
                  }
                }
		}
		
		public void onAccuracyChanged(Sensor sensor, int accuracy) {
		}
    }
}
