using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SaveUp.ViewModels
{
    /// <summary>
    /// Basisklasse für alle ViewModels in der Anwendung.
    /// Implementiert das Interface <see cref="INotifyPropertyChanged"/>, um Änderungen an Eigenschaften zu verfolgen und die Benutzeroberfläche zu aktualisieren.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Ereignis, das ausgelöst wird, wenn sich der Wert einer Eigenschaft ändert.
        /// Dieses Ereignis wird verwendet, um die Benutzeroberfläche zu benachrichtigen, dass sich die Daten geändert haben.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Benachrichtigt die Benutzeroberfläche über Änderungen an einer Eigenschaft.
        /// </summary>
        /// <param name="propertyName">
        /// Der Name der Eigenschaft, die geändert wurde.
        /// Der Parameter wird automatisch mit dem Namen der aufrufenden Eigenschaft gefüllt.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            // Löst das PropertyChanged-Ereignis aus, um die UI über die Änderung zu informieren.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Aktualisiert den Wert eines Feldes und benachrichtigt die Benutzeroberfläche über die Änderung.
        /// </summary>
        /// <typeparam name="T">Der Typ des Feldes.</typeparam>
        /// <param name="field">Das Feld, dessen Wert geändert werden soll.</param>
        /// <param name="value">Der neue Wert, der dem Feld zugewiesen werden soll.</param>
        /// <param name="propertyName">
        /// Der Name der aufrufenden Eigenschaft.
        /// Dieser Wert wird automatisch mit dem Namen der aufrufenden Eigenschaft gefüllt.
        /// </param>
        /// <returns>
        /// Gibt `true` zurück, wenn der Wert des Feldes geändert wurde, andernfalls `false`.
        /// </returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null!)
        {
            // Überprüft, ob der neue Wert gleich dem aktuellen Wert ist.
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            // Setzt das Feld auf den neuen Wert.
            field = value;

            // Benachrichtigt die UI über die Änderung der Eigenschaft.
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}