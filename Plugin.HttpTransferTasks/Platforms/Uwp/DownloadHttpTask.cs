using System;
using System.Threading;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;


namespace Plugin.HttpTransferTasks
{
    public class DownloadHttpTask : AbstractUwpHttpTask
    {
        IAsyncOperationWithProgress<DownloadOperation, DownloadOperation> task;
        readonly DownloadOperation operation;
        readonly bool restart;


        public DownloadHttpTask(TaskConfiguration config, DownloadOperation operation, bool restart) : base(config, false)
        {
            //var task = new BackgroundDownloader
            //{
            //    Method = config.HttpMethod,
            //    CostPolicy = config.UseMeteredConnection
            //        ? BackgroundTransferCostPolicy.Default
            //        : BackgroundTransferCostPolicy.UnrestrictedOnly
            //};
            //foreach (var header in config.Headers)
            //    task.SetRequestHeader(header.Key, header.Value);

            //var filePath = config.LocalFilePath ?? Path.Combine(ApplicationData.Current.LocalFolder.Path, Path.GetRandomFileName());
            //var fileName = Path.GetFileName(filePath);
            //var directory = Path.GetDirectoryName(filePath);

            //// why are these async??
            //var folder = StorageFolder.GetFolderFromPathAsync(directory).AsTask().Result;
            //var file = folder.CreateFileAsync(fileName).AsTask().Result;

            //var operation = task.CreateDownload(new Uri(config.Uri), file);
            //var httpTask = new DownloadHttpTask(config, operation, false);
            this.operation = operation;
            this.restart = restart;
            this.Identifier = this.operation.Guid.ToString();
            this.LocalFilePath = this.operation.ResultFile.Path;
        }


        public override void Start()
        {
            //this.operation.Resume();
            this.task = this.restart ? this.operation.AttachAsync() : this.operation.StartAsync();
            this.task.Progress = (result, progress) => this.SetData(
                progress.Progress.Status,
                progress.Progress.BytesReceived,
                progress.Progress.TotalBytesToReceive,
                progress.Progress.HasRestarted
            );
            this.task.Completed = (op1, op2) =>
            {
                switch (op2)
                {
                    case AsyncStatus.Canceled:
                        this.Status = TaskStatus.Cancelled;
                        break;

                    case AsyncStatus.Completed:
                        this.Status = TaskStatus.Completed;
                        break;

                    case AsyncStatus.Error:
                        this.Status = TaskStatus.Error;
                        break;

                    case AsyncStatus.Started:
                        this.Status = TaskStatus.Running;
                        break;
                }
            };
        }


        public override void Pause()
        {
            this.Status = TaskStatus.Paused;
            this.operation.Pause();
        }


        public override void Cancel()
        {
            this.operation.AttachAsync().Cancel();
            this.Status = TaskStatus.Cancelled;
        }
    }
}
