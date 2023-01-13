using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace openglclevel_server_data.DataAccess;

public partial class OpenglclevelContext : DbContext
{
    public OpenglclevelContext()
    {
    }

    public OpenglclevelContext(DbContextOptions<OpenglclevelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MealEvent> MealEvents { get; set; }

    public virtual DbSet<MealEventItem> MealEventItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MealEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MealEvent");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.MealAtDay).IsRequired();
            entity.Property(e => e.MealDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.MealEvents)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MealEvent__UserI__29572725");
        });

        modelBuilder.Entity<MealEventItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MealEventItem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).IsRequired();

            entity.HasOne(d => d.MealEvent).WithMany(p => p.MealEventItems)
                .HasForeignKey(d => d.MealEventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MealEvent__MealE__2A4B4B5E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.HashedPassword).IsRequired();
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Salt).IsRequired();
            entity.Property(e => e.UserName).IsRequired();
            entity.Property(e => e.UserNumber).ValueGeneratedOnAdd();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
