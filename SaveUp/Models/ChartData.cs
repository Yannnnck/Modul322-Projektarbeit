using System;

namespace SaveUp.Models
{
    /// <summary>
    /// Repräsentiert Daten für die grafische Darstellung (z. B. in einem Balkendiagramm).
    /// </summary>
    public class ChartData
    {
        /// <summary>
        /// Das Datum, das im Diagramm angezeigt wird (z. B. "01.01.2025").
        /// </summary>
        public string Date { get; set; } = string.Empty;

        /// <summary>
        /// Der Wert, der für das entsprechende Datum dargestellt wird (z. B. gesparter Betrag).
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Die berechnete Höhe des Balkens im Diagramm basierend auf dem Wert.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Die verbleibende Höhe, die möglicherweise für andere Darstellungen oder Berechnungen verwendet wird.
        /// </summary>
        public double RemainingHeight { get; set; }
    }
}