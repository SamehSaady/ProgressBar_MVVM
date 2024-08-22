using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProgressBar_MVVM.Utils
{
    /// <summary>
    /// Provides a base class for implementing the INotifyPropertyChanged interface and simplifies the property change notification process.
    /// </summary>
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        /// An event that triggers when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;



        /// <summary>
        /// Raises the <b><see cref="PropertyChanged"/></b> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. The default value is the name of the caller member.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Called in the setter of a Property to Set its value and raises the <b><see cref="PropertyChanged"/></b> event if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="backingField">A reference to the field storing the property's value.</param>
        /// <param name="value">The new value of the property.</param>
        /// <param name="onValueChange">Action to be called if the value has changed.
        /// <param name="propertyName">The name of the property.
        /// <br> The default value is the name of the caller member (Property name). </br></param>
        /// <returns><b>true</b> if the value has changed; otherwise, <b>false</b>.</returns>
        protected virtual bool SetProperty<T>(ref T backingField, T value, Action onValueChange = null, [CallerMemberName] string propertyName = null)
        {
            if (Equals(backingField, value))
                return false;
            
            backingField = value;
            OnPropertyChanged(propertyName);

            if (onValueChange != null)
                onValueChange();

            return true;
        }
    }
}
