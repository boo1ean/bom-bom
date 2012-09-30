namespace AirHockey.Recognition.Client
{
    using System;
    using System.Drawing;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    using AirHockey.Recognition.Client.ImageProcessing;

    using Point = System.Windows.Point;

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            this.InitializeComponent();
        }

        WebCam webcam;

        private bool mooving;

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            this.webcam = new WebCam();
            this.webcam.InitializeWebCam(this.imgVideo, this.diffImage, this.rects);
            
        }

        private void bntStart_Click(object sender, RoutedEventArgs e)
        {
            this.webcam.Start();
        }

        private void bntStop_Click(object sender, RoutedEventArgs e)
        {
            this.webcam.Stop();
        }

        private void bntContinue_Click(object sender, RoutedEventArgs e)
        {
            this.webcam.Continue();
        }

        private void bntCapture_Click(object sender, RoutedEventArgs e)
        {
            this.webcam.StoreBackgroung();

            this.imgCapture.Source = this.imgVideo.Source;
        }

        private void bntSaveImage_Click(object sender, RoutedEventArgs e)
        {
            PictureHelper.SaveImageCapture((BitmapSource)this.imgCapture.Source);
        }

        private void bntResolution_Click(object sender, RoutedEventArgs e)
        {
            this.webcam.ResolutionSetting();
        }

        private void bntSetting_Click(object sender, RoutedEventArgs e)
        {
            this.webcam.AdvanceSetting();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(canvas);

            Canvas.SetLeft(selection, position.X);
            Canvas.SetTop(selection, position.Y);

            mooving = true;
        }

        private void canvas_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(canvas);

            double X = Canvas.GetLeft(selection);
            double Y = Canvas.GetTop(selection);

            selection.Width = Math.Abs(position.X - X);
            selection.Height = Math.Abs(position.Y - Y);

            mooving = false;

            var result = GetSelectedPixels();

            webcam.SetSelection(result);
        }

        private void canvas_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (!mooving)
            {
                return;
            }

            Point position = e.GetPosition(canvas);

            double X = Canvas.GetLeft(selection);
            double Y = Canvas.GetTop(selection);

            selection.Width = Math.Abs(position.X - X);
            selection.Height = Math.Abs(position.Y - Y);
        }

        private Rectangle GetSelectedPixels()
        {
            double screenX = Canvas.GetLeft(selection);
            double screenY = Canvas.GetTop(selection);

            double screenWidth = selection.Width;
            double screenHeight = selection.Height;

            double sourceWidth = imgVideo.Source.Width;
            double sourceHeight = imgVideo.Source.Height;

            double xScale = canvas.Width / sourceWidth;
            double yScale = canvas.Height / sourceHeight;

            int rectX = (int)(screenX / xScale);
            int rectY = (int)(screenY / yScale);

            int rectWidth = (int)(screenWidth / xScale);
            int rectHeight = (int)(screenHeight / yScale);

            return new Rectangle(rectX, rectY, rectWidth, rectHeight);
        }
    }
}
