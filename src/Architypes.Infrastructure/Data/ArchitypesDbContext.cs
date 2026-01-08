using Architypes.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Architypes.Infrastructure.Data;

public class ArchitypesDbContext : DbContext
{
    public ArchitypesDbContext(DbContextOptions<ArchitypesDbContext> options) : base(options)
    {
    }

    public DbSet<Archetype> Archetypes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Assessment> Assessments { get; set; }
    public DbSet<AssessmentAnswer> AssessmentAnswers { get; set; }
    public DbSet<AssessmentResult> AssessmentResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Archetype configuration
        modelBuilder.Entity<Archetype>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.MaleName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FemaleName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CoreDrive).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Strengths).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Shadow).IsRequired().HasMaxLength(500);
            entity.Property(e => e.InBusiness).IsRequired().HasMaxLength(500);
            entity.Property(e => e.FreeTeaser).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.DetailedCharacteristics).IsRequired();
            entity.Property(e => e.Blindspots).IsRequired();
            entity.Property(e => e.InteractionPatterns).IsRequired();
        });

        // Question configuration
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Text).IsRequired().HasMaxLength(500);
            entity.HasOne(e => e.Archetype)
                .WithMany(a => a.Questions)
                .HasForeignKey(e => e.ArchetypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Assessment configuration
        modelBuilder.Entity<Assessment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SessionId).IsRequired();
            entity.Property(e => e.Gender).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.PaymentReference).HasMaxLength(100);
            entity.HasIndex(e => e.SessionId);
        });

        // AssessmentAnswer configuration
        modelBuilder.Entity<AssessmentAnswer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Rating).IsRequired();
            entity.HasOne(e => e.Assessment)
                .WithMany(a => a.Answers)
                .HasForeignKey(e => e.AssessmentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Question)
                .WithMany()
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // AssessmentResult configuration
        modelBuilder.Entity<AssessmentResult>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TopArchetypeScores).IsRequired();
            entity.HasOne(e => e.Assessment)
                .WithOne(a => a.Result)
                .HasForeignKey<AssessmentResult>(e => e.AssessmentId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.PrimaryArchetype)
                .WithMany()
                .HasForeignKey(e => e.PrimaryArchetypeId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.SecondaryArchetype)
                .WithMany()
                .HasForeignKey(e => e.SecondaryArchetypeId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.ShadowArchetype)
                .WithMany()
                .HasForeignKey(e => e.ShadowArchetypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
