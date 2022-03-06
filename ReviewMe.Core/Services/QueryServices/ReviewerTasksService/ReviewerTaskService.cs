using ReviewMe.Core.Exceptions;

namespace ReviewMe.Core.Services.QueryServices.ReviewerTasksService;

public class ReviewerTaskService : IReviewerTaskService
{
    private readonly IEmployeesRepository _employeesRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAssessmentReviewerRepository _assessmentReviewerRepository;

    public ReviewerTaskService(IEmployeesRepository employeesRepository, ICurrentUserService currentUserService,
        IAssessmentReviewerRepository assessmentReviewerRepository)
    {
        _employeesRepository = employeesRepository;
        _currentUserService = currentUserService;
        _assessmentReviewerRepository = assessmentReviewerRepository;
    }

    public GetReviewerTasksResponse Get()
        => new()
        {
            ReviewerTasks = GetAssessmentReviewersForCurrentUser()
                .Where(assessmentReviewer => assessmentReviewer.Assessment!.AssessmentState == AssessmentState.Open)
                .Select(assessmentReviewer => new ReviewerTask
                {
                    AssessmentId = assessmentReviewer.AssessmentId,
                    SurnameFirstName = assessmentReviewer.Assessment!.Employee!.SurnameFirstName,
                    Department = assessmentReviewer.Assessment.Employee.Department,
                    Position = assessmentReviewer.Assessment.Employee.Position,
                    Location = assessmentReviewer.Assessment.Employee.Location,
                    ImageSrc = Utilities.GetProfilePhoto(assessmentReviewer.Assessment.Employee.Login),
                    AssessmentDueDate = assessmentReviewer.Assessment.AssessmentDueDate,
                    ReviewerTaskState = assessmentReviewer.AssessmentReviewerState,
                    AreasForImprovements = assessmentReviewer.AreasForImprovements
                })
                .ToList()
        };

    public GetReviewerTaskResponse Get(int assessmentId)
    {
        var assessmentReviewer = GetAssessmentReviewersForCurrentUser()
            .FirstOrDefault(assessmentReviewer => assessmentReviewer.AssessmentId == assessmentId);

        if (assessmentReviewer == null)
            throw new ErrorTypeException(ErrorType.ResourceNotFound, $"Assessment with id '{assessmentId}' not found!");

        var assessment = assessmentReviewer.Assessment;

        return new GetReviewerTaskResponse
        {
            ReviewerTask = new ReviewerTask
            {
                AssessmentId = assessmentId,
                SurnameFirstName = assessment!.Employee!.SurnameFirstName,
                Department = assessment.Employee.Department,
                Position = assessment.Employee.Position,
                Location = assessment.Employee.Location,
                ImageSrc = Utilities.GetProfilePhoto(assessment.Employee.Login),
                AssessmentDueDate = assessment.AssessmentDueDate,
                ReviewerTaskState = assessmentReviewer.AssessmentReviewerState,
                Feedback = assessmentReviewer.Feedback,
                TeamLeaderName = _employeesRepository.GetByLogin(assessment.Employee.TeamLeaderLogin)?.SurnameFirstName ?? string.Empty,
                AreasForImprovements = assessmentReviewer.AreasForImprovements   
            }
        };
    }

    public void Decline(int assessmentId, DeclineRequest request)
    {
        var assessmentReviewer = _assessmentReviewerRepository.Get(GetCurrentEmployeeId(), assessmentId);

        CheckAssessmentReviewer(assessmentReviewer, assessmentId);
        if (assessmentReviewer.AssessmentReviewerState != AssessmentReviewerState.Created &&
            assessmentReviewer.AssessmentReviewerState != AssessmentReviewerState.Drafted)
            throw new ErrorTypeException(ErrorType.GeneralRequestValidation,
                $"Assessment with id '{assessmentId}' cannot be declined, because it is in state {assessmentReviewer.AssessmentReviewerState}!");

        assessmentReviewer.AssessmentReviewerState = AssessmentReviewerState.Declined;
        assessmentReviewer.AreasForImprovements = string.Empty;
        assessmentReviewer.Feedback = request.Reason;

        _assessmentReviewerRepository.Update(assessmentReviewer);
    }

    public void Draft(int assessmentId, DraftRequest draftRequest)
    {
        var assessmentReviewer = _assessmentReviewerRepository.Get(GetCurrentEmployeeId(), assessmentId);

        CheckAssessmentReviewer(assessmentReviewer, assessmentId);
        CheckAssessmentReviewerState(assessmentReviewer);

        assessmentReviewer.AssessmentReviewerState = AssessmentReviewerState.Drafted;
        assessmentReviewer.Feedback = draftRequest.Feedback;
        assessmentReviewer.AreasForImprovements = draftRequest.AreasForImprovements;

        _assessmentReviewerRepository.Update(assessmentReviewer);
    }
    public void Submit(int assessmentId, SubmitRequest submitRequest)
    {
        var assessmentReviewer = _assessmentReviewerRepository.Get(GetCurrentEmployeeId(), assessmentId);

        CheckAssessmentReviewer(assessmentReviewer, assessmentId);
        CheckAssessmentReviewerState(assessmentReviewer);

        assessmentReviewer.AssessmentReviewerState = AssessmentReviewerState.Reviewed;
        assessmentReviewer.Feedback = submitRequest.Feedback;
        assessmentReviewer.AreasForImprovements = submitRequest.AreasForImprovements;

        _assessmentReviewerRepository.Update(assessmentReviewer);
    }

    private static void CheckAssessmentReviewer(AssessmentReviewer assessmentReviewer, int assessmentId)
    {
        if (assessmentReviewer == null)
            throw new ErrorTypeException(ErrorType.ResourceNotFound, $"Assessment with id '{assessmentId}' not found!");

        if (assessmentReviewer.Assessment!.AssessmentState != AssessmentState.Open)
            throw new ErrorTypeException(ErrorType.GeneralRequestValidation, $"Assessment with id '{assessmentId}' is not open!");
    }

    private static void CheckAssessmentReviewerState(AssessmentReviewer assessmentReviewer)
    {
        if (assessmentReviewer.AssessmentReviewerState != AssessmentReviewerState.Created &&
            (assessmentReviewer.AssessmentReviewerState == AssessmentReviewerState.Declined ||
             assessmentReviewer.AssessmentReviewerState == AssessmentReviewerState.Reviewed))
            throw new ErrorTypeException(ErrorType.GeneralRequestValidation,
                $"Assessment with id '{assessmentReviewer.Assessment!.Id}' cannot be saved, because it is in state {assessmentReviewer.AssessmentReviewerState}!");
    }

    private IEnumerable<AssessmentReviewer> GetAssessmentReviewersForCurrentUser()
        => _employeesRepository
            .GetWithAllDetails(GetCurrentEmployeeId())
            .AssessmentReviewers;

    private int GetCurrentEmployeeId()
    {
        var employee = _employeesRepository.GetByLogin(_currentUserService.UserNameWithoutDomain);

        if (employee == null)
            throw new ErrorTypeException(ErrorType.GenericServerError,
                $"Cannot find employee record for current user '{_currentUserService.UserNameWithoutDomain}'");

        return employee.Id;
    }
}