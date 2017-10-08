using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Sample
{
    public abstract class ViewModel : INotifyPropertyChanged
    {

        public virtual void OnActivate()
        {
        }


        public virtual void OnDeactivate()
        {
        }


        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        protected virtual bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(property, value))
                return false;

            property = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
    }
}
