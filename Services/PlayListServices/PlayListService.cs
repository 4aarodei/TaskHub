using System.Globalization;
using System.Reflection.Emit;
using TaskHub.Models.Playlist;

namespace TaskHub.Services.PlayListServices;

public class PlaylistService
{
    public async Task<List<PlayList>> BuildDeafultPlayLists(List<PlayListQuery> queries)
    {
        var results = new List<PlayList>();
        
        await Task.Delay(2000); // затримка 2 секунди

        foreach (var query in queries)
        {
            // Імітуємо створення плейлисту
            var playlist = new PlayList
            {
                ID = query.WorkStationId * 1000 + query.Date.Day, // Унікальний Id для прикладу
                WorkStationID = query.WorkStationId,
                Date = query.Date,
                Updated = DateTime.Now,
                SoundVolume = "Medium",
                Played = false
            };

            // Імітуємо додавання кількох PlayListItem
            playlist.Items.Add(PlayListItem.Create(-1, playlist, clipId: 101, start: 0, startPosition: 0));
            playlist.Items.Add(PlayListItem.Create(-1, playlist, clipId: 102, start: 10, startPosition: 5));
            playlist.Items.Add(PlayListItem.Create(-1, playlist, clipId: 103, start: 20, startPosition: 15));

            results.Add(playlist);
        }

        return results;
    }
}