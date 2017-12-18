using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Plugin.HttpTransferTasks;
using Plugin.HttpTransferTasks.Rx;
using Xamarin.Forms;


namespace Sample
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            this.NewTask = new Command(async () =>
                await App.Current.MainPage.Navigation.PushAsync(new NewTaskPage())
            );
            this.CancelAll = new Command(CrossHttpTransfers.Current.CancelAll);
            this.Tasks = new ObservableCollection<HttpTaskViewModel>();

            CrossHttpTransfers
                .Current
                .WhenListChanged()
                .Where(x => x.Change == TaskListChange.Add)
                .Subscribe(x => Device.BeginInvokeOnMainThread(() =>
                    this.Tasks.Insert(0, new HttpTaskViewModel(x.Task)))
                );
        }


        public ICommand NewTask { get; }
        public ICommand CancelAll { get; }
        public ObservableCollection<HttpTaskViewModel> Tasks { get; }
    }
}
