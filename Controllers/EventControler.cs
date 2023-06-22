using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListWithAuth.Areas.Identity.Data;
using ToDoListWithAuth.Data;
using ToDoListWithAuth.Models;

namespace ToDoListWithAuth.Controllers
{
	[Authorize]
	public class EventController : Controller
	{
		private readonly EventDBContext _dbContext;
        private readonly UserManager<User> _userManager;

        public EventController(EventDBContext db, UserManager<User> userManager)
		{
			_dbContext = db;
            _userManager = userManager;
        }

		public async Task<IActionResult> Index()
		{
			List<Event> events = await _dbContext.Events.Where(a => a.UserId == _userManager.GetUserId(User)).ToListAsync();
			events.Sort(new EventComparer());
			return View(events);
		}

		public IActionResult Create()
		{
			TempData["UserId"] = _userManager.GetUserId(User);
			return View();
		}
		[HttpPost]
		public IActionResult Create(Event @event)
		{
            TempData["UserId"] = _userManager.GetUserId(User);
            if (@event.Start < DateTime.Now)
			{
				ModelState.AddModelError("Start", "Start cannot be before now");
			}
            if (@event.End < @event.Start)
            {
				ModelState.AddModelError("End", "End cannot be before start");
            }
            if (ModelState.IsValid)
			{
				_dbContext.Events.Add(@event);
				_dbContext.SaveChanges();
				TempData["success"] = "Event added successfully";
				return RedirectToAction("Index");
			}
			return View(@event);
		}

		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				TempData["error"] = "Something went wrong";
				return NotFound();
			}

			var @event = _dbContext.Events.Find(id);

			if (@event == null)
			{
				TempData["error"] = "Something went wrong";
				return NotFound();
			}
            TempData["UserId"] = _userManager.GetUserId(User);
            return View(@event);
		}

		[HttpPost]
		public IActionResult Edit(Event @event)
		{
            TempData["UserId"] = _userManager.GetUserId(User);
            if (@event.Start < DateTime.Now)
            {
                ModelState.AddModelError("Start", "Start cannot be before now");
            }
            if (@event.End < @event.Start)
            {
                ModelState.AddModelError("End", "End cannot be before start");
            }
            if (ModelState.IsValid)
            {
                _dbContext.Events.Update(@event);
                _dbContext.SaveChanges();
                TempData["success"] = "Event edited successfully";
                return RedirectToAction("Index");
            }
            return View(@event);
        }

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				TempData["error"] = "Something went wrong";
				return NotFound();
			}

			var @event = _dbContext.Events.Find(id);

			if (@event == null)
			{
				TempData["error"] = "Something went wrong";
				return NotFound();
			}
            TempData["UserId"] = _userManager.GetUserId(User);
            return View(@event);
		}

		[HttpPost]
		public IActionResult Delete(Event @event)
		{
			_dbContext.Remove(@event);

			_dbContext.SaveChanges();
			TempData["success"] = "Event deleted successfully";

			return RedirectToAction("Index");
		}
	}

	
}
