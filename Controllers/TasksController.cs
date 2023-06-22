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
	public class TasksController : Controller
	{
		private readonly ToDoDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public TasksController(ToDoDbContext db, UserManager<User> userManager)
		{
			_dbContext = db;
            _userManager = userManager;
        }

		public async Task<IActionResult> Index()
		{
			List<ToDoTask> tasks = await _dbContext.Tasks.Where(a => a.UserId == _userManager.GetUserId(User)).ToListAsync();
			tasks.Sort(new TaskComparer());
			return View(tasks);
		}

		public IActionResult Create()
		{
			TempData["UserId"] = _userManager.GetUserId(User);
			return View();
		}
		[HttpPost]
		public IActionResult Create(ToDoTask task)
		{
            TempData["UserId"] = _userManager.GetUserId(User);
            if (task.Deadline < DateTime.Now)
			{
				ModelState.AddModelError("Deadline", "Deadline cannot be before today");
			}
            if (ModelState.IsValid)
			{
				_dbContext.Tasks.Add(task);
				_dbContext.SaveChanges();
				TempData["success"] = "Task added successfully";
				return RedirectToAction("Index");
			}
			return View(task);
		}

		public IActionResult ToggleStatus(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var obj = _dbContext.Tasks.Find(id);

			if (obj == null)
			{
				return NotFound();
			}

			obj.IsDone = !obj.IsDone;

			_dbContext.Update(obj);

			_dbContext.SaveChanges();

			return RedirectToAction("Index");
		}

		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				TempData["error"] = "Something went wrong";
				return NotFound();
			}

			var task = _dbContext.Tasks.Find(id);

			if (task == null)
			{
				TempData["error"] = "Something went wrong";
				return NotFound();
			}
            TempData["UserId"] = _userManager.GetUserId(User);
            return View(task);
		}

		[HttpPost]
		public IActionResult Edit(ToDoTask task)
		{
            TempData["UserId"] = _userManager.GetUserId(User);
            if (task.Deadline < DateTime.Now)
			{
				ModelState.AddModelError("Deadline", "Deadline cannot be before today");
			}
			if (ModelState.IsValid)
			{
				_dbContext.Tasks.Update(task);
				_dbContext.SaveChanges();
				TempData["success"] = "Task edited successfully";
				return RedirectToAction("Index");
			}
			return View(task);
		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				TempData["error"] = "Something went wrong";
				return NotFound();
			}

			var task = _dbContext.Tasks.Find(id);

			if (task == null)
			{
				TempData["error"] = "Something went wrong";
				return NotFound();
			}
            TempData["UserId"] = _userManager.GetUserId(User);
            return View(task);
		}

		[HttpPost]
		public IActionResult Delete(ToDoTask task)
		{
			_dbContext.Remove(task);

			_dbContext.SaveChanges();
			TempData["success"] = "Task deleted successfully";

			return RedirectToAction("Index");
		}
	}

	
}
