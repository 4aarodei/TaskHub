using System;
using System.Collections.Generic;

namespace TaskHub.Models.Playlist
{
    public class PlayList
    {
        // Константи назв полів
        public const string WORKSTATION_ID_FIELD = "WorkstationID";
        public const string DATE_FIELD = "Date";
        public const string UPDATED_FIELD = "Updated";
        public const string LAST_RELATION_CHANGE_FIELD = "LastRelationChange";
        public const string SOUND_VOLUME_FIELD = "SoundVolume";
        public const string PLAYED_FIELD = "Played";

        // Властивості
        public int ID { get; set; }

        public int WorkStationID { get; set; }

        public DateTime Date { get; set; }

        public DateTime Updated { get; set; }

        public DateTime? LastRelationChange { get; set; }

        public string SoundVolume { get; set; }

        public bool Played { get; set; }

        public IList<PlayListItem> Items { get; set; } = new List<PlayListItem>();
    }

    public class PlayListItem
    {
        // navigation property
        private PlayList playList;
        public PlayList PlayList
        {
            get => playList;
            set
            {
                playList = value;
                PlayListID = playList?.ID ?? 0;
            }
        }
        // Константи назв полів
        public const string ID_FIELD = "ID";
        public const string PLAY_LIST_ID_FIELD = "PlayListID";
        public const string CLIP_ID_FIELD = "ClipID";
        public const string START_FIELD = "Start";
        public const string START_POSITION_FIELD = "StartPosition";

        // Властивості
        public int ID { get; set; }
        public int PlayListID { get; set; }
        public int ClipID { get; set; }
        public double Start { get; set; }
        public double StartPosition { get; set; }

        // Фабричний метод
        public static PlayListItem Create(int id, PlayList playList, int clipId, double start, double startPosition)
        {
            return new PlayListItem
            {
                ID = -1,
                PlayList = playList,
                ClipID = clipId,
                Start = start,
                StartPosition = startPosition
            };
        }
    }
}
