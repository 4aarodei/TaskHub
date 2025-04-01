using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskHub.Data;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Controllers
{
    public class TaskController : Controller
    {
        //private readonly ApplicationDbContext _context;

        private readonly UserService _userService;
        private readonly TaskService _taskService;

        public TaskController(/*ApplicationDbContext context,*/ UserService userService,TaskService taskService )
        {
            //_context = context;

            _userService = userService;
            _taskService = taskService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
            {
                return RedirectToAction(/* силка на логін */);
            }

          var userTasks = await _taskService.GetAllTasksByUserIdAsync(user.Id);

            return View(userTasks);
        }

        // GET: Task/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        // GET: Task/Create
        public IActionResult Create()
        {


            return View();
        }

        // POST: Task/Create
        [HttpPost]
        public async Task<IActionResult> Create(TaskModel task)
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: Task/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        // POST: Task/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(TaskModel task)
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: Task/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }

        // POST: Task/Delete/{id}
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }

    }
}