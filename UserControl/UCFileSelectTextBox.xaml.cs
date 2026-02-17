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

namespace UserControls
{
    /// <summary>
    /// UCFileSelectTextBox.xaml の相互作用ロジック
    /// </summary>
    public partial class UCFileSelectTextBox : System.Windows.Controls.UserControl
    {
        public UCFileSelectTextBox()
        {
            InitializeComponent();
            textBoxFileName.DragEnter += new DragEventHandler(textBoxFileName_DragEnter);

            textBoxFileName.PreviewDragEnter += new DragEventHandler(textBoxFileName_PreviewDragEnter);
            textBoxFileName.PreviewDrop += new DragEventHandler(textBoxFileName_PreviewDrop);

            BitmapImage myBitmapImage = new BitmapImage();

            //myBitmapImage.BeginInit();
            //myBitmapImage.UriSource = new Uri(@"dir.JPG");
            //myBitmapImage.EndInit();

            //imgFileDirSelecter.Source = myBitmapImage;

        }
         

        void textBoxFileName_PreviewDrop(object sender, DragEventArgs e)
        {
            //throw new NotImplementedException();
            textBox_Drop(sender, e);
        }

        void textBoxFileName_PreviewDragEnter(object sender, DragEventArgs e)
        {
            //throw new NotImplementedException();
            textBox_DragEnter(sender, e);
        }

        void textBoxFileName_DragEnter(object sender, DragEventArgs e)
        {
            //throw new NotImplementedException();
            textBox_DragEnter(sender, e);
        }

        public string Text
        {
            get
            {
                return textBoxFileName.Text;
            }

            set
            {
                textBoxFileName.Text = value;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //TODO:フォルダー選択かっこ悪いのでカスタムを作成したい
            System.Windows.Forms.FolderBrowserDialog fbd =
                new System.Windows.Forms.FolderBrowserDialog();

            fbd.SelectedPath = textBoxFileName.Text;
            System.Windows.Forms.DialogResult result = fbd.ShowDialog();
            
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (sender == buttonFileDirSelecter)
                {
                    textBoxFileName.Text = fbd.SelectedPath;
                }
            }
        }

        private void textBox_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "" || textBox.Text == null)
            {
                textBox.ToolTip = null;
            }
            else
            {
                textBox.ToolTip = textBox.Text;
            }
        }

        private void textBox_Drop(object sender, DragEventArgs e)
        {
            //TODO:ドラッグアンドドロップ対応したのだが動かないので調査が必要
            /*
            if (e.Effects == System.Windows.DragDropEffects.Copy)
            {
                TextBox textBox = (TextBox)sender;

                textBox.Text = e.Data.GetData(System.Windows.DataFormats.FileDrop).ToString();
            }
            */
            // ﾌｧｲﾙﾄﾞﾛｯﾌﾟした場合のみ処理
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                foreach (string fileName in (string[])e.Data.GetData(DataFormats.FileDrop))
                {
                    TextBox textBox = (TextBox)sender;
                    textBox.Text = fileName;

                    break;
                }
            }

        }

        private void textBox_DragEnter(object sender, DragEventArgs e)
        {
            //textBoxFrom.Text = "test";
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop)
                || e.Data.GetDataPresent(System.Windows.DataFormats.Text)
                || e.Data.GetDataPresent(System.Windows.DataFormats.UnicodeText))
            {
               e.Effects = System.Windows.DragDropEffects.Copy;
            }
        }
    }
}
