namespace ReviewMe.Infrastructure.DbStorage;

public class ReviewMeDbContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;

    public ReviewMeDbContext(DbContextOptions<ReviewMeDbContext> options, ILoggerFactory loggerFactory) : base(options)
    {
        _loggerFactory = loggerFactory;
    }

    public DbSet<EmployeeDo> Employees => Set<EmployeeDo>();
    public DbSet<AssessmentDo> Assessments => Set<AssessmentDo>();
    public DbSet<AssessmentReviewerDo> AssessmentReviewers => Set<AssessmentReviewerDo>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseLoggerFactory(_loggerFactory);

#if DEBUG
        //TODO: create config switch for sensitive logging
        optionsBuilder.EnableSensitiveDataLogging();
#endif
    }


    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        base.OnModelCreating(modelbuilder);

        modelbuilder.Entity<AssessmentReviewerDo>()
            .HasKey(p => new { p.EmployeeDoId, p.AssessmentDoId });

        modelbuilder.Entity<AssessmentReviewerDo>()
            .HasOne(ar => ar.EmployeeDo)
            .WithMany(e => e.AssessmentReviewers)
            .OnDelete(DeleteBehavior.Restrict);

        modelbuilder.Entity<AssessmentReviewerDo>()
            .HasOne(ar => ar.AssessmentDo)
            .WithMany(ar => ar.AssessmentReviewers)
            .OnDelete(DeleteBehavior.Cascade);

        modelbuilder.Entity<EmployeeDo>()
            .HasMany(employee => employee.CreatedAssessments)
            .WithOne(assessment => assessment.CreatedByEmployeeDo)
            .HasForeignKey(assessment => assessment.CreatedByEmployeeDoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}