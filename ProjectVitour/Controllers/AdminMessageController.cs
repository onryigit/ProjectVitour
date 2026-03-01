using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Services.ContactMessageServices;

namespace ProjectVitour.Controllers
{
    public class AdminMessageController : Controller
    {
        private readonly IContactMessageService _messageService;

        public AdminMessageController(IContactMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<IActionResult> MessageList()
        {
            var values = await _messageService.GetAllMessagesAsync();
            return View(values);
        }

        public async Task<IActionResult> MessageDetails(string id)
        {
            var value = await _messageService.GetMessageByIdAsync(id);
            if (value != null && !value.IsRead)
            {
                await _messageService.MarkAsReadAsync(id);
                value.IsRead = true;
            }
            return View(value);
        }

        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _messageService.MarkAsReadAsync(id);
            return RedirectToAction("MessageList");
        }

        public async Task<IActionResult> DeleteMessage(string id)
        {
            await _messageService.DeleteMessageAsync(id);
            return RedirectToAction("MessageList");
        }
    }
}