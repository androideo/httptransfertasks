using System;
//using Plugin.Notifications;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]


namespace Sample
{
    public partial class App : Application
    {
        //const string NOT_TITLE = "HTTP Transfers";


        public App()
        {
            this.InitializeComponent();
            this.MainPage = new NavigationPage(new MainPage());
        }


        //protected override void OnStart()
        //{
        //    base.OnStart();

        //    CrossHttpTransfers.Current.CurrentTasksChanged += (sender, args) =>
        //    {
        //        switch (args.Change)
        //        {
        //            case TaskListChange.Add:
        //                args.Task.PropertyChanged += this.OnTaskPropertyChanged;
        //                break;

        //            case TaskListChange.Remove:
        //                args.Task.PropertyChanged -= this.OnTaskPropertyChanged;
        //                break;
        //        }
        //    };
        //}


        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    CrossNotifications.Current.SetBadge(0);
        //}


        //async void OnTaskPropertyChanged(object sender, PropertyChangedEventArgs args)
        //{
        //    if (args.PropertyName != nameof(IHttpTask.Status))
        //        return;

        //    var task = (IHttpTask) sender;
        //    var type = task.IsUpload ? "Upload" : "Download";

        //    var cn = CrossNotifications.Current;
        //    if (task.Status == TaskStatus.Error)
        //    {
        //        Debug.WriteLine(task.Exception.ToString());
        //        await cn.Send(new Notification
        //        {
        //            Title = NOT_TITLE,
        //            Message = $"[ERROR] {task.Configuration.Uri} - {task.Exception}"
        //        });
        //    }
        //    else
        //    {
        //        await cn.Send(new Notification
        //        {
        //            Title = NOT_TITLE,
        //            Message = $"{type} of {task.RemoteFileName} to {task.Configuration.Uri} finished with status: {task.Status}"
        //        });
        //        var badge = await cn.GetBadge();
        //        await cn.SetBadge(badge + 1);
        //    }
        //}
    }
}
