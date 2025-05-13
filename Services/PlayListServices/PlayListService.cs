using System.Globalization;
using System.Reflection.Emit;
using TaskHub.Models.Playlist;

namespace TaskHub.Services.PlayListServices;

public class PlaylistService
{
    private readonly PlaylistService _playlistService;

    public PlaylistService(PlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    public List<PlayList> BuildPlaylist(StationDaySelectionViewModel model)
    {
        var playLists = new List<PlayList>();

        return playLists;
    }

    /// Основний метод для створення плейлистів на основі моделі з view.
    public List<PlayList> GeneratePlayListsForSelection(StationDaySelectionViewModel selection)
    {
        var playLists = new List<PlayList>();

        foreach (var station in selection.Stations)
        {
            // 1. Отримати WorkStationID по StationId
            int workStationId = station.WorkStationID;

            foreach (var selectedDate in station.SelectedDates)
            {
                // 2. Парсимо дату
                DateTime date = selectedDate;

                // 3. Створюємо PlayList
                var playList = CreatePlayList(workStationId, date);

                // 4. Генеруємо елементи
                playList.Items = GeneratePlayListItems(playList, station.WorkStationID, date);

                // 5. Додаємо в список
                playLists.Add(playList);
            }
        }

        return playLists;
    }

    /// Створює об'єкт PlayList із дефолтними значеннями.
    private PlayList CreatePlayList(int workStationId, DateTime date)
    {
        var playList = new PlayList
        {
            ID = 0, // Якщо збереження в БД – потім згенерується
            WorkStationID = workStationId,
            Date = date,
            Updated = DateTime.Now,
            SoundVolume = "", // Або за замовчуванням
            Played = false,
        };
        playList.Items = GeneratePlayListItems(playList, workStationId, date);
        return playList;
    }

    /// Генерує список PlayListItem для заданої станції та дати.
    /// Якщо не потрібні – просто повертай порожній список.
    private IList<PlayListItem> GeneratePlayListItems(PlayList playList, int WorkStationID, DateTime date)
    {
        // // TODO: Реалізуй логіку, якщо є джерело кліпів (наприклад, репозиторій)
        // // Можеш поки повертати порожній список, якщо немає даних
        // return new List<PlayListItem>();
        //
        // // Приклад: повертаємо два елементи плейлиста       
        return new List<PlayListItem>
        {
            PlayListItem.Create(1, playList, 123, 0.0, 0.0),
            PlayListItem.Create(2, playList, 124, 30.0, 0.0)
        };
    }
}