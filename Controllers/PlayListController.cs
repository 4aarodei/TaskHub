using Microsoft.AspNetCore.Mvc;
using TaskHub.Models.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskHub.Controllers
{
    // Інтерфейс для сервісу
    public interface IWS_Service
    {
        List<WorkStantion> GetAll();
        bool GenerationFunk(List<WorkStantion> WSList);
    }

    // Псевдореалізація сервісу
    public class FakeWS_Service : IWS_Service
    {
        public List<WorkStantion> GetAll()
        {
            return new List<WorkStantion>
            {
                new WorkStantion { Id = Guid.NewGuid(), Name = "WS1", PlaylistState = false},
                new WorkStantion { Id = Guid.NewGuid(), Name = "WS2", PlaylistState = false },
                new WorkStantion { Id = Guid.NewGuid(), Name = "WS3", PlaylistState = false }
            };
        }

        public bool GenerationFunk(List<WorkStantion> WSList)
        {
            // Імітація генерації
            return true;
        }
    }


    public class PlayListController : Controller
    {
        private readonly IWS_Service _wsService;

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
                    StationId = s.Id,
                    StationName = s.Name,
                    SelectedDates = new List<string>() // Поки пусто, але тут можна завантажити попередній вибір
                }).ToList()
            };

            return View(model);
        }



        [HttpPost]
        public IActionResult SaveSelection(StationDaySelectionViewModel model)
        {
            foreach (var station in model.Stations)
            {
                var id = station.StationId;
                var selectedDates = station.SelectedDates;
                // Збереження або логіка
            }

            // Можна зробити редирект назад
            return RedirectToAction("Index", new { startDate = model.StartDate, endDate = model.EndDate });
        }

    }
}
