using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SaveUp.Converters
{
    /// <summary>
    /// Konverter, der einen Prozentsatz und die Gesamtbreite verwendet, um die tatsächliche Breite eines Fortschrittsbalkens zu berechnen.
    /// </summary>
    public class PercentageToWidthConverter : IMultiValueConverter
    {
        /// <summary>
        /// Konvertiert den Fortschrittsprozentsatz und die Gesamtbreite in die Breite des Fortschrittsbalkens.
        /// </summary>
        /// <param name="values">Ein Array mit Werten:
        /// [0] Der Fortschrittsprozentsatz (als double).
        /// [1] Die Gesamtbreite (als double).</param>
        /// <param name="targetType">Der Zieltyp (in der Regel nicht verwendet).</param>
        /// <param name="parameter">Zusätzliche Parameter (in der Regel nicht verwendet).</param>
        /// <param name="culture">Die aktuelle Kulturinformation (falls benötigt).</param>
        /// <returns>Die berechnete Breite des Fortschrittsbalkens oder 0 bei Fehlern.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Überprüfe, ob die erwarteten Werte im Array vorhanden sind
            if (values[0] is double progressPercentage && values[1] is double totalWidth)
            {
                // Berechne die Breite basierend auf dem Prozentsatz und der Gesamtbreite
                return progressPercentage * totalWidth;
            }

            // Rückgabewert bei ungültigen Eingaben
            return 0;
        }

        /// <summary>
        /// Nicht implementierte Methode, um den Umkehrvorgang durchzuführen (nicht erforderlich in diesem Fall).
        /// </summary>
        /// <param name="value">Der Wert, der zurückkonvertiert werden soll.</param>
        /// <param name="targetTypes">Die Zieltypen (in der Regel nicht verwendet).</param>
        /// <param name="parameter">Zusätzliche Parameter (in der Regel nicht verwendet).</param>
        /// <param name="culture">Die aktuelle Kulturinformation (falls benötigt).</param>
        /// <returns>Wirft eine NotImplementedException, da der Umkehrvorgang nicht implementiert ist.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
