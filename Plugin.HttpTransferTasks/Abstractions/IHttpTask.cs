﻿using System;
using System.ComponentModel;


namespace Plugin.HttpTransferTasks
{
    public interface IHttpTask : INotifyPropertyChanged
    {
        TaskConfiguration Configuration { get; }

        string Identifier { get; }
        bool IsUpload { get; }
        TaskStatus Status { get; }
        string RemoteFileName { get; }
        string LocalFilePath { get; }

        long ResumeOffset { get; }
        long FileSize { get; }
        long BytesTransferred { get; }
        decimal PercentComplete { get; }
        double BytesPerSecond { get; }
        TimeSpan EstimatedCompletionTime { get; }
        DateTimeOffset StartTime { get; }
        // TODO: DateTimeOffset? FinishTime { get; }

        Exception Exception { get; }

        void Start(); // or resume
        void Pause();
        void Cancel();
    }
}