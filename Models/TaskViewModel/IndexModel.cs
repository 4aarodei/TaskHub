namespace TaskHub.Models.TaskViewModel
{
    public class IndexModel
    {
        public IndexModel(List<TaskModel> taskList, TeamModel teamModel, List<AppUser> usersOnTeam, List<TaskModel> teamTasks)
        {
            TaskList = taskList;
            TeamModel = teamModel;
            UsersOnTeam = usersOnTeam;
            TeamTasks = teamTasks;
        }
        public List<AppUser> UsersOnTeam { get; set; }
        public List<TaskModel> TaskList { get; set; }
        public List<TaskModel> TeamTasks { get; set; }

        public TeamModel TeamModel { get; set; }
    }
}
