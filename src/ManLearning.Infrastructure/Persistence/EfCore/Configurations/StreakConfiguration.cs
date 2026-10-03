using ManLearning.Domain.Learners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManLearning.Infrastructure.Persistence.EfCore.Configurations;

/// <summary>
/// Maps <see cref="Streak"/>. There is at most one streak row per learner, so the learner id is
/// the primary key (see docs/decisions/0003-initial-streak-calendar-policy.md).
/// </summary>
internal sealed class StreakConfiguration : IEntityTypeConfiguration<Streak>
{
    public void Configure(EntityTypeBuilder<Streak> streak)
    {
        streak.ToTable("Streaks");
        streak.HasKey(s => s.LearnerId);
        streak.Property(s => s.LearnerId).HasConversion(IdValueConverters.LearnerId);
        streak.Property(s => s.CurrentLength).IsRequired();
        streak.Property(s => s.LongestLength).IsRequired();
        streak.Property(s => s.LastActiveDate);
    }
}
