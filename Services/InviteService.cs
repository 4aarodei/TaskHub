using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskHub.Data;
using TaskHub.Models;

namespace TaskHub.Services
{
    public class InviteService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        public InviteService(ApplicationDbContext context, UserService userService)
        {
            _userService = userService;
            _context = context;
        }

        public async Task<string> GenerateInviteAsync(Guid teamId, string requestScheme, string requestHost)
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            var team = await _context.Teams
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.ID == teamId);

            if (team == null)
            {
                throw new KeyNotFoundException("Team not found");
            }

            

            var invite = new TeamInvite
            {
                Id = Guid.NewGuid(),
                TeamId = teamId,
                CreatedAt = DateTime.UtcNow
            };

            _context.TeamInvites.Add(invite);
            await _context.SaveChangesAsync();

            return $"{requestScheme}://{requestHost}/TeamInvite/join?token={invite.Id}";
        }

        public async Task JoinTeamAsync(string token)
        {
            if (!Guid.TryParse(token, out var inviteToken))
            {
                throw new ArgumentException("Invalid token");
            }

            var invite = await _context.TeamInvites
                .FirstOrDefaultAsync(i => i.Id == inviteToken);

            if (invite == null)
            {
                throw new KeyNotFoundException("Invite not found or expired");
            }

            if (invite.CreatedAt.AddMinutes(15) < DateTime.UtcNow)
            {
                _context.TeamInvites.Remove(invite);
                await _context.SaveChangesAsync();
                throw new InvalidOperationException("This invite has expired.");
            }

            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }

            var team = await _context.Teams
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.ID == invite.TeamId);

            if (team == null)
            {
                throw new KeyNotFoundException("Team not found");
            }

            if (!team.Users.Contains(user))
            {
                team.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            _context.TeamInvites.Remove(invite);
            await _context.SaveChangesAsync();
        }
    }
}