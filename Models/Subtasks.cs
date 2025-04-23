namespace TaskHub.Models
{
    public class Subtasks
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
        public int OrderIndex { get; set; }

        // navigations properties
        public TaskModel? TaskModel { get; set; }
        public Guid TaskId { get; set; }
    }
}
