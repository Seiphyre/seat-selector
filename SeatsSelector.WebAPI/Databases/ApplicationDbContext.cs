using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeatsSelector.WebAPI.Models;

namespace SeatsSelector.WebAPI.Database
{
    public class ApplicationDbContext : IdentityDbContext<UserEntity, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<RoomEntity> Rooms { get; set; }
        public DbSet<SeatEntity> Seats { get; set; }



        // -----------------------------------------------------------------------------

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoomEntity>().HasKey(e => e.Id);

            modelBuilder.Entity<SeatEntity>().HasKey(e => e.Id);
            modelBuilder.Entity<SeatEntity>().HasOne(e => e.Room).WithMany(e => e.Seats).HasForeignKey(e => e.RoomId);
            modelBuilder.Entity<SeatEntity>().HasOne(e => e.User).WithOne(e => e.Seat).HasForeignKey<SeatEntity>(e => e.UserId).IsRequired(false);

            modelBuilder.Entity<UserEntity>().HasKey(e => e.Id);
        }
    }
}
