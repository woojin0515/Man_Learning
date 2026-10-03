using ManLearning.Domain.Progress;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManLearning.Infrastructure.Persistence.EfCore.Configurations;

/// <summary>
/// Maps <see cref="LessonProgress"/>. The natural key is the (learner, lesson) pair — a learner
/// has at most one progress row per lesson.
/// </summary>
internal sealed class LessonProgressConfiguration : IEntityTypeConfiguration<LessonProgress>
{
    public void Configure(EntityTypeBuilder<LessonProgress> progress)
    {
        progress.ToTable("LessonProgress");
        progress.HasKey(p => new { p.LearnerId, p.LessonId });
        progress.Property(p => p.LearnerId).HasConversion(IdValueConverters.LearnerId);
        progress.Property(p => p.LessonId).HasConversion(IdValueConverters.LessonId);
        progress.Property(p => p.State).IsRequired().HasConversion<string>().HasMaxLength(20);
    }
}
