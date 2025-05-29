using Microsoft.AspNetCore.Mvc;
using TaskHub.Models.Playlist;
using Microsoft.Extensions.Caching.Memory;
using TaskHub.Services;
using TaskHub.Services.PlayListServices;

namespace TaskHub.Controllers
{
    // Інтерфейс для сервісу

    public class PlayListController : Controller
    {
        private readonly IWS_Service _wsService;
        private readonly IMemoryCache _cache;
        private readonly PlaylistService _playlistService;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IPlaylistGeneratorService _generator;
        private readonly ILogger<PlayListController> _logger; // Вже має бути

        // private readonly PlaylistService _playlistService;

        public PlayListController(IWS_Service wsService, IMemoryCache cache, PlaylistService playlistService, IPlaylistGeneratorService generator, IBackgroundTaskQueue taskQueue, ILogger<PlayListController> logger)
        {
            _wsService = wsService;
            _cache = cache;
            _playlistService = playlistService;
            _generator = generator;
            _taskQueue = taskQueue;
            _logger = logger;
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

        public class StartGenerationRequest
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string ConnectionId { get; set; }
        }

        [HttpPost]
        public IActionResult StartGeneration([FromBody] StartGenerationRequest request)
        {
            _logger.LogInformation("PlayListController.StartGeneration: Received HTTP POST request.");

            if (request == null)
            {
                _logger.LogWarning("PlayListController.StartGeneration: Request model is null after deserialization. Client sent invalid data.");
                return BadRequest(new { Message = "Невірні дані запиту." });
            }

            // Додаткове логування отриманих даних
            _logger.LogInformation("PlayListController.StartGeneration: Request data received: StartDate='{StartDate}', EndDate='{EndDate}', ConnectionId='{ConnectionId}'",
                                   request.StartDate, request.EndDate, request.ConnectionId);

            // Перевірка коректності ConnectionId
            if (string.IsNullOrWhiteSpace(request.ConnectionId))
            {
                _logger.LogWarning("PlayListController.StartGeneration: ConnectionId is null or empty. This might prevent SignalR updates.");
                // Можливо, тут варто повернути BadRequest або генерувати ID на сервері, якщо ConnectionId є критичним.
            }

            _logger.LogDebug("PlayListController.StartGeneration: Queueing background work item using IBackgroundTaskQueue.");
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                _logger.LogInformation("PlayListController.StartGeneration.BackgroundWorker: Starting execution for ConnectionId='{ConnectionId}'.", request.ConnectionId);
                try
                {
                    await _generator.GenerateAsync(request.ConnectionId, request.StartDate, request.EndDate, token);
                    _logger.LogInformation("PlayListController.StartGeneration.BackgroundWorker: Generator finished for ConnectionId='{ConnectionId}'.", request.ConnectionId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "PlayListController.StartGeneration.BackgroundWorker: An error occurred during background generation for ConnectionId='{ConnectionId}'.", request.ConnectionId);
                    // Тут можна додати логіку для відправки повідомлення про помилку через SignalR, якщо потрібно
                }
            });

            _logger.LogInformation("PlayListController.StartGeneration: Background work item successfully queued. Sending OK response with ConnectionId.");
            // Цей рядок обов'язково має бути таким, щоб клієнт отримав JSON
            return Ok(new { Message = "Фонова генерація розпочата успішно.", ConnectionId = request.ConnectionId });
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePlayLists(StationDaySelectionViewModel model)
        {
            if (model == null || model.Stations == null)
            {
                return BadRequest("Invalid model data.");
            }

            try
            {
                var queries = model.Stations
                    .SelectMany(station => station.SelectedDates
                        .Select(date => new PlayListQuery(station.WorkStationID, date.Date)))
                    .ToList();

                if (!queries.Any())
                {
                    return Json(new { Errcode = "No dates or stations selected for generation." });
                }

                // Використовуємо наш PlaylistService
                var results = await _playlistService.BuildDeafultPlayLists(queries);

                IList<PlayList> pls = results.Select(result => result).ToList();

                // _playlistService.SetPlayLists(pls); // Зберігаємо згенеровані плейлисти

                return Json(new { Errcode = false, Message = "Playlists generated and saved successfully." });
            }
            catch (Exception ex)
            {
                // Замість StackTrace у публічному API, краще логувати і повертати загальну помилку
                return Json(new { Errcode = true, Message = ex.Message, DebugInfo = ex.StackTrace });
            }
        }
    }
    public class CityServiceLocator
    {
        public class PlayListLogic
        {
            public async Task<List<PlayListResult>> BuildDeafultPlayLists(List<PlayListQuery> queries)
            {
                await Task.Delay(7000); // затримка 7 секунд

                return new List<PlayListResult>();
            }

            public static void SetPlayLists(IList<PlayList> playLists)
            {
                // Реалізація логіки для збереження плейлистів
            }
        }
    }

    public class PlayListResult
    {
    }
    
        // [HttpPost]
        // public IActionResult GeneratePlayLists([FromBody] StationDaySelectionViewModel model)
        // {
        //     try
        //     {
        //         var queries = model.Stations
        //             .SelectMany(station => station.SelectedDates
        //                 .Select(date => new PlayListQuery(station.WorkStationID, date.Date)))
        //             .ToList();
        //
        //         Task.Run(() =>
        //         {
        //             int total = queries.Count;
        //             var allPlayLists = new List<PlayList>();
        //
        //             // Колекція для зберігання помилок
        //             var errors = new List<object>();
        //
        //             for (int i = 0; i < total; i++)
        //             {
        //                 var query = queries[i];
        //
        //                 try
        //                 {
        //                     var result = _playlistService.BuildDeafultPlayLists(new List<PlayListQuery> { query });
        //
        //                     if (result != null && result.Count > 0)
        //                     {
        //                         allPlayLists.AddRange(result.Select(r => r.PlayList));
        //                     }
        //                 }
        //                 catch (Exception ex)
        //                 {
        //                     // Якщо сталася помилка, записуємо її з деталями:
        //                     errors.Add(new
        //                     {
        //                         WorkStationId = query.WorkStationId,
        //                         Date = query.Date,
        //                         Message = ex.Message,
        //                         StackTrace = ex.StackTrace
        //                     });
        //                 }
        //
        //                 // Оновлюємо прогрес (у відсотках)
        //                 double progress = (i + 1) * 100.0 / total;
        //                 _cache.Set("playlist-progress", progress);
        //
        //                 // Оновлюємо список помилок у кеші (перезаписуємо щоразу)
        //                 _cache.Set("playlist-errors", errors);
        //
        //                 // Thread.Sleep(100); // для симуляції затримки (необов’язково)
        //             }
        //
        //             // Зберігаємо всі успішно створені плейлисти
        //
        //             // Тут можна зберегти плейлисти у базу даних або в кеш
        //
        //             // Після завершення прибираємо прогрес (можна залишити errors для звіту)
        //             _cache.Remove("playlist-progress");
        //         });
        //
        //         return Json(new { started = true });
        //     }
        //     catch (Exception ex)
        //     {
        //         return Json(new { Errcode = ex.Message, Exception = ex.StackTrace });
        //     }
        // }

}