using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using System.Timers;

namespace RecordingDeviceSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int secondsDelay = 5;
        private FileInfo[] fileList;
        private bool stopFlag= true;
        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.speedBox.Text = secondsDelay.ToString();
            this.DataContext = this;
        }

        private void OpenPathDialog(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();
            var dialogResult = openFolderDialog.ShowDialog();

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = openFolderDialog.SelectedPath;
            }
        }

        private void IncreaseSpeed(object sender, RoutedEventArgs e)
        {
            secondsDelay++;
            this.speedBox.Text = secondsDelay.ToString();
        }

        private void DecreaseSpeed(object sender, RoutedEventArgs e)
        {
            secondsDelay--;
            this.speedBox.Text = secondsDelay.ToString();
        }

        private void SendPicturesStream(object source, ElapsedEventArgs e)
        {
            if (FilePath != string.Empty || FilePath != "...")
            {
                DirectoryInfo dirInfo = new DirectoryInfo(FilePath);
                fileList = dirInfo.GetFiles("*.*");
            }

            ImageSource imageSource;
            foreach (FileInfo f in fileList)
            {
                //imageSource = new BitmapImage(new Uri(f.FullName));
                //this.image.Source = imageSource;
                ImageSource = new BitmapImage(new Uri(f.FullName));

               if (stopFlag)
                break;
            }
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            stopFlag = true;
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            stopFlag = false;
            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Elapsed += new ElapsedEventHandler(SendPicturesStream);
            myTimer.Interval = secondsDelay*1000; // 1000 ms is one second
            myTimer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }
    }
}
