using Microsoft.EntityFrameworkCore;
using TaskHub.Data;
using TaskHub.Models;

namespace TaskHub.Services
{
    public class TeamService
    {
        private readonly ApplicationDbContext _context;

        public TeamService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TeamModel?> GetTeamByIdAsync(Guid teamId)
        {
            return await _context.Teams
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.ID == teamId);
        }

        public async Task<TeamModel> CreateTeamAsync(string teamName)
        {
            var team = new TeamModel
            {
                ID = Guid.NewGuid(),
                Name = teamName,
                CreatedDate = DateTime.UtcNow
            };
            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return team;
        }

        public async Task<List<TeamModel>> GetAllTeamsForUserAsync(string userId)
        {
            return await _context.Teams
                .Include(t => t.Users)
                .Where(t => t.Users.Any(u => u.Id == userId.ToString()))
                .ToListAsync();
        }

        public async Task<List<AppUser>> GetUsersForTeamAsync(Guid TeamId)
        {
            var team = await _context.Teams
                .Include(t => t.Users)
                .FirstOrDefaultAsync(t => t.ID == TeamId);

            return team?.Users ?? new List<AppUser>();
        }

        //public async Task<bool> UpdateTeam()
        //{
           
        //    _context.Teams.Update()

        //}
    }
}
