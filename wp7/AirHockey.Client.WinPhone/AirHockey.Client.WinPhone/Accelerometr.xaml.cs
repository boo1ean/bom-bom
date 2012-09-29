using System;
using System.Collections.Generic;
using AirHockey.Client.WinPhone.Network;
using Microsoft.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace AirHockey.Client.WinPhone
{
    public partial class MainPage
    {
        private Accelerometer _accelerometer;
        private readonly SocketClient _socketClient = GameNetworkClient.GlobalSocketClient;
        private readonly GameNetworkClient _gameClient = GameNetworkClient.GlobalGameNetworkClient;

        public void PlaySound(string soundFile)
        {
            using (var stream = TitleContainer.OpenStream("Content\\" + soundFile))
            {
                var effect = SoundEffect.FromStream(stream);
                FrameworkDispatcher.Update();
                effect.Play();
            }
        }

        public static void Vibrate(double milliseconds)
        {
            VibrateController.Default.Start(TimeSpan.FromMilliseconds(milliseconds));
        }

        public MainPage()
        {
            InitializeComponent();
            _gameClient.ReceivedNotification += changedGameState;

            if(!Accelerometer.IsSupported)
            {
                statusTextBlock.Text = "device does not support accelerometer";
                startAccelerometer();
            }
        }

        void changedGameState(ServerNotifications notification)
        {
            switch (notification)
            {
                case ServerNotifications.Hit:
                    Vibrate(300);
                    PlaySound("Flop.wav");
                    break;
                case ServerNotifications.Win:
                    Vibrate(1000);
                    break;
                case ServerNotifications.FieldUpdated:

                    break;
                case ServerNotifications.Miss:

                    break;
                case ServerNotifications.Goal:

                    break;
                case ServerNotifications.Start:
                    Vibrate(1000);
                    PlaySound("OutHere1.wav");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("notification");
            }
        }

        private void startAccelerometer()
        {
            if(_accelerometer == null)
            {
                _accelerometer = new Accelerometer { TimeBetweenUpdates = TimeSpan.FromMilliseconds(20) };
                _accelerometer.CurrentValueChanged += accelerometer_CurrentValueChanged;
            }

            try
            {
                statusTextBlock.Text = "starting accelerometer.";
                _accelerometer.Start();
            }
            catch (InvalidOperationException)
            {
                statusTextBlock.Text = "unable to start accelerometer.";
            }
        }

        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            Dispatcher.BeginInvoke(() => updateUi(e.SensorReading));
        }

        private void updateUi(AccelerometerReading sensorReading)
        {
            statusTextBlock.Text = "getting data";

            Vector3 acceleration = sensorReading.Acceleration;

            // Show the numeric values.
            xTextBlock.Text = "X: " + acceleration.X.ToString("0.00");
            yTextBlock.Text = "Y: " + acceleration.Y.ToString("0.00");
            zTextBlock.Text = "Z: " + acceleration.Z.ToString("0.00");

            var buffer = new List<byte>
                             {
                                 (byte) ServerCommands.AccelerometerData

                             };
            buffer.AddRange(BitConverter.GetBytes(acceleration.X));
            buffer.AddRange(BitConverter.GetBytes(acceleration.Y));
            buffer.AddRange(BitConverter.GetBytes(acceleration.Z));
            
            _socketClient.Send(buffer.ToArray());
        }
    }
}