using System;

namespace SaveUp.Models
{
    /// <summary>
    /// Repräsentiert ein Produkt, das in der "SaveUp"-App erfasst wird.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Name des Produkts, das erfasst wird.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Kommentar oder zusätzliche Beschreibung zum Produkt.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Preis oder Betrag, der durch den Verzicht auf das Produkt gespart wurde.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Datum und Uhrzeit, zu der das Produkt in die Liste aufgenommen wurde.
        /// </summary>
        public DateTime DateAdded { get; set; }
    }
}
