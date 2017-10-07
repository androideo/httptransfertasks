using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Sample
{
    public abstract class ViewModel : INotifyPropertyChanged
    {

        protected virtual void OnActivate()
        {
        }


        protected virtual void OnDeactivate()
        {
        }


        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
