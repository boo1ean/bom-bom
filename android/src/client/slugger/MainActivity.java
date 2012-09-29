package client.slugger;

import java.io.DataOutputStream;
import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.UnknownHostException;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.Date;

import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;
import android.os.StrictMode;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.widget.EditText;
import android.app.Activity;
import android.content.Context;

public class MainActivity extends Activity {
	
	private SensorManager mSensorManager;
	private Sensor mAccelerometer;
	private Socket connection;
	AccReader accReader;
	final static String HOST_NAME = "192.168.1.2";
	final static int    HOST_PORT = 9595;
	private DataOutputStream dataOutputStream;
	
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();

        StrictMode.setThreadPolicy(policy);
        mSensorManager = (SensorManager) getSystemService(Context.SENSOR_SERVICE);
        mAccelerometer = mSensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);
		try {
			connection = new Socket();
			Log.d("socket", "created");
			connection.connect(new InetSocketAddress(HOST_NAME, HOST_PORT), 1000);
			Log.d("socket", "connected");
			if (connection.isConnected()) 
				setupAll();
				
		} catch (UnknownHostException e) {
			finish();
		} catch (IOException e) {
			finish();
		}
    }
   
    
    public void onBackPressed() {
		Log.d("finish", "");
		mSensorManager.unregisterListener(accReader);
		try {
			connection.close();
		} catch (IOException e) {}
		finish();
  	 }
    
    public void sendName(String name) throws IOException {
    	dataOutputStream.writeInt(2);
    	dataOutputStream.writeInt(name.length());
        ByteBuffer bb = ByteBuffer.allocate(2*name.length()).order(ByteOrder.LITTLE_ENDIAN);
        for(int i=0;i<name.length();i++)
        	bb.putChar(name.charAt(i));
    	dataOutputStream.write(bb.array());    	
    }
    
    public void setupAll() throws IOException {
		connection.setTcpNoDelay(true);
		dataOutputStream = new DataOutputStream(connection.getOutputStream());
		dataOutputStream.writeInt(1);
		dataOutputStream.writeInt(1);
		accReader = new AccReader();
		mSensorManager.registerListener(accReader, mAccelerometer, SensorManager.SENSOR_DELAY_UI);
		EditText textMessage = (EditText)findViewById(R.id.editText1);
		sendName(textMessage.getText().toString());
	    textMessage.addTextChangedListener(new TextWatcher(){
	        public void afterTextChanged(Editable s) {
	            try {
	            	sendName(s.toString());
				} catch (IOException e) {
					// TODO Auto-generated catch block				e.printStackTrace();
				}
	        }
	        public void beforeTextChanged(CharSequence s, int start, int count, int after){}
	        public void onTextChanged(CharSequence s, int start, int before, int count){}
	    }); 	    	    	
    }
    
    class ResponseData
    {
    }
    
    boolean gravityKnown = false;
    float gravity[] = new float[3];
    class AccReader implements SensorEventListener
    {
        long lastDate=0; 
		public void onSensorChanged(SensorEvent event) {
			if (event.sensor.getType() == Sensor.TYPE_ACCELEROMETER) {
			  try {
				  ByteBuffer bb = ByteBuffer.allocate(16).order(ByteOrder.LITTLE_ENDIAN);
  				  bb.putFloat(event.values[0]/SensorManager.GRAVITY_EARTH);
				  bb.putFloat(event.values[1]/SensorManager.GRAVITY_EARTH);
				  bb.putFloat(event.values[2]/SensorManager.GRAVITY_EARTH);/**/
				  if (lastDate!=0)
					  bb.putFloat((new Date()).getTime()-lastDate);/**/
				  else
					  bb.putFloat(0);
				  lastDate = (new Date()).getTime();
				  dataOutputStream.writeInt(0);
				  dataOutputStream.write(bb.array());
				  //dataOutputStream.flush();
				  /*if (connection.getTcpNoDelay())
					  Log.d("tcp no delay", "true");*/				  

			  } catch (IOException e) {
				onBackPressed(); 				//e.printStackTrace();
			  }
			  //onBackPressed();
			}
		}
		
		public void onAccuracyChanged(Sensor sensor, int accuracy) {
			// TODO Auto-generated method stub
			
		}
    }
    
}
