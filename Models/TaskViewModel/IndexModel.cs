namespace TaskHub.Models.TaskViewModel
{
    public class IndexModel
    {
        public IndexModel(List<TaskModel> taskWithoutUser, List<AppUser> usersOnTeam, List<TaskModel> userTaskList, List<TaskModel> allTeamTasks, TeamModel teamModel)
        {
            TaskWithoutUser = taskWithoutUser;
            UsersOnTeam = usersOnTeam;
            UserTaskList = userTaskList;
            AllTeamTasks = allTeamTasks;
            TeamModel = teamModel;
        }

        public List<TaskModel> TaskWithoutUser { get; set; }
        public List<AppUser> UsersOnTeam { get; set; }
        public List<TaskModel> UserTaskList { get; set; }
        public List<TaskModel> AllTeamTasks { get; set; }

        public TeamModel TeamModel { get; set; }
    }
}
