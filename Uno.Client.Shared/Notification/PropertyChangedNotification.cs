using Microsoft.UI.Dispatching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Uno.Client.Notification
{
    public class PropertyChangedNotification : INotifyPropertyChanged
    {
        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        public bool ValidationEnabled { get; set; }

        #region Privates

        private string GetPropertyName(LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException();
            }

            return memberExpression.Member.Name;
        }
        #endregion

        #region Getters and Setters
        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertySelector">Expression tree contains the property definition.</param>
        /// <returns>The value of the property or default value if not exist.</returns>
        protected T GetValue<T>(Expression<Func<T>> propertySelector)
        {
            string propertyName = GetPropertyName(propertySelector);

            return GetValue<T>(propertyName);
        }

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of the property or default value if not exist.</returns>
        protected T GetValue<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            object value;
            if (!_values.TryGetValue(propertyName, out value))
            {
                value = default(T);
                _values.Add(propertyName, value);
            }

            return (T)value;
        }

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertySelector">Expression tree contains the property definition.</param>
        /// <param name="value">The property value.</param>
        protected void SetValue<T>(Expression<Func<T>> propertySelector, T value)
        {

#if WINDOWS
            if (!_dispatcherQueue.HasThreadAccess)
                _dispatcherQueue.TryEnqueue(() => SetValueInternal(propertySelector, value));
            else
                SetValueInternal(propertySelector, value);
#else
            SetValueInternal(propertySelector, value);
#endif
        }
        private void SetValueInternal<T>(Expression<Func<T>> propertySelector, T value)
        {
            string propertyName = GetPropertyName(propertySelector);

            SetValue<T>(propertyName, value);
        }

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The property value.</param>
        protected void SetValue<T>(string propertyName, T value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            _values[propertyName] = value;
            //if(ValidationEnabled)
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private bool _notifyingObjectIsChanged;
        private readonly object _notifyingObjectIsChangedSyncRoot = new Object();

        public bool IsChanged
        {
            get
            {
                lock (_notifyingObjectIsChangedSyncRoot)
                {
                    return _notifyingObjectIsChanged;
                }
            }
            set
            //protected set
            {
                lock (_notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(_notifyingObjectIsChanged, value))
                    {
                        _notifyingObjectIsChanged = value;

                        this.OnPropertyChanged(new PropertyChangedEventArgs("IsChanged"));
                        ValueChanged.Invoke();
                    }
                }
            }
        }
        public event Action ValueChanged;


        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {


                if (e != null && !String.Equals(e.PropertyName, "IsChanged", StringComparison.Ordinal))
                {
                    this.PropertyChanged(this, e);
                    this.IsChanged = false;
                }
            }
        }
        #endregion
    }
}
