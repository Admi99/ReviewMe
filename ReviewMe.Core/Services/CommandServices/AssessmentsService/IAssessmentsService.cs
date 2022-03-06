namespace ReviewMe.Core.Services.CommandServices.AssessmentsService;

public interface IAssessmentsService
{
    void OpenAssessment(int employeeId, OpenAssessmentRequest request);
    void UpdateAssessment(int employeeId, UpdateAssessmentRequest request);
    void CloseAssessment(int employeeId);
    void DeleteAssessment(int employeeId);
    void SaveAdditionalFeedback(int employeeId, SaveAdditionalFeedbackRequest request);
}