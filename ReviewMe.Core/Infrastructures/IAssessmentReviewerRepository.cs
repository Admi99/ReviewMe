namespace ReviewMe.Core.Infrastructures;

public interface IAssessmentReviewerRepository
{
    AssessmentReviewer Get(int employeeId, int assessmentId);
    void Update(AssessmentReviewer assessmentReviewer);
}