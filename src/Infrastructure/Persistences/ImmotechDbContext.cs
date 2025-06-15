using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistences
{
    public class ImmotechDbContext(DbContextOptions options, Application.Common.ICurrentUser currentUser) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options), Application.Common.IImmotechDbContext
    {
        public DbSet<Property> Properties { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<ProfessionalUser> ProfessionalUsers { get; set; }
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

                entity.OwnsOne(e => e.Address, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.Property(a => a.Street).IsRequired();
                    ownedNavigationBuilder.Property(a => a.City).IsRequired();
                    ownedNavigationBuilder.Property(a => a.State).IsRequired();
                    ownedNavigationBuilder.Property(a => a.ZipCode).IsRequired();
                });
                
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
                entity.HasQueryFilter(p => p.UserId == currentUser.UserId || p.AgencyId == currentUser.AgencyId);
                // Relationships with User and Agency.
                entity.OwnsOne(e => e.Address, ownedNavigationBuilder =>
                {
                    ownedNavigationBuilder.Property(a => a.Street).IsRequired();
                    ownedNavigationBuilder.Property(a => a.City).IsRequired();
                    ownedNavigationBuilder.Property(a => a.State).IsRequired();
                    ownedNavigationBuilder.Property(a => a.ZipCode).IsRequired();
                });
                entity.OwnsMany(e => e.Photos);

                // Indexes
                entity.HasIndex(e => e.AgencyId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.Price);

                entity.Property(p => p.Type)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.BidType)
                      .HasConversion<string>()
                      .HasMaxLength(10)
                      .HasDefaultValue(PropertyBidType.Sale);

                entity.Property(e => e.Status)
                      .HasConversion<string>()
                      .HasMaxLength(20);
            });


            // ProfessionalUser Configuration
            modelBuilder.Entity<ProfessionalUser>(entity =>
            {
                entity.HasOne(e => e.Agency)
                    .WithMany(e => e.ProfessionalUsers)
                    .HasForeignKey(e => e.AgencyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Notification Configuration
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.IsRead).HasDefaultValue(false);
                entity.HasIndex(e => e.IsRead);
                entity.HasQueryFilter(n => n.RecipientId == currentUser.UserId || n.AgencyId == currentUser.AgencyId); // TODO: remove this query filter
                entity.Property(e => e.SenderEmail).IsRequired();
                entity.Property(e => e.RecipientEmail).IsRequired();
                entity.HasIndex(e => e.SentAt);
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
                entity.HasQueryFilter(sc => sc.UserId == currentUser.UserId);
            });
        }
    }
}
