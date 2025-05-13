using TaskHub.Controllers;
using TaskHub.Models.Playlist;

namespace TaskHub.Services.PlayListServices
{
    public interface IWS_Service
    {
        List<WorkStation> GetAll();
    }

    public class FakeWS_Service : IWS_Service
    {
        public List<WorkStation> GetAll()
        {
            return new List<WorkStation>
            {
                new WorkStation { Id = 1, Name = "WS1", PlaylistState = false},
                new WorkStation { Id = 2, Name = "WS2", PlaylistState = false },
                new WorkStation { Id = 3, Name = "WS3", PlaylistState = false }
            };
        }
    }
}
