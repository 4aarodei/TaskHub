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
    public class TeamMenagerController : Controller
    {
        private readonly InviteService _inviteService;
        private readonly TeamService _teamService;
        private readonly UserService _userService;

        public TeamMenagerController(InviteService inviteService, TeamService teamService, UserService userService)
        {

            _inviteService = inviteService;
            _teamService = teamService;
            _userService = userService;
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

        // GET: Team
        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetCurrentUserAsync();

            return View(await _teamService.GetAllTeamsForUserAsync(user.Id));
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
               await _teamService.CreateTeamAsync(teamModel.Name);
                return RedirectToAction("Index");
            }

            return View(teamModel);
        }

        public async Task<IActionResult> Details(Guid teamId)
        {

            var team = await _teamService.GetTeamByIdAsync(teamId);

            if (team == null)
            {
                return NotFound("Team not found");
            }
            return View(team);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid teamId)
        {
            if (teamId == Guid.Empty)
            {
                return NotFound("Team not found");
            }
            var team = await _teamService.GetTeamByIdAsync(teamId);

            if (team == null)
            {
                return NotFound("Team not found");
            }
            return View(team);
        }

        //[HttpPost]
        //public async Task<IActionResult> Edit(TeamModel teamModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _teamService.
        //        await _context.SaveChangesAsync();

        //        return RedirectToAction("Index");
        //    }

        //    return View(teamModel);
        //}

    }
}
