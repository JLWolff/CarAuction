using CarAuctionApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionApi.Infrastructure.Data
{
    public class CarAuctionDbContext : DbContext
    {
        public CarAuctionDbContext(DbContextOptions<CarAuctionDbContext> options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Id).IsRequired();
                entity.Property(v => v.Manufacturer).IsRequired().HasMaxLength(100);
                entity.Property(v => v.Model).IsRequired().HasMaxLength(100);
                entity.Property(v => v.Year).IsRequired();
                entity.Property(v => v.StartingBid).IsRequired();
                entity.Property(v => v.Type).IsRequired();

                entity.HasDiscriminator<VehicleType>(v => v.Type)
                    .HasValue<Sedan>(VehicleType.Sedan)
                    .HasValue<Hatchback>(VehicleType.Hatchback)
                    .HasValue<SUV>(VehicleType.SUV)
                    .HasValue<Truck>(VehicleType.Truck);
            });

            modelBuilder.Entity<Sedan>(entity =>
            {
                entity.Property(s => s.NumberOfDoors).IsRequired();
            });

            modelBuilder.Entity<Hatchback>(entity =>
            {
                entity.Property(h => h.NumberOfDoors).IsRequired();
            });

            modelBuilder.Entity<SUV>(entity =>
            {
                entity.Property(s => s.NumberOfSeats).IsRequired();
            });

            modelBuilder.Entity<Truck>(entity =>
            {
                entity.Property(t => t.LoadCapacity).IsRequired();
            });

            modelBuilder.Entity<Auction>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Id).IsRequired();
                entity.Property(a => a.VehicleId).IsRequired();
                entity.Property(a => a.Status).IsRequired();
                entity.Property(a => a.CurrentHighestBid).IsRequired();
                entity.Property(a => a.StartTime).IsRequired();

                entity.HasOne(a => a.Vehicle)
                    .WithMany()
                    .HasForeignKey(a => a.VehicleId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(a => a.Bids)
                    .WithOne()
                    .HasForeignKey(b => b.AuctionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Bid>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Id).IsRequired();
                entity.Property(b => b.AuctionId).IsRequired();
                entity.Property(b => b.BidderId).IsRequired().HasMaxLength(100);
                entity.Property(b => b.Amount).IsRequired();
                entity.Property(b => b.Timestamp).IsRequired();
            });
        }
    }
}
