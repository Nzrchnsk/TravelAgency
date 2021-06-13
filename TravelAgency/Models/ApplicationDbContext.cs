using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TravelAgency.Models
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        // public DbSet<Order> Orders { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Trip> Trips { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseIdentityColumns();
            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable(name:"Trip");
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");


                entity.Property(e => e.Price);
                entity.Property(e => e.ArrivalDate);
                entity.Property(e => e.TotalTicket);
                entity.Property(e => e.DepartureDate);

                
                entity.Property(e => e.ArrivalPlaceId).HasColumnName("ArrivalPlaceId");
                entity.HasOne(e => e.ArrivalPlace)
                    .WithMany(pt => pt.ArrivalTrips)
                    .HasForeignKey(e => e.ArrivalPlaceId)
                    .OnDelete(DeleteBehavior.NoAction);
                
                entity.Property(e => e.DeparturePlaceId).HasColumnName("DeparturePlaceId");
                entity.HasOne(e => e.DeparturePlace)
                    .WithMany(pt => pt.DepartureTrips)
                    .HasForeignKey(e => e.DeparturePlaceId)
                    .OnDelete(DeleteBehavior.NoAction);
            });  
            
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable(name:"Ticket");
                entity.Property(e => e.Id)
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");


                entity.Property(e => e.Number);
                entity.Property(e => e.Status);

                
                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.HasOne(e => e.User)
                    .WithMany(pt => pt.Tickets)
                    .HasForeignKey(e => e.UserId);
                
                entity.Property(e => e.TripId).HasColumnName("TripId");
                entity.HasOne(e => e.Trip)
                    .WithMany(pt => pt.Tickets)
                    .HasForeignKey(e => e.TripId);
                
                // entity.Property(e => e.OrderId).HasColumnName("OrderId");
                // entity.HasOne(e => e.Order)
                //     .WithMany(pt => pt.Tickets)
                //     .HasForeignKey(e => e.OrderId)
                //     .OnDelete(DeleteBehavior.NoAction);
            });
        }
        

    }
}