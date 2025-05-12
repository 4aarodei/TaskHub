using Microsoft.VisualStudio.TextTemplating;

namespace TaskHub.Models.Playlist
{
    public class PlayList
    {
        public int ID { get; set; }

        private IList<PlayListItem> items = new List<PlayListItem>();

        public IList<PlayListItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        public const string WORKSTATION_ID_FIELD = "WorkstationID";
        public int WorkStationID { get; set; }

        public const string DATE_FIELD = "Date";

        public DateTime Date { get; set; }

        public const string UPDATED_FIELD = "Updated";
        public DateTime Updated { get; set; }

        public const string LAST_RELATION_CHANGE_FIELD = "LastRelationChange";

        public DateTime? LastRelationChange { get; set; }

        public const string SOUND_VOLUME_FIELD = "SoundVolume";

        public string SoundVolume { get; set; }

        //public List<Pair<int, int>> SoundVolumes
        //{
        //    get
        //    {
        //        return StringHelper.StringToSoundVolumes(SoundVolume);
        //    }
        //    set
        //    {
        //        SoundVolume = StringHelper.SoundVolumesToString(value);
        //    }
        //}

        public const string PLAYED_FIELD = "Played";
        public bool Played { get; set; }

        public bool NeedToRegenerate
        {
            get
            {
                return (LastRelationChange.HasValue && LastRelationChange.Value > Updated);
            }
        }

    }

    public class PlayListItem
    {
        public const string ID_FIELD = "ID";

        public int ID { get; set; }

        private PlayList playList;
        public PlayList PlayList
        {
            get
            {
                return playList;
            }
            set
            {
                playList = value;
                PlayListID = playList.ID;
            }
        }

        public const string PLAY_LIST_ID_FIELD = "PlayListID";
        public int PlayListID { get; set; }

        public const string CLIP_ID_FIELD = "ClipID";

        public int ClipID { get; set; }

        public const string START_FIELD = "Start";

        public double Start { get; set; }

        public const string START_POSITION_FIELD = "StartPosition";
        public double StartPosition { get; set; }

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
