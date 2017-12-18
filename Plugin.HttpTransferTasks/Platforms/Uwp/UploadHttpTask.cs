using System;
using System.Threading;
using Windows.Networking.BackgroundTransfer;


namespace Plugin.HttpTransferTasks
{
    public class UploadHttpTask : AbstractUwpHttpTask
    {
        readonly UploadOperation operation;
        readonly bool restart;


        public UploadHttpTask(TaskConfiguration config, UploadOperation operation, bool restart) : base(config, true)
        {
            this.operation = operation;
            this.restart = restart;
            this.Identifier = operation.Guid.ToString();
            this.LocalFilePath = operation.SourceFile.ToString();
        }


        public override void Start()
        {
            var task = this.restart ? this.operation.AttachAsync() : this.operation.StartAsync();
            task.AsTask(
                CancellationToken.None,
                new Progress<UploadOperation>(x => this.SetData(
                    x.Progress.Status,
                    x.Progress.BytesSent,
                    x.Progress.TotalBytesToSend,
                    x.Progress.HasRestarted
                ))
            );
        }

        public override void Pause()
        {

        }


        public override void Cancel()
        {
            this.Status = TaskStatus.Cancelled;
            this.operation.AttachAsync().Cancel();
        }
    }
}
