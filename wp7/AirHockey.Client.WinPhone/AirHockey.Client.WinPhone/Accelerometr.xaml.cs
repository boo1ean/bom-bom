using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AirHockey.Client.WinPhone.Network;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;

namespace AirHockey.Client.WinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Accelerometer accelerometer;
        private SocketClient socketClient = SocketClient.Client;
        


        public MainPage()
        {
            InitializeComponent();

            if(!Accelerometer.IsSupported)
            {
                statusTextBlock.Text = "device does not support accelerometer";
                startAccelerometer();
            }
        }

        private void startAccelerometer()
        {
            if(accelerometer == null)
            {
                accelerometer = new Accelerometer();
                accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);
            }

            try
            {
                statusTextBlock.Text = "starting accelerometer.";
                accelerometer.Start();
            }
            catch (InvalidOperationException ex)
            {
                statusTextBlock.Text = "unable to start accelerometer.";
            }
        }

        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            Dispatcher.BeginInvoke(() => UpdateUI(e.SensorReading));
        }

        private void UpdateUI(AccelerometerReading sensorReading)
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
            
            socketClient.Send(buffer.ToArray());
        }
    }
}