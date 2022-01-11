using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Uno.Client.Notification;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace Uno.Client.ViewModels
{
    public abstract class ViewModelBase : PropertyChangedNotification
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected CoreDispatcher Dispatcher => CoreApplication.MainView.Dispatcher;

        public int Errors { get; set; }
        public int ItemErrors { get; set; }
        public int DirectItemErrors { get; set; }
        public bool IsAdd
        {
            get { return GetValue(() => IsAdd); }
            set { SetValue(() => IsAdd, value); }
        }
        public bool IsEdit
        {
            get { return GetValue(() => IsEdit); }
            set { SetValue(() => IsEdit, value); }
        }
        public bool canChange
        {
            get { return GetValue(() => canChange); }
            set { SetValue(() => canChange, value); }
        }

        // Insert SetProperty below here
        protected virtual bool SetProperty<T>(ref T backingVariable, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingVariable, value)) return false;

            backingVariable = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        // Insert RaisePropertyChanged below here
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DispatchAsync(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        // Insert Dispatch below here
        protected async Task DispatchAsync(DispatchedHandler callback)
        {
            // As WASM is currently single-threaded, and Dispatcher.HasThreadAccess always returns false for broader compatibility reasons
            // the following code ensures the local code always directly invokes the callback on WASM.
            var hasThreadAccess =
#if __WASM__
                true;
#else
                Dispatcher.HasThreadAccess;
#endif

            if (hasThreadAccess)
            {
                callback.Invoke();
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, callback);
            }
        }
    }
}
