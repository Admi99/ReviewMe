namespace ReviewMe.Core.Infrastructures;

public interface IAssessmentsNotificationService
{
    Task NotifyOnOpenAssessment(int employeeId, IReadOnlyCollection<int> reviewers);
    Task NotifyOnUpdateAssessment(int employeeId, IReadOnlyCollection<int> reviewers, IReadOnlyCollection<int> canceledReviewers);
    Task NotifyOnDeleteAssessment(int employeeId, IReadOnlyCollection<int> canceledReviewers);

}