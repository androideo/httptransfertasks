using System;
using System.Reactive;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;


namespace Plugin.HttpTransferTasks.Rx
{
    public static class RxExtensions
    {
        public static IObservable<IHttpTask> WhenDataChanged(this IHttpTask task) => task
            .WhenPropertyChanged(x => x.BytesTransferred)
            .Select(x => task);


        public static IObservable<TaskStatus> WhenStatusChanged(this IHttpTask task) =>
            task.WhenPropertyChanged(x => x.Status);


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


        public static IObservable<TRet> WhenPropertyChanged<TSender, TRet>(this TSender This, Expression<Func<TSender, TRet>> expression) where TSender : INotifyPropertyChanged
        {
            var p = This.GetPropertyInfo(expression);

            return Observable
                .FromEventPattern<PropertyChangedEventArgs>(This, nameof(INotifyPropertyChanged.PropertyChanged))
                .StartWith(new EventPattern<PropertyChangedEventArgs>(This, new PropertyChangedEventArgs(p.Name)))
                .Where(x => x.EventArgs.PropertyName == p.Name)
                .Select(x =>
                {
                    var value = (TRet)p.GetValue(This);
                    return value;
                });
        }


        public static PropertyInfo GetPropertyInfo<TSender, TRet>(this TSender sender, Expression<Func<TSender, TRet>> expression)
        {
            var lambda = expression as LambdaExpression;
            var member = lambda.Body as MemberExpression;
            var property = sender.GetType().GetRuntimeProperty(member.Member.Name);
            return property;
        }
    }
}
