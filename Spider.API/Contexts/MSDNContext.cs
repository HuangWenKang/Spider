using Microsoft.EntityFrameworkCore;
using Spider.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider.API.Contexts
{
    public class MSDNContext : DbContext
    {
        public DbSet<Video> Videos { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }

        public MSDNContext(DbContextOptions<MSDNContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            base.OnModelCreating(modelBuilder);
        }
    }
}
