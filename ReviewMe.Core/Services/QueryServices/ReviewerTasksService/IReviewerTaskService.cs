namespace ReviewMe.Core.Services.QueryServices.ReviewerTasksService;

public interface IReviewerTaskService
{
    GetReviewerTasksResponse Get();
    GetReviewerTaskResponse Get(int assessmentId);
    void Decline(int assessmentId, DeclineRequest request);
    void Draft(int assessmentId, DraftRequest draftRequest);
    void Submit(int assessmentId, SubmitRequest submitRequest);
}