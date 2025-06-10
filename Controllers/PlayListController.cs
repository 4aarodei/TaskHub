using Microsoft.AspNetCore.Mvc;
using TaskHub.Models.Playlist;
using Microsoft.Extensions.Caching.Memory;
using TaskHub.Services.PlayListServices;

namespace TaskHub.Controllers;
// Інтерфейс для сервісу

public class PlayListController : Controller
{
    private readonly IWS_Service _wsService;
    private readonly IMemoryCache _cache;
    private readonly PlaylistService _playlistService;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<PlayListController> _logger; // Вже має бути

    // private readonly PlaylistService _playlistService;

    public PlayListController(IWS_Service wsService, IMemoryCache cache, PlaylistService playlistService
         , ILogger<PlayListController> logger, IBackgroundTaskQueue taskQueue)
    {
        _wsService = wsService;
        _cache = cache;
        _playlistService = playlistService;
        _logger = logger;
        _taskQueue = taskQueue;
    }


    [HttpGet]
    public IActionResult Index(DateTime? startDate, DateTime? endDate)
    {
        var actualStartDate = startDate ?? DateTime.Today;
        var actualEndDate = endDate ?? DateTime.Today.AddDays(7);

        var stations = _wsService.GetAll();

        var model = new StationDaySelectionViewModel
        {
            StartDate = actualStartDate,
            EndDate = actualEndDate,
            Stations = stations.Select(s => new StationSelectionItem
            {
                WorkStationID = s.Id,
                StationName = s.Name,
                SelectedDates = new List<DateTime>() // Поки пусто, але тут можна завантажити попередній вибір
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult RegeneratePlaylist(StationDaySelectionViewModel model, string sessionId)
    {
        if (string.IsNullOrEmpty(sessionId))
        {
            _logger.LogInformation("missing session ID");
            return Json(new { success = false });
        }

        _logger.LogInformation("session ID - OK");

        var queries = model.Stations
            .SelectMany(station => station.SelectedDates
                .Select(date => new PlayListQuery(station.WorkStationID, date.Date)))
            .ToList();

        _taskQueue.Enqueue(async token =>
        {
            await _playlistService.BuildDefaultPlaylistsAsync(queries, sessionId, token);
        });

        return Json(new { success = true });
    }


}

