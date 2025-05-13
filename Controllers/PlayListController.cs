using Microsoft.AspNetCore.Mvc;
using TaskHub.Models.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskHub.Services.PlayListServices;

namespace TaskHub.Controllers
{
    // Інтерфейс для сервісу

    public class PlayListController : Controller
    {
        private readonly IWS_Service _wsService;
        private readonly PlaylistService _playlistService;

        public PlayListController(IWS_Service wsService)
        {
            _wsService = wsService;
        }

        [HttpGet]
        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            var actualStartDate = startDate ?? DateTime.Today;
            var actualEndDate = endDate ?? DateTime.Today.AddDays(7);

            var stations = _wsService.GetAll(); // Наприклад: List<Station> з полями Id, Name

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

        public IActionResult GeneratePlayLists(StationDaySelectionViewModel model)
        {
            var playLists = _playlistService.GeneratePlayListsForSelection(model);

            return View(playLists);
        }

    }
}