namespace ReviewMe.Infrastructure.DbStorage.Repository;

public class AssessmentsRepository : IAssessmentsRepository
{
    private readonly ReviewMeDbContext _reviewMeDbContext;
    private readonly IMapper _mapper;

    public AssessmentsRepository(ReviewMeDbContext reviewMeDbContext, IMapper mapper)
    {
        _reviewMeDbContext = reviewMeDbContext;
        _mapper = mapper;
    }

    public void Add(Assessment assessment)
    {
        var assessmentDo = _mapper.Map<AssessmentDo>(assessment);
        _reviewMeDbContext.Add(assessmentDo);
        _reviewMeDbContext.SaveChanges();
    }

    public void Update(Assessment assessment)
    {
        var foundAssessment = _reviewMeDbContext.Assessments
            .Include(a => a.AssessmentReviewers)
            .FirstOrDefault(a => a.Id == assessment.Id);

        if (foundAssessment == null) return;

        foundAssessment.AssessmentState = assessment.AssessmentState;
        foundAssessment.AssessmentDueDate = assessment.AssessmentDueDate;
        foundAssessment.PerformanceReviewDate = assessment.PerformanceReviewDate;
        foundAssessment.AdditionalFeedback = assessment.AdditionalFeedback;
        foundAssessment.AssessmentReviewers = assessment.AssessmentReviewers
            .Select(a =>
            new AssessmentReviewerDo
            {
                EmployeeDoId = a.EmployeeId,
                AssessmentDoId = a.AssessmentId,
                Feedback = a.Feedback,
                AssessmentReviewerState = a.AssessmentReviewerState

            }).ToList();

        _reviewMeDbContext.Entry(foundAssessment).State = EntityState.Modified;

        _reviewMeDbContext.Assessments.Update(foundAssessment);
        _reviewMeDbContext.SaveChanges();
    }

    public Assessment? Get(int employeeId, AssessmentState state)
        => _reviewMeDbContext.Assessments
            .AsNoTracking()
            .FirstOrDefault(assessment =>
                assessment.EmployeeDoId == employeeId && assessment.AssessmentState == state)
            .Map<Assessment?>(_mapper);

    public Assessment? GetWithReviewers(int employeeId, AssessmentState state)
        => _reviewMeDbContext.Assessments
            .AsNoTracking()
            .Include(assessment => assessment.AssessmentReviewers)
            .FirstOrDefault(assessment =>
                assessment.EmployeeDoId == employeeId && assessment.AssessmentState == state)
            .Map<Assessment?>(_mapper);

    public Assessment? GetWithReviewersWithEmployees(int employeeId, AssessmentState state)
        => _reviewMeDbContext.Assessments
            .AsNoTracking()
            .Include(assessment => assessment.AssessmentReviewers)
            .ThenInclude(assessmentReviewer => assessmentReviewer.EmployeeDo)
            .FirstOrDefault(assessment => assessment.EmployeeDoId == employeeId && assessment.AssessmentState == state)
            .Map<Assessment?>(_mapper);
}