namespace ProjectVitour.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendReservationSuccessEmailAsync(string toEmail, string userName, string tourName, string tourDate, string price, int personCount);
    }
}