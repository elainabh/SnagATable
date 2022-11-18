using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snag_That_Table.Data;
using Snag_That_Table.Models.Table;
using Snag_That_Table.Services.TableServices;
using System.Security.Claims;

namespace Snag_That_Table.Controllers
{
    public class TableController : Controller
    {

        private readonly ITableServices _tableService;
        public TableController(ITableServices tableService)
        {
            _tableService = tableService;
        }

        [Authorize]

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userId, out var newId);
            var service = new TableServices(newId);
            var model = service.GetAllTables();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] TableCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userId, out var newId);
            var service = new TableServices(newId);

            if (service.CreateTable(model))
            {
                TempData["SaveResult"] = "Your table was created.";

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Table could not be created.");

            return View(model);

        }

        public async Task<IActionResult> GetAllTables()
        {
            var tableList = await _tableService.GetAllTables();

            if (tableList is null)
            {
                return NotFound();
            }
            return Ok(tableList);
        }

        [HttpPut]
        public async Task<IActionResult> EditRestaurant([FromForm] Guid restaurantId, TableUpdate model)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();
            if (model.RestaurantId != restaurantId) return BadRequest();
            bool wasSuccessful = _tableService.UpdateATable(model);
            if (wasSuccessful) return Ok();
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] int tableId)
        {

            return _tableService.DeleteTable(tableId) ? Ok($"Table was deleted successfully.") : BadRequest($"Table could not be deleted.");
        }

    }
}

    

