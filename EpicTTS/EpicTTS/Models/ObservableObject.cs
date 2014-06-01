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

        protected virtual bool SetProperty<T>(ref T t, T value, [CallerMemberName] string propertyName = null)
        {
            var original = t;
            t = value;
            if (Equals(original, t))
                return false;
            OnPropertyChanged(propertyName);
            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}