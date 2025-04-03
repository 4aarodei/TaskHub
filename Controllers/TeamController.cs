using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        private readonly TaskService _taskService;
        private readonly TeamService _teamService;
        private readonly InviteService _inviteService;

        public TeamController(ApplicationDbContext context, TaskService taskService, TeamService teamService,
            UserService userService, InviteService inviteService)
        {
            _context = context;
            _taskService = taskService;
            _teamService = teamService;
            _userService = userService;
            _inviteService = inviteService;
        }

        // GET: Team
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams.ToListAsync());
        }

        // GET: Team/TeamCreate
        [HttpGet]
        public IActionResult TeamCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TeamCreate([Bind("ID,Name,CreatedDate")] TeamModel teamModel)
        {
            if (ModelState.IsValid)
            {
                teamModel.ID = Guid.NewGuid();
                _context.Add(teamModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(teamModel);
        }

        public async Task<IActionResult> Details(Guid teamId)
        {
            if (teamId != Guid.Empty)
            {
                var team = await _context.Teams
                    .Include(t => t.Users)
                    .Include(t => t.Tasks)
                    .FirstOrDefaultAsync(t => t.ID == teamId);

                if (team == null)
                {
                    return NotFound("Team not found");
                }
                return View(team);
            }
            return NotFound("Team not found");
        }

        [HttpPost("generate-invite")]
        public async Task<IActionResult> GenerateInvite([FromBody] Guid teamId)
        {
            try
            {
                var inviteLink = await _inviteService.GenerateInviteAsync(teamId, Request.Scheme, Request.Host.Value);
                return Ok(new { link = inviteLink });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/TeamInvite/join")]
        public async Task<IActionResult> JoinTeam([FromQuery] string token)
        {
            try
            {
                await _inviteService.JoinTeamAsync(token);
                return Ok("You have been added to the team!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
