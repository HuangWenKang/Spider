using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spider.API.Contexts;
using Spider.API.Entities;
using Spider.API.Repositories;

namespace Spider.API.Repositories
{
    public class VideosRepository : IVideosRepository, IDisposable
    {
        private MSDNContext _context;        
        private readonly ILogger<VideosRepository> _logger;
        private CancellationTokenSource _cancellationTokenSource;

        public VideosRepository(MSDNContext context,
              ILogger<VideosRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));            
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Video> GetVideoAsync(Guid id)
        {            
            return await _context.Videos.FirstOrDefaultAsync(b => b.Id == id);
        }
        
        public async Task<IEnumerable<Video>> GetVideosAsync()
        {            
            return await _context.Videos.ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetVideosAsync(IEnumerable<Guid> videoIds)
        {
            return await _context.Videos.Where(v => videoIds.Contains(v.Id)).ToListAsync();
        }

        public void AddOrUpdateVideo(Video videoToAdd)
        {
            if (videoToAdd == null)
            {
                throw new ArgumentNullException(nameof(videoToAdd));
            }
            var video = _context.Videos.SingleOrDefault(c => c.Id == videoToAdd.Id);
            if (video == null)
            {
                _context.Add(videoToAdd);
            }                       
        }        

        public async Task<bool> SaveChangesAsync()
        {            
            return (await _context.SaveChangesAsync() > 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                }
            }
        }        
    }
}
