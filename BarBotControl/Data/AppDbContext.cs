using Microsoft.EntityFrameworkCore;
using BarBotControl.Data.Models;

namespace BarBotControl.Data;

public class AppDbContext : DbContext
{
    public DbSet<Module> Modules { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Sequence> Sequences { get; set; }
    public DbSet<SequenceItem> SequenceItem { get; set; }
    public DbSet<ErrorType> ErrorTypes { get; set; }
    public DbSet<SudoUser> SudoUsers { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SudoUser>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        modelBuilder.Entity<Option>()
            .HasIndex(nameof(Option.OptionValue), nameof(Option.ModuleId))
            .IsUnique();

        modelBuilder.Entity<Module>()
            .HasMany(e => e.Options)
            .WithOne(e => e.Module)
            .HasForeignKey(e => e.ModuleId)
            .IsRequired();

        modelBuilder.Entity<Module>()
            .HasMany(e => e.ErrorTypes)
            .WithOne(e => e.Module)
            .HasForeignKey(e => e.ModuleId)
            .IsRequired();

        modelBuilder.Entity<Sequence>()
            .HasMany(e => e.SequenceItems)
            .WithOne(e => e.Sequence)
            .HasForeignKey(e => e.SequenceId)
            .IsRequired();

        modelBuilder.Entity<Sequence>()
            .HasMany(e => e.ErrorTypes)
            .WithOne(e => e.Sequence)
            .HasForeignKey(e => e.SequenceId);

        modelBuilder.Entity<SequenceItem>()
            .HasOne(e => e.Module)
            .WithMany(e => e.SequenceItems)
            .HasForeignKey(e => e.ModuleId)
            .IsRequired();

        modelBuilder.Entity<SequenceItem>()
            .HasOne(e => e.Option)
            .WithMany(e => e.SequenceItems)
            .HasForeignKey(e => e.OptionId)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}
