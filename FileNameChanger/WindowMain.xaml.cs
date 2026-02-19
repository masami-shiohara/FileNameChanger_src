//TODO:スレッドの廃棄処理を考慮
//TODO:フェイドアウト　フェイドインで処理中のバーを表示する。
//TODO:処理ファイルをバックグラウンドに流せないか
//TODO:コンソールアプリ、WEBアプリを作成したい
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace FileNameChenger
{
    /// <summary>
    /// WindowMain.xaml の相互作用ロジック
    /// </summary>
    public partial class WindowMain : Window
    {
        const string RunText = "変換実行";
        const string CancelText = "中断";

        //FileNameChengerCore.EStateFlg eStateFlg = FileNameChengerCore.EStateFlg.Non;
        bool isRun = false;
        System.Windows.Forms.Timer stateTimer = null;
        FileNameChengerCore.CFileCopy cFileCopy = null;
        FileNameChengerCore.CConfig cConfig = null;

        public WindowMain()
        {
            InitializeComponent();

            progressBarWork.Opacity = 0;
            //imageWork.Opacity = 0.25;
            imageWork.Opacity = 0.0;

            //progressBarWork.Visibility = Visibility.Hidden;
            //imageWork.Visibility = Visibility.Hidden;

            //btnStoryboard.Storyboard.Completed += new EventHandler(Storyboard_Completed);

            stateTimer = new System.Windows.Forms.Timer();
            stateTimer.Interval = 100;
            stateTimer.Tick += new EventHandler(stateTimer_Tick);


            cConfig = new FileNameChengerCore.CConfig(FileNameChanger.Properties.Resources.ConfigFileName);
            cConfig.getConfig();

            textBoxFrom.Text = cConfig.FileNameFrom;
            textBoxTo.Text = cConfig.FileNameTo;
        }

        bool IsWidthUp = false;
        private void labelSetup_MouseEnter(object sender, MouseEventArgs e)
        {
            if (IsWidthUp)
            {
                Width -= 200;
                Grid.Width -= 200;
            }
            else
            {
                Width += 200;
                Grid.Width += 200;
            }
            IsWidthUp = !IsWidthUp;
        }



        private void setImage()
        {
            string dir = @".\img";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string[] files = Directory.GetFiles(dir, "*.jpg", SearchOption.AllDirectories);


            BitmapImage myBitmapImage = new BitmapImage();

            if (files.Length == 0)
            {
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(@"FileNameChanger.ico", UriKind.RelativeOrAbsolute);
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

        }


        private void buttonRun_Click(object sender, RoutedEventArgs e)
        {
            /*
            switch (eStateFlg)
            {
                case FileNameChengerCore.EStateFlg.Non:
                    buttonRun.Content = RunText;
                    break;
                case FileNameChengerCore.EStateFlg.Run:
                    buttonRun.Content = CancelText;
                    break;
                case FileNameChengerCore.EStateFlg.End:
                case FileNameChengerCore.EStateFlg.Cancel:
                    buttonRun.Content = RunText;
                    break;
                default:
                    break;
                
            }
             */

            //WorkView view = new WorkView();
            //view.ShowDialog();
            
            /*
             */


            
            if (isRun)
            {
                /*
                labelFrom.Visibility = Visibility.Visible;
                labelTo.Visibility = Visibility.Visible;
                textBoxFrom.Visibility = Visibility.Visible;
                textBoxTo.Visibility = Visibility.Visible;
                 */

                //checkBoxViewControl_Run.IsChecked = !checkBoxViewControl_Run.IsChecked;

                if (cFileCopy != null)
                {
                    cFileCopy.Cancel();
                }
                isRun = false;
                buttonRun.Content = RunText;
                Title = FileNameChanger.Properties.Resources.Title;
            }
            else
            {
                /*
                progressBarWork.Visibility = Visibility.Visible;
                imageWork.Visibility = Visibility.Visible;
                 */

                //checkBoxViewControl.IsChecked = !checkBoxViewControl.IsChecked;

                cFileCopy = new FileNameChengerCore.CFileCopy(
                    textBoxFrom.Text, textBoxTo.Text, cConfig.SerchPattern, cConfig.FileNamePattern);
                if (cFileCopy.Count > 0)
                {
                    state = 0;
                    buttonRun.Content = CancelText;
                    if (cFileCopy.FreeSpace)
                    {
                        labelNonFreeSpace.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        labelNonFreeSpace.Visibility = Visibility.Visible;
                    }
                    files.Clear();
                    workingImage.Angle = 0;
                    setImage();
                    progressBarWork.Minimum = 0;
                    progressBarWork.Maximum = cFileCopy.Count;
                    progressBarWork.Value = 0;

                    cFileCopy.DoWorkEndEvent += new EventHandler<EventArgs>(cFileCopy_DoWorkEndEvent);
                    cFileCopy.DoWorkingEvent += new EventHandler<FileNameChengerCore.DoWorkingEventArgs>(cFileCopy_DoWorkingEvent);

                    isRun = true;
                    cFileCopy.Run();


                    stateTimer.Start();

                }
                else
                {
                    cFileCopy = null;
                }
            }
            //btnStoryboard.Storyboard.Begin(this);
        }

        List<string> files = new List<string>();
        void cFileCopy_DoWorkingEvent(object sender, FileNameChengerCore.DoWorkingEventArgs e)
        {
            //throw new NotImplementedException();
            if (files != null)
            {
                files.Add(e.FileNameFrom + "→" + e.FileNameTo);
            }

            state++;
        }

        void cFileCopy_DoWorkEndEvent(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
           //buttonRun.Content = CancelText;
           isRun = false;
        }


        int state = 0;

        void stateTimer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            Title = FileNameChanger.Properties.Resources.Title
                + "(" + state.ToString() + "/" + progressBarWork.Maximum.ToString() + ")";
            /*
            if (files.Count > 0)
            {
                Title = files[0];
                files.RemoveAt(0);
            }
            else
            {
                Title = FileNameChenger.Properties.Resources.Title;
            }
             */

            if (isRun)
            {

                lock (workingImage)
                {
                    if (workingImage.Angle >= 360)
                    {
                        workingImage.Angle = 0;
                        setImage();
                    }
                    else
                    {
                        workingImage.Angle++;
                    }
                }

                //フェードイン
                lock (progressBarWork)
                {
                    if (progressBarWork.Opacity <= 1)
                    {
                        progressBarWork.Opacity += 0.1;
                    }
                    progressBarWork.Value = state;
                }
                if (imageWork.Opacity <= 1)
                {
                    imageWork.Opacity += 0.1;
                }


                //フェードアウト
                if (labelFrom.Opacity >= 0)
                {
                    labelFrom.Opacity -= 0.1;
                }
                if (labelTo.Opacity >= 0)
                {
                    labelTo.Opacity -= 0.1;
                }
                if (textBoxFrom.Opacity >= 0)
                {
                    textBoxFrom.Opacity -= 0.1;
                }
                if (textBoxTo.Opacity >= 0)
                {
                    textBoxTo.Opacity -= 0.1;
                }
            }
            else
            {
                buttonRun.Content = RunText;


                //フェードアウト
                if (progressBarWork.Opacity >= 0)
                {
                    progressBarWork.Opacity -= 0.1;
                }
                if (imageWork.Opacity >= 0)
                {
                    imageWork.Opacity -= 0.1;
                }

                //フェードイン
                if (labelFrom.Opacity <= 1)
                {
                    labelFrom.Opacity += 0.1;
                }
                if (labelTo.Opacity <= 1)
                {
                    labelTo.Opacity += 0.1;
                }
                if (textBoxFrom.Opacity <= 1)
                {
                textBoxFrom.Opacity += 0.1;
                }
                if (textBoxTo.Opacity <= 1)
                {
                    textBoxTo.Opacity += 0.1;
                }

                
                if (labelTo.Opacity == 1)
                {
                    files.Clear();
                    stateTimer.Stop();
                }
            }

        }

        void Storyboard_Completed(object sender, EventArgs e)
        {

            //throw new NotImplementedException();
            /*
            daLabelFrom.From = labelFrom.Opacity;
            daLabelTo.From = labelTo.Opacity;
            daTextBoxFrom.From = textBoxFrom.Opacity;
            daTextBoxTo.From = textBoxTo.Opacity;
            daButtonFrom.From = buttonFrom.Opacity;
            daButtonTo.From = buttonTo.Opacity;

            daProgressBarWork.From = progressBarWork.Opacity;
            daImageWork.From = imageWork.Opacity;
            */
            if (isRun)
            {
                labelFrom.Visibility = Visibility.Hidden;
                labelTo.Visibility = Visibility.Hidden;
                textBoxFrom.Visibility = Visibility.Hidden;
                textBoxTo.Visibility = Visibility.Hidden;

                /*
                daLabelFrom.To = 1;
                daLabelTo.To = 1;
                daTextBoxFrom.To = 1;
                daTextBoxTo.To = 1;
                daButtonFrom.To = 1;
                daButtonTo.To = 1;

                daProgressBarWork.To = 0;
                daImageWork.To = 0;
                 */
            }
            else
            {
                progressBarWork.Visibility = Visibility.Hidden;
                imageWork.Visibility = Visibility.Hidden;
                /*
                daLabelFrom.To = 0;
                daLabelTo.To = 0;
                daTextBoxFrom.To = 0;
                daTextBoxTo.To = 0;
                daButtonFrom.To = 0;
                daButtonTo.To = 0;

                daProgressBarWork.To = 1;
                daImageWork.To = 1;
                 */
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cFileCopy != null)
            {
                cFileCopy.Cancel();
            }

            cConfig.FileNameFrom = textBoxFrom.Text;
            cConfig.FileNameTo = textBoxTo.Text;
            cConfig.putConfig();
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop)
                || e.Data.GetDataPresent(System.Windows.DataFormats.Text)
                || e.Data.GetDataPresent(System.Windows.DataFormats.UnicodeText))
            {
                e.Effects = System.Windows.DragDropEffects.Copy;
            }

        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            // ﾌｧｲﾙﾄﾞﾛｯﾌﾟした場合のみ処理
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                foreach (string fileName in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    Point p = e.GetPosition(this);
                    if (p.Y < (Height/2))
                    {
                        textBoxFrom.Text = fileName;
                    }
                    else
                    {
                    //TextBox textBox = (TextBox)sender;
                    //textBox.Text = fileName;
                        textBoxTo.Text = fileName;
                    }

                    break;
                }
            }

        }
    }
}
