using Spider.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider.API.Repositories
{
    public interface ICatalogsRepository
    {                
        Task<IEnumerable<Catalog>> GetCatalogsAsync();

        Task<IEnumerable<Catalog>> GetCatalogsAsync(IEnumerable<string> catalogIds);
        
        Task<Catalog> GetCatalogAsync(string id);

        void AddCatalog(Catalog catalogToAdd);

        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Catalog>> GetCatalogsAsync(string keyword);
    }
}
