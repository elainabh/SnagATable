using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snag_That_Table.Data;
<<<<<<< HEAD
=======
using Snag_That_Table.Models.Messages;
>>>>>>> 29a9784 (fingers crossed)
using Snag_That_Table.Models.RestaurantModels;  
using Snag_That_Table.Services.RestaurantServices;
using System.Security.Claims;

namespace Snag_That_Table.Controllers
{
    public class RestaurantController : Controller
    {

        private readonly IRestaurantServices _restaurantService;
        public RestaurantController(IRestaurantServices restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [Authorize]

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userId, out var newId);
            _restaurantService.SetUserId(newId);
            var model = _restaurantService.GetAllRestaurants();
            
            return View(model);
        }

<<<<<<< HEAD
=======
        public ActionResult Create(Guid ownerId)
        {
            var model = new RestaurantCreate();
            model.OwnerId = ownerId;
            return View(model);
        }

>>>>>>> 29a9784 (fingers crossed)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] RestaurantCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userId, out var newId);
            _restaurantService.SetUserId(newId);

            if (_restaurantService.CreateRestaurant(model))
            {
                TempData["SaveResult"] = "Your restaurant was created.";

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Restaurant could not be created.");

            return View(model);

        }

        public async Task<IActionResult> GetAllRestaurants()
        {
            var restaurantList = _restaurantService.GetAllRestaurants();

            if (restaurantList is null)
            {
                return NotFound();
            }
            return Ok(restaurantList);
        }

        [HttpPut]
<<<<<<< HEAD
        public async Task<IActionResult> EditRestaurant([FromForm] RestaurantUpdate model, Guid restaurantId)
=======
        public async Task<IActionResult> Edit([FromForm] RestaurantUpdate model, Guid restaurantId)
>>>>>>> 29a9784 (fingers crossed)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();
            if (model.RestaurantId != restaurantId) return BadRequest();
            bool wasSuccessful = _restaurantService.UpdateRestaurant(model, restaurantId);
            if (wasSuccessful) return Ok();
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] Guid restaurantId, string restaurantName)
        {

            return _restaurantService.DeleteARestaurant(restaurantId) ? Ok($"{restaurantName} was deleted successfully.") : BadRequest($"{restaurantName} could not be deleted.");
        }

    }
}
