namespace TaskHub.Models.Playlist
{
    public class PlayList
    {
        public bool generet { get; set; } // true - згенеровано false - незгенеровано
    }

    public class PlayListIndexViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<WorkStantion> WsList { get; set; } = new(); // Виправлено CS8619: Додано ініціалізацію за замовчуванням
    }


    // нові моделі
    public class StationDaySelectionViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<StationSelectionItem> Stations { get; set; } = new();
    }

    public class StationSelectionItem
    {
        public Guid StationId { get; set; }
        public string StationName { get; set; }

        // Список обраних днів (дата у форматі yyyy-MM-dd)
        public List<string> SelectedDates { get; set; } = new();
    }

}
