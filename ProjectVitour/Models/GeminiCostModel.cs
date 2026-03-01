namespace ProjectVitour.Models
{
    public class GeminiCostResponse
    {
        public CostItem Yemek { get; set; }
        public CostItem Ulasim { get; set; }
        public CostItem Muzeler { get; set; }
        public CostItem Ekstra { get; set; }
        public int ToplamMaliyet { get; set; }
    }

    public class CostItem
    {
        public int Fiyat { get; set; }
        public string Detay { get; set; }
    }
}