namespace TaskHub.Models.TaskViewModel
{
    public class CreateTaskViewModel
    {
        public CreateTaskViewModel(List<AppUser> usersOnTeam, TeamModel team)
        {
            UsersOnTeam = usersOnTeam;
            Team = team;
        }

        public List<AppUser> UsersOnTeam { get; set; }
        public TeamModel Team { get; set; }
    }
}
