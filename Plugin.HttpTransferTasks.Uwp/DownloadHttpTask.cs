using System;
using System.Threading;
using Windows.Networking.BackgroundTransfer;


namespace Plugin.HttpTransferTasks
{
    public class DownloadHttpTask : AbstractUwpHttpTask
    {
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
            var task = this.restart ? this.operation.AttachAsync() : this.operation.StartAsync();
            task.AsTask(
                CancellationToken.None,
                new Progress<DownloadOperation>(x => this.SetData(
                        x.Progress.Status,
                        x.Progress.BytesReceived,
                        x.Progress.TotalBytesToReceive,
                        x.Progress.HasRestarted
                    )
                )
            );
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
