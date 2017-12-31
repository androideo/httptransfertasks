using System;
using System.Reactive.Linq;
using Acr;


namespace Plugin.HttpTransferTasks
{
    public static class RxExtensions
    {
        public static IObservable<IHttpTask> WhenDataChanged(this IHttpTask task) => task
            .WhenAnyValue(x => x.BytesTransferred)
            .Select(x => task);


        public static IObservable<TaskStatus> WhenStatusChanged(this IHttpTask task) =>
            task.WhenAnyValue(x => x.Status);


        public static IObservable<string> WhenCompleted(this IHttpTask task) => task
            .WhenStatusChanged()
            .Where(x => x == TaskStatus.Completed)
            .Select(_ => task.LocalFilePath);


        public static IObservable<TaskListEventArgs> WhenListChanged(this IHttpTransferTasks tasks) => Observable.Create<TaskListEventArgs>(ob =>
        {
            var handler = new EventHandler<TaskListEventArgs>((sender, args) => ob.OnNext(args));
            tasks.CurrentTasksChanged += handler;
            return () => tasks.CurrentTasksChanged -= handler;
        });
    }
}
