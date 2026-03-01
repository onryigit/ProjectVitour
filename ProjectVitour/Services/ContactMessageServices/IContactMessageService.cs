namespace ProjectVitour.Services.ContactMessageServices
{
    public interface IContactMessageService
    {
        Task<List<Entities.ContactMessage>> GetAllMessagesAsync();
        Task<Entities.ContactMessage> GetMessageByIdAsync(string id);
        Task CreateMessageAsync(Entities.ContactMessage message);
        Task DeleteMessageAsync(string id);
        Task MarkAsReadAsync(string id);
    }
}