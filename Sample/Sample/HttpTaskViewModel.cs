using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Humanizer;
using Plugin.HttpTransferTasks;
using Plugin.HttpTransferTasks.Rx;
using Xamarin.Forms;


namespace Sample
{
    public class HttpTaskViewModel : ViewModel
    {
        readonly IHttpTask task;
        IDisposable taskSub;
        IDisposable statusSub;


        public HttpTaskViewModel(IHttpTask task)
        {
            this.task = task;

            this.Cancel = new Command(task.Cancel);
            this.Actions = new Command(() =>
            {
                switch (task.Status)
                {
                    case TaskStatus.Completed:
                        // TODO: need completion time
                        //task.StartTime
                        var msg = task.IsUpload ? "Upload Completed" : "Download Completed";
                        UserDialogs.Instance.Alert(msg, "COMPLETED");
                        break;

                    case TaskStatus.Paused:
                        task.Start();
                        break;

                    case TaskStatus.PausedByCostedNetwork:
                        UserDialogs.Instance.Alert("Paused due to costed network", "Error");
                        break;

                    case TaskStatus.PausedByNoNetwork:
                        UserDialogs.Instance.Alert("Paused due to no network", "Error");
                        break;

                    case TaskStatus.Running:
                    case TaskStatus.Resumed:
                    case TaskStatus.Retrying:
                        task.Pause();
                        break;

                    case TaskStatus.Error:
                        UserDialogs.Instance.Alert(task.Exception.ToString(), "Error");
                        break;

                    default:
                        UserDialogs.Instance.Alert("Unknown status", "Error");
                        break;
                }
            });
        }


        public override void OnActivate()
        {
            this.taskSub = this.task
                .WhenDataChanged()
                .Sample(TimeSpan.FromSeconds(1))
                .Subscribe(x =>
                    Device.BeginInvokeOnMainThread(() => this.OnPropertyChanged(String.Empty))
                );

            this.statusSub = this.task
                .WhenStatusChanged()
                .Subscribe(x =>
                    Device.BeginInvokeOnMainThread(() => this.OnPropertyChanged(nameof(Status)))
                );
        }


        public override void OnDeactivate()
        {
            this.taskSub?.Dispose();
            this.statusSub?.Dispose();
        }


        public ICommand Actions { get; }
        public ICommand Cancel { get; }
        public string Identifier => this.task.Identifier;
        public bool IsUpload => this.task.IsUpload;
        public TaskStatus Status => this.task.Status;
        public string Uri => this.task.Configuration.Uri;
        public decimal PercentComplete => this.task.PercentComplete;
        public string TransferSpeed => Math.Round(this.task.BytesPerSecond.Bytes().Kilobytes, 2) + " Kb/s";
        public string EstimateMinsRemaining => Math.Round(this.task.EstimatedCompletionTime.TotalMinutes, 1) + " min(s)";


        protected virtual void OnTaskPropertyChanged(object sender, PropertyChangedEventArgs args)
            => Device.BeginInvokeOnMainThread(() => this.OnPropertyChanged(String.Empty));
    }
}
