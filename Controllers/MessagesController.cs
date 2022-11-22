using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snag_That_Table.Models.Messages;
using Snag_That_Table.Services.MessageServices;
using System.Security.Claims;
namespace Snag_That_Table.Controllers
{
    public class MessagesController : Controller
    {
        private readonly IMessageServices _messagesService;
        public MessagesController(IMessageServices messagesService)
        {
            _messagesService = messagesService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userId, out var newId);
            _messagesService.SetUserId(newId);
            var model = _messagesService.GetMessagesForUser();

            return View(model);
        }

        public ActionResult Create(Guid restaurantId)
        {
            var model = new MessagesCreate();
            model.RestaurantId = restaurantId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] MessagesCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userId, out var newId);
            _messagesService.SetUserId(newId);

            if (_messagesService.CreateMessages(model))
            {
                TempData["SaveResult"] = "Your message was sent.";

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Message could not be sent.");

            return View(model);

        }

        public async Task<IActionResult> GetUserMessages()
        {
            var tableList =  _messagesService.GetMessagesForRestaurantUser();

            if (tableList is null)
            {
                return NotFound();
            }
            return View(tableList);
        }

        public async Task<IActionResult> GetRestaurantMessages()
        {
            var tableList = _messagesService.GetMessagesForUser();

            if (tableList is null)
            {
                return NotFound();
            }
            return View(tableList);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] int messagesId, Guid userId)
        {

            return _messagesService.DeleteMessage(messagesId, userId) ? Ok($"Message was deleted successfully.") : BadRequest($"Message could not be deleted.");
        }
    }
}
