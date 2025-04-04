namespace TaskHub.Models.TaskViewModel
{
    public class IndexModel
    {
        public IndexModel(List<TaskModel> taskList, TeamModel teamModel)
        {
            TaskList = taskList;
            TeamModel = teamModel;
        }
        public List<TaskModel> TaskList { get; set; }
        public TeamModel TeamModel { get; set; }
    }
}
