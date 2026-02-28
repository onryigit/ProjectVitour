using MongoDB.Driver;
using ProjectVitour.Entities;
using ProjectVitour.Settings;

namespace ProjectVitour.Services.DestinationServices
{
    public class DestinationService : IDestinationService
    {
        private readonly IMongoCollection<Destination> _destinationCollection;

        public DestinationService(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _destinationCollection = database.GetCollection<Destination>(databaseSettings.DestinationCollectionName);
        }

        public async Task CreateDestinationAsync(Destination destination)
        {
            await _destinationCollection.InsertOneAsync(destination);
        }

        public async Task DeleteDestinationAsync(string id)
        {
            await _destinationCollection.DeleteOneAsync(x => x.DestinationID == id);
        }

        public async Task<List<Destination>> GetAllDestinationsAsync()
        {
            return await _destinationCollection.Find(x => true).ToListAsync();
        }

        public async Task<Destination> GetDestinationByIdAsync(string id)
        {
            return await _destinationCollection.Find(x => x.DestinationID == id).FirstOrDefaultAsync();
        }

        public async Task UpdateDestinationAsync(Destination destination)
        {
            await _destinationCollection.FindOneAndReplaceAsync(x => x.DestinationID == destination.DestinationID, destination);
        }
    }
}