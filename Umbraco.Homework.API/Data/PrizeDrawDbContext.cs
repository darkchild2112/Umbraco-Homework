using System;
using Microsoft.EntityFrameworkCore;
using Umbraco.Homework.API.Models;

// Migration Commands...
// dotnet-ef migrations add <name>
// dotnet-ef migrations remove
// dotnet-ef database update

namespace Umbraco.Homework.API.Data
{
    public class PrizeDrawDbContext : DbContext
    {
        public PrizeDrawDbContext(DbContextOptions<PrizeDrawDbContext> options)
            : base(options)
        {

        }

        public DbSet<PrizeDrawEntry> PrizeDrawEntries { get; set; }

        public DbSet<SerialNumber> SerialNumbers { get; set; }
    }
}
