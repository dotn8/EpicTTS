using System.ComponentModel;
using System.Runtime.CompilerServices;
using EpicTTS.Annotations;

namespace EpicTTS.Models
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual T GetProperty<T>(ref T t, [CallerMemberName] string propertyName = null)
        {
            return t;
        }

        protected virtual void SetProperty<T>(out T t, T value, [CallerMemberName] string propertyName = null)
        {
            t = value;
            OnPropertyChanged(propertyName);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}