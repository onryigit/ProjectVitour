using MongoDB.Driver;
using ProjectVitour.Entities;
using ProjectVitour.Settings;

namespace ProjectVitour.Services.GuideServices
{
    public class GuideService : IGuideService
    {
        private readonly IMongoCollection<Guide> _guideCollection;

        public GuideService(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _guideCollection = database.GetCollection<Guide>(databaseSettings.GuideCollectionName);
        }

        public async Task CreateGuideAsync(Guide guide)
        {
            await _guideCollection.InsertOneAsync(guide);
        }

        public async Task DeleteGuideAsync(string id)
        {
            await _guideCollection.DeleteOneAsync(x => x.GuideID == id);
        }

        public async Task<List<Guide>> GetAllGuidesAsync()
        {
            return await _guideCollection.Find(x => true).ToListAsync();
        }

        public async Task<Guide> GetGuideByIdAsync(string id)
        {
            return await _guideCollection.Find(x => x.GuideID == id).FirstOrDefaultAsync();
        }

        public async Task UpdateGuideAsync(Guide guide)
        {
            await _guideCollection.FindOneAndReplaceAsync(x => x.GuideID == guide.GuideID, guide);
        }
    }
}