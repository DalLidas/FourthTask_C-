using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FourthTask.ViewModels.Base
{
    internal abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Запрос на обновление
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Решает проблеммы зацикленных запросов на обновлений (замыканий в кольцо)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="PeopserName"></param>
        /// <returns></returns>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string peopserName = null!)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(peopserName);
            return true;
        }

        //~ViewModelBase()
        //{
        //    Dispose(false);
        //}
    
        public void Dispose()
        {
            Dispose(true);
        }
        private bool _Disposed;

        /// <summary>
        /// Освобождение 
        /// </summary>
        /// <param name="Disposing"></param>
        protected virtual void Dispose(bool Disposing)
        {
            if (!Disposing || _Disposed) return;
            _Disposed = true;
            // Освобождение управляемых ресурсов
            // ...
        }
    }
}