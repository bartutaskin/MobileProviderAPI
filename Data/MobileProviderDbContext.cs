using Microsoft.EntityFrameworkCore;
using MobileProvider.Models.Entities;

namespace MobileProvider.Data
{
    public class MobileProviderDbContext : DbContext
    {
        public MobileProviderDbContext(DbContextOptions<MobileProviderDbContext> options) : base(options) { }

        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Usage> Usages { get; set; }
        public DbSet<Bill> Bills { get; set; }

    }
}
