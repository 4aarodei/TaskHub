using Microsoft.VisualStudio.TextTemplating;

namespace TaskHub.Models.Playlist
{
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

    public class PlaylistGenerationRequest
    {
        // Ідентифікатор станції
        public Guid StationId { get; set; }

        // Дата для якої генерується плейлист
        public DateTime Date { get; set; }

        // Додаткові властивості для конфігурації плейлиста
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // Можна додати інші дані, які потрібні для генерації плейлиста
        public string PlaylistType { get; set; }

        // Можливо, фільтрація по певних ефірах чи програмах
        public IEnumerable<Guid> ProgramIds { get; set; }

        // Пов'язано з плейлистом
        public List<PlayListItem> Items { get; set; } = new();
    }

    public static class StationDaySelectionMapper
    {
        public static List<PlaylistGenerationRequest> ToGenerationRequests(StationDaySelectionViewModel viewModel)
        {
            var requests = new List<PlaylistGenerationRequest>();

            foreach (var station in viewModel.Stations)
            {
                foreach (var date in station.SelectedDates)
                {
                    // Перетворюємо дату з формату string в DateTime
                    var parsedDate = DateTime.Parse(date);

                    requests.Add(new PlaylistGenerationRequest
                    {
                        StationId = station.StationId,
                        Date = parsedDate,
                        StartTime = TimeSpan.Zero,
                        EndTime = TimeSpan.Zero,
                        PlaylistType = "Default",
                        ProgramIds = new List<Guid>()
                    });
                }
            }

            return requests;
        }
    }

    public class PlaylistGenerationFacade
    {
        private readonly PlaylistGeneratorService _generator;

        public PlaylistGenerationFacade(PlaylistGeneratorService generator)
        {
            _generator = generator;
        }

        public async Task GenerateForMultipleAsync(List<PlaylistGenerationRequest> requests)
        {
            foreach (var request in requests)
            {
                try
                {
                    await _generator.GenerateAsync(request);
                }
                catch (Exception ex)
                {
                    // Логування помилок (для окремих запитів)
                    Console.Error.WriteLine($"Error generating playlist for station {request.StationId} on {request.Date}: {ex.Message}");
                }
            }
        }
    }

    public class PlaylistGeneratorService
    {
        private readonly PlaylistService _playlistService;

        public PlaylistGeneratorService(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        public async Task GenerateAsync(PlaylistGenerationRequest request)
        {
            try
            {
                // Створення нового плейлиста
                var playlist = new PlayList
                {
                    WorkStationID = request.StationId.GetHashCode(), // Можливо, потрібно змінити на правильне значення
                    Date = request.Date,
                    Updated = DateTime.UtcNow,
                    Items = request.Items,
                    SoundVolume = "50", // Значення за замовчуванням
                    Played = false
                };

                // Збереження плейлиста
                await _playlistService.SavePlaylistAsync(playlist);
            }
            catch (Exception ex)
            {
                // Логування помилок при генерації плейлиста
                Console.Error.WriteLine($"Error generating playlist: {ex.Message}");
            }
        }
    }

    public class PlaylistService
    {
        public async Task SavePlaylistAsync(PlayList playlist)
        {
            try
            {
                // Зберігаємо плейлист у БД
                // await _dbContext.Playlists.AddAsync(playlist);
                // await _dbContext.SaveChangesAsync();

                Console.WriteLine($"Playlist for station {playlist.WorkStationID} on {playlist.Date} saved.");
            }
            catch (Exception ex)
            {
                // Логування помилок при збереженні плейлиста
                Console.Error.WriteLine($"Error saving playlist: {ex.Message}");
            }
        }
    }
}