using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.IO;



namespace FileNameChenger
{
    /// <summary>
    /// WorkView.xaml の相互作用ロジック
    /// </summary>
    public partial class WorkView : Window
    {

        //Timer stateTimer = null;
        System.Windows.Forms.Timer stateTimer = null;
        public WorkView()
        {
            InitializeComponent();
//            Run();

            //this.Content = FileNameChenger.Properties.Resources.LineageII.darkelf;
            //BitmapImage myBitmapImage = new BitmapImage();
            //myBitmapImage.StreamSource = FileNameChenger.Properties.Resources.LineageII.darkelf.;

            string[] files = Directory.GetFiles(@".\img", "*.jpg", SearchOption.AllDirectories);
            

            BitmapImage myBitmapImage = new BitmapImage();

            if (files.Length == 0)
            {
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(@"FileNameChenger.ico", UriKind.RelativeOrAbsolute);
                myBitmapImage.EndInit();
            }
            else
            {
                Random r = new Random();
                int index = r.Next(files.Length - 1);
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(@"file://" + Directory.GetCurrentDirectory() + files[index], UriKind.RelativeOrAbsolute);
                myBitmapImage.EndInit();
            }
            
            imageWork.Source = myBitmapImage;


            //imagStoryboard.Storyboard.Stop();
            progressBarWork.Minimum = 0;
            progressBarWork.Maximum = 100;

            /*
            AutoResetEvent autoEvent     = new AutoResetEvent(false);

                 // Create the delegate that invokes methods for the timer.
            TimerCallback timerDelegate = 
                new TimerCallback(CheckStatus);


            stateTimer =
                     new Timer(timerDelegate, autoEvent, 1000, 250);
             */
            stateTimer = new System.Windows.Forms.Timer();
            stateTimer.Interval = 1000;
            stateTimer.Tick += new EventHandler(stateTimer_Tick);
            stateTimer.Start();
        }

        /*
        public override ~WorkView()
        {
            if (stateTimer != null)
            {
                stateTimer.Dispose();
            }
        }
         */

        void stateTimer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            lock (workingImage)
            {
                workingImage.Angle = angle;
            }

            lock (progressBarWork)
            {
                progressBarWork.Value = angle;
            }
        }
        /*
        public void CheckStatus(Object stateInfo)
        {
            lock (workingImage)
            {
                workingImage.Angle = angle;
            }

            lock (progressBarWork)
            {
                progressBarWork.Value = angle;
            }
        }
         */

        private void Run()
        {
            DoWorkingEvent += new EventHandler<EventArgs>(WorkView_DoWorkingEvent);

            
            ThreadStart threadDelegate = new ThreadStart(DoWork);
            Thread newThread = new Thread(threadDelegate);
            newThread.Start();
            
            //DoWork();

            /*四角アニメーション
            NameScope.SetNameScope(this, new NameScope());

            this.Title = "Fading Rectangle Example";
            StackPanel myPanel = new StackPanel();
            myPanel.Margin = new Thickness(10);

            Rectangle myRectangle = new Rectangle();
            myRectangle.Name = "myRectangle";
            this.RegisterName(myRectangle.Name, myRectangle);
            myRectangle.Width = 100;
            myRectangle.Height = 100;
            myRectangle.Fill = Brushes.Blue;

            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 1.0;
            myDoubleAnimation.To = 0.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            myDoubleAnimation.AutoReverse = true;
            myDoubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            myStoryboard = new Storyboard();
            myStoryboard.Children.Add(myDoubleAnimation);
            Storyboard.SetTargetName(myDoubleAnimation, myRectangle.Name);
            //Storyboard.SetTargetName(myDoubleAnimation, imageWork.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Rectangle.OpacityProperty));

            // Use the Loaded event to start the Storyboard.
            myRectangle.Loaded += new RoutedEventHandler(myRectangleLoaded);
            //imageWork.Loaded += new RoutedEventHandler(myRectangleLoaded);

            myPanel.Children.Add(myRectangle);
            //myPanel.Children.Add(imageWork);
            this.Content = myPanel;
            */

            /* ボタン　アニメーション
            StackPanel myStackPanel = new StackPanel();
            myStackPanel.Margin = new Thickness(20);


            // Create and set the Button.
            Button aButton = new Button();
            aButton.Content = "A Button";

            // Animate the Button's Width.
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 75;
            myDoubleAnimation.To = 300;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(5));
            myDoubleAnimation.AutoReverse = true;
            myDoubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // Apply the animation to the button's Width property.
            aButton.BeginAnimation(Button.WidthProperty, myDoubleAnimation);

            // Create and animate a Brush to set the button's Background.
            SolidColorBrush myBrush = new SolidColorBrush();
            myBrush.Color = Colors.Blue;

            ColorAnimation myColorAnimation = new ColorAnimation();
            myColorAnimation.From = Colors.Blue;
            myColorAnimation.To = Colors.Red;
            myColorAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(7000));
            myColorAnimation.AutoReverse = true;
            myColorAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // Apply the animation to the brush's Color property.
            myBrush.BeginAnimation(SolidColorBrush.ColorProperty, myColorAnimation);
            aButton.Background = myBrush;

            // Add the Button to the panel.
            myStackPanel.Children.Add(aButton);
            this.Content = myStackPanel;
            */


            //imagStoryboard.Storyboard.Begin(this);
            //imagStoryboard.Storyboard.Stop(this);
            //imagStoryboard.Storyboard.Pause(this);
        }

        /*
        private Storyboard myStoryboard;
        private void myRectangleLoaded(object sender, RoutedEventArgs e)
        {
            myStoryboard.Begin(this);
        }
         */


        int angle = 0;
        void WorkView_DoWorkingEvent(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            /*
            lock (workingImage)
            {
                workingImage.Angle++;
            }
             */
            angle++;
           
       }

        bool IsCancel = false;
        private void DoWork() 
        {
            while(!IsCancel)
            {
                OnDoWorkingEvent(new EventArgs());
                Thread.Sleep(1000);
            }
        }

        public event EventHandler<EventArgs> DoWorkingEvent;
        protected virtual void OnDoWorkingEvent(EventArgs e)
        {
            EventHandler<EventArgs> handler = DoWorkingEvent;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            IsCancel = true;
        }

        private void buttonRun_Click(object sender, RoutedEventArgs e)
        {
            Run();
        }



        private void imageWork_Loaded(object sender, RoutedEventArgs e)
        {
            imagStoryboard.Storyboard.Stop(this);
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            imagStoryboard.Storyboard.Stop(this);
        }
    }
}

