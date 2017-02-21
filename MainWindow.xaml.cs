using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;


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
            ((ViewModel) DataContext).StatusChangedManually += OnStatusChanged;
        }

        private void OnStatusChanged(object sender, StatusChangedManuallyEventArgs eventArgs)
        {
            Notification.Send(eventArgs.Status);
            if (SyncCheckbox.IsChecked.HasValue && SyncCheckbox.IsChecked.Value && eventArgs.Manually)
            {
                CommunicatorService.SetStatus(eventArgs.Status);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!CommunicatorService.IsRunning())
            {
                ((CheckBox) sender).IsChecked = false;
                return;
            }

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
            ((ViewModel)DataContext).ChangeStatusAutomatically((Status) progressChangedEventArgs.UserState);
        }

        private void WorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            do
            {
                (sender as BackgroundWorker).ReportProgress(0, CommunicatorService.GetCurrentStatus());
                Thread.Sleep(5000);
            } while (myWorker != null && !myWorker.CancellationPending);

            if (myWorker == null)
            {
                return;
            }
            myWorker.ProgressChanged -= WorkerOnProgressChanged;
            myWorker.DoWork -= WorkerOnDoWork;
            myWorker.Dispose();
            myWorker = null;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (myWorker != null && !myWorker.CancellationPending)
            {
                myWorker.CancelAsync();
            }
        }
    }
}
