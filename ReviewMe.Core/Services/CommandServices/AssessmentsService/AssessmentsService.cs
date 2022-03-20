using ReviewMe.Core.Exceptions;

namespace ReviewMe.Core.Services.CommandServices.AssessmentsService;

public class AssessmentsService : IAssessmentsService
{
    private readonly IAssessmentsRepository _assessmentsRepository;
    private readonly IEmployeesRepository _employeesRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAssessmentsNotificationService _assessmentsNotificationService;

    public AssessmentsService(IAssessmentsRepository assessmentsRepository, IEmployeesRepository employeesRepository,
        ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider, IAssessmentsNotificationService assessmentsNotificationService)
    {
        _assessmentsRepository = assessmentsRepository;
        _employeesRepository = employeesRepository;
        _currentUserService = currentUserService;
        _dateTimeProvider = dateTimeProvider;
        _assessmentsNotificationService = assessmentsNotificationService;
    }

    public void OpenAssessment(int employeeId, OpenAssessmentRequest request)
    {
        var now = _dateTimeProvider.Now();
        var assessment = new Assessment
        {
            AssessmentState = AssessmentState.Open,
            AssessmentDueDate = request.AssessmentDueDate,
            PerformanceReviewDate = request.PerformanceReviewDate,
            EmployeeId = employeeId,
            CreatedByEmployeeId = CurrentEmployeeId,
            CreatedAt = now,
            AssessmentReviewers = request.Reviewers
                .Distinct()
                .Select(reviewer => new AssessmentReviewer
                {
                    EmployeeId = reviewer
                })
                .ToList()
        };

        if (request.AssessmentDueDate <= now)
            throw new ErrorTypeException(ErrorType.GeneralRequestValidation,
                $"Assessment due date cannot be in past ({assessment.AssessmentDueDate})!");

        if (request.PerformanceReviewDate <= assessment.AssessmentDueDate)
            throw new ErrorTypeException(ErrorType.GeneralRequestValidation,
                $"Performance review date cannot be before Assessment due date  (Assessment Due date: {assessment.AssessmentDueDate}, Performance review date: {assessment.PerformanceReviewDate})!");

        if (_assessmentsRepository.Get(employeeId, AssessmentState.Open) is not null)
            throw new ErrorTypeException(ErrorType.GeneralRequestValidation,
                $"Employee '{employeeId}' already has an open assessment!");

        _assessmentsRepository.Add(assessment);

        _assessmentsNotificationService.NotifyOnOpenAssessment(
            employeeId,
            assessment.AssessmentReviewers.Select(assessmentReviewer => assessmentReviewer.EmployeeId).ToList());

    }

    public void UpdateAssessment(int employeeId, UpdateAssessmentRequest request)
    {
        var now = DateTimeOffset.Now;
        var assessment = _assessmentsRepository.GetWithReviewers(employeeId, AssessmentState.Open);

        if (assessment is null)
            throw new ErrorTypeException(ErrorType.ResourceNotFound,
                $"No open assessment for employee '{employeeId}' found!");

        if (request.AssessmentDueDate <= now)
            throw new ErrorTypeException(ErrorType.GeneralRequestValidation,
                $"Assessment due date cannot be in past ({assessment.AssessmentDueDate})!");

        if (request.PerformanceReviewDate <= assessment.AssessmentDueDate)
            throw new ErrorTypeException(ErrorType.GeneralRequestValidation,
                $"Performance review date cannot be before Assessment due date  (Assessment Due date: {assessment.AssessmentDueDate}, Performance review date: {assessment.PerformanceReviewDate})!");

        var employeeIds = assessment.AssessmentReviewers.Select(reviewer => reviewer.EmployeeId).ToList();
        var newReviewers = request.Reviewers.Except(employeeIds).ToList();
        var canceledReviewer = employeeIds.Except(request.Reviewers).ToList();

        assessment.AssessmentDueDate = request.AssessmentDueDate;
        assessment.PerformanceReviewDate = request.PerformanceReviewDate;
        assessment.AssessmentReviewers = request.Reviewers
            .Distinct()
            .Select(reviewer =>
            {
                var existingReviewer = assessment.AssessmentReviewers.FirstOrDefault(x => x.EmployeeId == reviewer);
                if (existingReviewer != null)
                {
                    return existingReviewer;
                }

                return new AssessmentReviewer
                {
                    EmployeeId = reviewer
                };
            })
            .ToList();


        _assessmentsRepository.Update(assessment);

        _assessmentsNotificationService.NotifyOnUpdateAssessment(employeeId, newReviewers, canceledReviewer);
    }

    public void CloseAssessment(int employeeId)
    {
        var assessment = _assessmentsRepository.GetWithReviewers(employeeId, AssessmentState.Open);

        if (assessment is null)
            throw new ErrorTypeException(ErrorType.ResourceNotFound,
                $"No open assessment for employee '{employeeId}' found!");

        assessment.AssessmentState = AssessmentState.Closed;

        _assessmentsRepository.Update(assessment);
    }

    public void DeleteAssessment(int employeeId)
    {
        var assessment = _assessmentsRepository.GetWithReviewers(employeeId, AssessmentState.Open);

        if (assessment is null)
            throw new ErrorTypeException(ErrorType.ResourceNotFound,
                $"No open assessment for employee '{employeeId}' found!");

        assessment.AssessmentState = AssessmentState.Deleted;

        _assessmentsRepository.Update(assessment);

        _assessmentsNotificationService.NotifyOnDeleteAssessment(employeeId, assessment.AssessmentReviewers.Select(assessmentReviewer => assessmentReviewer.EmployeeId).ToList());

    }

    private int CurrentEmployeeId
        => _employeesRepository.GetByLogin(_currentUserService.UserNameWithoutDomain)!.Id;
}