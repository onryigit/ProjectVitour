using MongoDB.Driver;
using ProjectVitour.Entities;
using ProjectVitour.Settings;

namespace ProjectVitour.Services.ContactMessageServices
{
    public class ContactMessageService : IContactMessageService
    {
        private readonly IMongoCollection<ContactMessage> _messageCollection;

        public ContactMessageService(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _messageCollection = database.GetCollection<ContactMessage>(databaseSettings.ContactMessageCollectionName);
        }

        public async Task CreateMessageAsync(ContactMessage message)
        {
            message.SendDate = DateTime.Now;
            message.IsRead = false;
            await _messageCollection.InsertOneAsync(message);
        }

        public async Task DeleteMessageAsync(string id)
        {
            await _messageCollection.DeleteOneAsync(x => x.ContactMessageID == id);
        }

        public async Task<List<ContactMessage>> GetAllMessagesAsync()
        {
            return await _messageCollection.Find(x => true).SortByDescending(x => x.SendDate).ToListAsync();
        }

        public async Task MarkAsReadAsync(string id)
        {
            var update = Builders<ContactMessage>.Update.Set(x => x.IsRead, true);
            await _messageCollection.UpdateOneAsync(x => x.ContactMessageID == id, update);
        }
    }
}