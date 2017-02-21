using System.ComponentModel;
using System.Threading;
using System.Windows;


namespace Meesta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker myWorker;

        public MainWindow()
        {
            InitializeComponent();
            ((ViewModel) DataContext).PropertyChanged += OnPropertyChanged;
        }

        private static void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "Status")
            {
                Notification.Send(((ViewModel)sender).Status.Status);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            myWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            myWorker.ProgressChanged += WorkerOnProgressChanged;
            myWorker.DoWork += WorkerOnDoWork;
            myWorker.RunWorkerAsync();
        }

        private void WorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        {
            ((ViewModel) DataContext).Status = StatusView.Of((Status)progressChangedEventArgs.UserState);
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            do
            {
                (sender as BackgroundWorker).ReportProgress(0, CommunicatorService.GetCurrentStatus());
                Thread.Sleep(5000);
            } while (!doWorkEventArgs.Cancel);

            myWorker.ProgressChanged -= WorkerOnProgressChanged;
            myWorker.DoWork -= WorkerOnDoWork;
            myWorker.Dispose();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!myWorker.CancellationPending)
            {
                myWorker.CancelAsync();
            }
        }
    }
}
