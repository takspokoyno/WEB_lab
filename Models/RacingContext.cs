using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Labka1.Models
{
    public partial class RacingContext : DbContext
    {
        public RacingContext()
        {
        }

        public RacingContext(DbContextOptions<RacingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Car> Cars { get; set; } = null!;
        public virtual DbSet<Part> Parts { get; set; } = null!;
        public virtual DbSet<Participation> Participations { get; set; } = null!;
        public virtual DbSet<Racer> Racers { get; set; } = null!;
        public virtual DbSet<Sponsor> Sponsors { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<Tournament> Tournaments { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-G8PL86U; Database=Racing; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Car");

                entity.Property(e => e.Brand).HasMaxLength(50);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.OwnerId).HasColumnName("Owner_id");

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Car_Racer");
            });

            modelBuilder.Entity<Part>(entity =>
            {
                entity.Property(e => e.CarId).HasColumnName("Car_id");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Car)
                    .WithMany(p => p.Parts)
                    .HasForeignKey(d => d.CarId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Parts_Car");
            });

            modelBuilder.Entity<Participation>(entity =>
            {
                entity.ToTable("Participation");

                entity.Property(e => e.RacerId).HasColumnName("Racer_id");

                entity.Property(e => e.TournamentId).HasColumnName("Tournament_id");

                entity.HasOne(d => d.Racer)
                    .WithMany(p => p.Participations)
                    .HasForeignKey(d => d.RacerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Participation_Racer");

                entity.HasOne(d => d.Tournament)
                    .WithMany(p => p.Participations)
                    .HasForeignKey(d => d.TournamentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Participation_Tournament");
            });

            modelBuilder.Entity<Racer>(entity =>
            {
                entity.ToTable("Racer");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("date")
                    .HasColumnName("Birth_date");

                entity.Property(e => e.Name).HasMaxLength(64);

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TeamId).HasColumnName("Team_id");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Racers)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Racer_Team");
            });

            modelBuilder.Entity<Sponsor>(entity =>
            {
                entity.ToTable("Sponsor");

                entity.Property(e => e.Name).HasMaxLength(64);

                entity.Property(e => e.TeamId).HasColumnName("Team_id");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Sponsors)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Sponsor_Team");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");

                entity.Property(e => e.Name).HasMaxLength(64);
            });

            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.ToTable("Tournament");

                entity.Property(e => e.Name).HasMaxLength(64);

                entity.Property(e => e.Reward).HasColumnType("money");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
