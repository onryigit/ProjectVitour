using System.Globalization;

namespace ProjectVitour.Helpers
{
    public static class LocalizationHelper
    {
        // 1. Dinamik Başlık ve Açıklama Çekme
        public static string GetLocalizedText(string trText, string enText, string deText)
        {
            var culture = CultureInfo.CurrentCulture.Name;
            
            if (culture.StartsWith("en") && !string.IsNullOrEmpty(enText))
                return enText;
            if (culture.StartsWith("de") && !string.IsNullOrEmpty(deText))
                return deText;

            return trText; // Varsayılan veya TR
        }

        // 2. Dinamik Fiyat ve Para Birimi Formatı (Baz değer USD kabul ediliyor)
        public static string GetLocalizedPrice(decimal priceInUsd)
        {
            var culture = CultureInfo.CurrentCulture.Name;
            
            // Sabit kur çarpanları (Gerçek projede bir API'den alınabilir)
            decimal tryRate = 35.0m;
            decimal eurRate = 0.92m;

            if (culture.StartsWith("tr"))
            {
                decimal tryPrice = priceInUsd * tryRate;
                return tryPrice.ToString("N0", new CultureInfo("tr-TR")) + " ₺";
            }
            else if (culture.StartsWith("de"))
            {
                decimal eurPrice = priceInUsd * eurRate;
                return eurPrice.ToString("N0", new CultureInfo("de-DE")) + " €";
            }
            else // Varsayılan İngilizce (USD)
            {
                return "$" + priceInUsd.ToString("N0", new CultureInfo("en-US"));
            }
        }
    }
}