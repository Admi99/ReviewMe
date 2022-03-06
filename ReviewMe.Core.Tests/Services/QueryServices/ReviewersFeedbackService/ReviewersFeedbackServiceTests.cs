using ReviewMe.Core.Services.QueryServices.ReviewersFeedbackService;

namespace ReviewMe.Core.Tests.Services.QueryServices.ReviewersFeedbackService;

public class ReviewersFeedbackServiceTests
{
    [Test]
    public void GetByEmployeeId()
    {
        // Arrange
        var assessedEmployeeId = 5;
        var assessedemployeeSurnameFirstName = "Brown John";

        var employeeId = 5;
        var employeeSurnameFirstName = "Doe John";

        var employeeReviewerId1 = 6;
        var employeeReviewerSurnameFeedBack = "I Dont know this person";

        var assessmentId = 2001;

        var employeeAssessed = new Employee
        {
            Id = assessedEmployeeId,
            SurnameFirstName = assessedemployeeSurnameFirstName
        };

        var employeeReviewer = new Employee
        {
            Id = employeeId,
            SurnameFirstName = employeeSurnameFirstName,
            Login = "Doe"
        };

        var assessmentReviewers = new List<AssessmentReviewer>
        {
            new()
            {
                Id = employeeReviewerId1,
                EmployeeId = employeeId,
                Employee = employeeReviewer,
                Feedback = employeeReviewerSurnameFeedBack,
                AssessmentId = assessmentId,
                AssessmentReviewerState = AssessmentReviewerState.Declined
            }
        };
        var assessment = new Assessment
        {
            Id = assessmentId,
            AssessmentState = AssessmentState.Open,
            Employee = employeeAssessed,
            EmployeeId = employeeAssessed.Id,
            AssessmentReviewers = assessmentReviewers
        };

        var expectedFeedback = new List<ReviewerFeedback>
        {
            new()
            {
                Name = employeeSurnameFirstName,
                Feedback = employeeReviewerSurnameFeedBack,
                ImageSrc = Utilities.GetProfilePhoto(employeeReviewer.Login),
                AssessmentReviewerState = AssessmentReviewerState.Declined
            }
        };

        var expectedResponse = new GetReviewerFeedbackResponse
        {
            Feedbacks = expectedFeedback
        };

        var assessmentRepository = Substitute.For<IAssessmentsRepository>();
        assessmentRepository.GetWithReviewersWithEmployees(employeeId, assessment.AssessmentState).Returns(assessment);

        var testee = new ReviewerFeedbackService(assessmentRepository);

        // Act
        var actualResult = testee.GetByEmployeeId(employeeAssessed.Id);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResponse);
    }
}

