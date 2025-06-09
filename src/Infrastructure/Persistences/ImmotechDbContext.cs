using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistences
{
    public class ImmotechDbContext(DbContextOptions options) : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<Property> Properties { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<ProfessionalUser> ProfessionalUsers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ModerationLog> ModerationLogs { get; set; }
        public DbSet<SearchCriteria> SearchCriterias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Agency Configuration
            modelBuilder.Entity<Agency>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.ContactEmail).IsRequired().HasMaxLength(100);
                
                // Relationships
                entity.HasMany(e => e.Properties)
                    .WithOne(e => e.Agency)
                    .HasForeignKey(e => e.AgencyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.ProfessionalUsers)
                    .WithOne(e => e.Agency)
                    .HasForeignKey(e => e.AgencyId);
            });

            // Property Configuration
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Price).HasPrecision(10, 2);

                // Relationships
                entity.OwnsOne(e => e.Address);
                entity.OwnsMany(e => e.Photos);

                // Indexes
                entity.HasIndex(e => e.AgencyId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.Price);
            });


            // ProfessionalUser Configuration
            modelBuilder.Entity<ProfessionalUser>(entity =>
            {
                entity.HasOne(e => e.Agency)
                    .WithMany(e => e.ProfessionalUsers)
                    .HasForeignKey(e => e.AgencyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Address Configuration
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.Street).IsRequired();
                entity.Property(e => e.City).IsRequired();
                entity.Property(e => e.State).IsRequired();
                entity.Property(e => e.ZipCode).IsRequired();
            });


            // Notification Configuration
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.IsRead).HasDefaultValue(false);
                entity.HasIndex(e => e.IsRead);
            });

            // ModerationLog Configuration
            modelBuilder.Entity<ModerationLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Action).IsRequired();

                entity.HasOne(e => e.Moderator)
                    .WithMany()
                    .HasForeignKey(e => e.ModeratorId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.PropertyId);
                entity.HasIndex(e => e.ModeratorId);
            });

            // SearchCriteria Configuration
            modelBuilder.Entity<SearchCriteria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Keywords).HasMaxLength(200);
                entity.Property(e => e.Location);
                entity.Property(e => e.MinPrice).HasPrecision(10, 2);
                entity.Property(e => e.MaxPrice).HasPrecision(10, 2);
            });
        }
    }
}
