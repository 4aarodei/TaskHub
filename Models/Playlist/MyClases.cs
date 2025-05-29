using Microsoft.VisualStudio.TextTemplating;
using System.Globalization;
using TaskHub.Models.Playlist;
using TaskHub.Services.PlayListServices;

namespace TaskHub.Models.Playlist;

public class StationDaySelectionViewModel // отримуємо після Index
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public List<StationSelectionItem> Stations { get; set; } = new();
}

public class StationSelectionItem
{
    public int WorkStationID { get; set; }
    public string StationName { get; set; }

    // Список обраних днів (дата у форматі yyyy-MM-dd)
    public List<DateTime> SelectedDates { get; set; } = new();
} 
 // father classes
public class PlayListQuery
{
    public int WorkStationId { get; private set; }
    public DateTime Date { get; private set; }

    public PlayListQuery(int wsId, DateTime date)
    {
        WorkStationId = wsId;
        Date = date;
    }
}

public class BuildPlayListResult
{
    public PlayListQuery Query { get; set; }
    public PlayList PlayList { get; set; }
}

