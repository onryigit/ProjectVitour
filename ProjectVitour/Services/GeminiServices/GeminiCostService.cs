using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProjectVitour.Models;

namespace ProjectVitour.Services.GeminiServices
{
    public class GeminiCostService : IGeminiCostService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiCostService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"];
            
            if (string.IsNullOrEmpty(_apiKey) || _apiKey == "YOUR_API_KEY_HERE")
            {
                throw new InvalidOperationException("Gemini API Key is not configured properly.");
            }
        }

        public async Task<GeminiCostResponse> CalculateHiddenCostAsync(string destination, int days, decimal tourPrice, string includedItems)
        {
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

            string prompt = $"Sen bir seyahat acentesi için çalışan ekonomik bütçe danışmanısın. Amacın müşteriyi korkutmadan, en uygun fiyatlı ve sadece ZORUNLU ekstra masrafları hesaplamak. Hedef: {destination}, Süre: {days} gün, Turun Kendi Fiyatı: ${tourPrice}. Şunlar zaten fiyata DAHİL: {includedItems}.\n\nKESİN KURALLAR:\n\nFiyata dahil olan şeyleri ASLA hesaba katma (Örn: Kahvaltı dahilse, sadece ekonomik bir öğle/akşam yemeği veya atıştırmalık hesapla).\n\nRakamları çok ekonomik (budget traveler) seviyesinde tut. Lüks restoranlar veya özel transferler yerine toplu taşıma ve sokak lezzetleri/uygun kafeler düşün.\n\n'Ekstra (Kahve, Bahşiş vb.)' kalemi günlük maksimum 10-15 Doları kesinlikle geçmesin.\n\nToplam ekstra maliyet, turun kendi fiyatının (%20 - %25'ini) mantıken geçmemeli.\n\nLütfen sadece şu JSON yapısını dön, başına veya sonuna hiçbir işaret (markdown vs) ekleme: {{\"Yemek\": {{\"Fiyat\": 0, \"Detay\": \"Sadece öğle/akşam ve atıştırmalık\"}}, \"Ulasim\": {{\"Fiyat\": 0, \"Detay\": \"Toplu taşıma\"}}, \"Muzeler\": {{\"Fiyat\": 0, \"Detay\": \"Önemli girişler\"}}, \"Ekstra\": {{\"Fiyat\": 0, \"Detay\": \"Günlük kahve ve bahşiş\"}}, \"ToplamMaliyet\": 0}}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, jsonContent);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API error: {response.StatusCode} - {errorContent}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(responseString);
            
            var candidates = jsonDocument.RootElement.GetProperty("candidates");
            if (candidates.GetArrayLength() > 0)
            {
                var content = candidates[0].GetProperty("content");
                var parts = content.GetProperty("parts");
                if (parts.GetArrayLength() > 0)
                {
                    var textResponse = parts[0].GetProperty("text").GetString();
                    if (textResponse.StartsWith("```json"))
                    {
                        textResponse = textResponse.Substring(7);
                    }
                    if (textResponse.EndsWith("```"))
                    {
                        textResponse = textResponse.Substring(0, textResponse.Length - 3);
                    }
                    textResponse = textResponse.Trim();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<GeminiCostResponse>(textResponse, options);
                }
            }

            throw new Exception("Gemini API did not return a valid response format.");
        }
    }
}