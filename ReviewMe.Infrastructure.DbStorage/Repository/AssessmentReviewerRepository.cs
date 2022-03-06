namespace ReviewMe.Infrastructure.DbStorage.Repository;

public class AssessmentReviewerRepository : IAssessmentReviewerRepository
{
    private readonly ReviewMeDbContext _dbContext;
    private readonly IMapper _mapper;

    public AssessmentReviewerRepository(ReviewMeDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public AssessmentReviewer Get(int employeeId, int assessmentId)
        => _dbContext.AssessmentReviewers
            .AsNoTracking()
            .Include(assessmentReviewer => assessmentReviewer.AssessmentDo)
            .FirstOrDefault(assessmentReviewerDo =>
                assessmentReviewerDo.EmployeeDoId == employeeId && assessmentReviewerDo.AssessmentDoId == assessmentId)
            .Map<AssessmentReviewer>(_mapper);

    public void Update(AssessmentReviewer assessmentReviewer)
    {
        var assessmentReviewerDo = _mapper.Map<AssessmentReviewerDo>(assessmentReviewer);
        _dbContext.Update(assessmentReviewerDo);
        _dbContext.SaveChanges();
    }
}