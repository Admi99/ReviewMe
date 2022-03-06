namespace ReviewMe.Core.Infrastructures;

public interface IAssessmentsRepository
{
    void Add(Assessment assessment);
    void Update(Assessment assessment);
    Assessment? Get(int employeeId, AssessmentState state);
    Assessment? GetWithReviewers(int employeeId, AssessmentState state);
    Assessment? GetWithReviewersWithEmployees(int employeeId, AssessmentState state);
}