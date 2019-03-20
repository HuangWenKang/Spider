using Spider.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider.API.Repositories
{
    public interface IVideosRepository
    {                
        Task<IEnumerable<Video>> GetVideosAsync();

        Task<IEnumerable<Video>> GetVideosAsync(IEnumerable<Guid> videoIds);
        
        Task<Video> GetVideoAsync(Guid id);

        void AddOrUpdateVideo(Video videoToAdd);        

        Task<bool> SaveChangesAsync();
    }
}
