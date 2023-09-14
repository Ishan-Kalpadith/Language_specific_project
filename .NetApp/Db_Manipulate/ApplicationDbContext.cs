using Db_Manipulate.Models;
using Microsoft.EntityFrameworkCore;

namespace Db_Manipulate
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserData> UserDatas { get; set; }
        public DbSet<AddressData> UserAddresses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=(localdb)\\MSSQLLocalDB; Initial Catalog = master1; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate = False; Application Intent = ReadWrite; Multi Subnet Failover = False"
                );
            }
        }
        //manage relatioships between address table and user table
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UserData>()
                .HasOne(u => u.address)
                .WithMany()
                .HasForeignKey(u => u.AddressId);
            modelBuilder.Entity<AddressData>().HasKey(a => a.AddressId);

            modelBuilder
                .Entity<UserData>()
                .Property(u => u.tags)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                );
        }
    }
}
