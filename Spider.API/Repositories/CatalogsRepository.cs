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
    public class CatalogsRepository : ICatalogsRepository, IDisposable
    {
        private MSDNContext _context;        
        private readonly ILogger<CatalogsRepository> _logger;
        private CancellationTokenSource _cancellationTokenSource;

        public CatalogsRepository(MSDNContext context,
              ILogger<CatalogsRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));            
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Catalog> GetCatalogAsync(string id)
        {            
            return await _context.Catalogs.FirstOrDefaultAsync(b => b.Id == id);
        }
        
        public async Task<IEnumerable<Catalog>> GetCatalogsAsync()
        {            
            return await _context.Catalogs.ToListAsync();
        }

        public async Task<IEnumerable<Catalog>> GetCatalogsAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await GetCatalogsAsync();
            }
            return await _context.Catalogs.Where(c => c.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) || c.Group.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToListAsync();
        }

        public async Task<IEnumerable<Catalog>> GetCatalogsAsync(IEnumerable<string> catalogIds)
        {
            return await _context.Catalogs.Where(v => catalogIds.Contains(v.Id)).ToListAsync();
        }

        public void AddCatalog(Catalog catalogToAdd)
        {
            if (catalogToAdd == null)
            {
                throw new ArgumentNullException(nameof(catalogToAdd));
            }
            var catalog = _context.Catalogs.SingleOrDefault(c => c.Id == catalogToAdd.Id);
            if (catalog == null)
            {
                _context.Add(catalogToAdd);
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
