using Microsoft.Extensions.Logging;
using ReviewMe.Core.DomainEntities.EmailTemplateModels;
using ReviewMe.Core.Services.EmailNotificationServices;

namespace ReviewMe.Core.Tests.Services.EmailNotificationServices;

public class NotifyAssessmentServiceTests
{
    [Test]
    public async Task NotifyAssessmentAfterAdd()
    {
        // Arrange

        var emailSendingService = Substitute.For<IEmailSendingService>();
        var employeeRepository = Substitute.For<IEmployeesRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();
        var configuration = Substitute.For<IConfiguration>();
        var logger = Substitute.For<ILogger<AssessmentsNotificationService>>();

        var notifyAssessmentService =
            new AssessmentsNotificationService(emailSendingService, employeeRepository, currentUserService, configuration, logger);

        configuration.GetSection("EmailDomain").Value.Returns("@cngroup.dk");

        var confSection = Substitute.For<IConfigurationSection>();
        confSection["FeedbackUrl"].Returns("https://hakkastack-test.domain.local/reviews");
        confSection["DeclineUrl"].Returns("https://hakkastack-test.domain.local/reviews?declineAssessmentId=");
        confSection["AssessmentUrl"].Returns("https://hakkastack-test.domain.local/reviews/employees");

        configuration.GetSection("HakkastackUrl").Returns(confSection);

        currentUserService.UserNameWithoutDomain.Returns("doe");

        var employeeId = 1;
        var untilDate = DateTime.Now;
        var employee = new Employee
        {
            Login = "doe",
            SurnameFirstName = "Doe John",
            Assessments = new List<Assessment>
            {
                new()
                {
                    Id = 5,
                    AssessmentState = AssessmentState.Open,
                    AssessmentDueDate = untilDate
                }
            }
        };
        employeeRepository.Get(employeeId).Returns(employee);

        var reviewers = new List<int> { 10, 11, 12 };

        employeeRepository.Get(10).Returns(new Employee
        {
            Login = "newton",
            SurnameFirstName = "Newton Issac",
        });

        employeeRepository.Get(11).Returns(new Employee
        {
            Login = "einstain",
            SurnameFirstName = "Einstain Albert",
        });

        employeeRepository.Get(12).Returns(new Employee
        {
            Login = "feynman",
            SurnameFirstName = "Richard Feynman",
        });

        var expectedResultRecipientsTo = new List<string>
        {
            "newton@cngroup.dk",
            "einstain@cngroup.dk",
            "feynman@cngroup.dk"
        };

        var expectedResultRecipientsTo2 = new List<string>
        {
            "doe@cngroup.dk",
            "doe@cngroup.dk"
        };

        var expectedTemplate1 = "/EmailTemplates/ReviewerFeedbackRequested.cshtml";

        var expectedTemplate2 = "/EmailTemplates/PerformanceReviewSessionReminder.cshtml";

        var exprectedReviewerReminderModel = new List<ReviewerFeedbackRequestedModel>
        {
           new()
           {
               ReviewerName = "Newton Issac",
               AssessedPersonName = "Doe John",
               DeclineUrl = "https://hakkastack-test.domain.local/reviews?declineAssessmentId=5",
               FeedbackUrl = "https://hakkastack-test.domain.local/reviews/5",
               AssessmentDueDate = untilDate
           } ,
           new()
           {
               ReviewerName = "Einstain Albert",
               AssessedPersonName = "Doe John",
               DeclineUrl = "https://hakkastack-test.domain.local/reviews?declineAssessmentId=5",
               FeedbackUrl = "https://hakkastack-test.domain.local/reviews/5",
               AssessmentDueDate = untilDate
           } ,
           new()
           {
               ReviewerName = "Richard Feynman",
               AssessedPersonName = "Doe John",
               DeclineUrl = "https://hakkastack-test.domain.local/reviews?declineAssessmentId=5",
               FeedbackUrl = "https://hakkastack-test.domain.local/reviews/5",
               AssessmentDueDate = untilDate
            }
        };

        var expectedReviewerReminder = exprectedReviewerReminderModel.Zip(expectedResultRecipientsTo);

        var exprectedPrSessionReminderModel = new List<PerformanceReviewSessionReminderModel>
        {
            new()
            {
                ReviewersNames = new List<string>
                {
                    "Newton Issac",
                    "Einstain Albert",
                    "Richard Feynman"
                },
                AssessmentUrl = "https://hakkastack-test.domain.local/reviews/employees/1",
                IsAdmin = true
            },
            new()
            {
                ReviewersNames = new List<string>
                {
                    "Newton Issac",
                    "Einstain Albert",
                    "Richard Feynman"
                },
                AssessmentUrl = "https://hakkastack-test.domain.local/reviews/employees/1",
                IsAdmin = true
            }
        };

        // Act

        await notifyAssessmentService.NotifyOnOpenAssessment(employeeId, reviewers);

        // Assert

        foreach (var (model, email) in expectedReviewerReminder)
        {
            await emailSendingService.Received(1).SendEmail(
                Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string> { email })),
                Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string>())),
                Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string>())),
                Arg.Is<string>(actualResult => actualResult.IsEquivalentTo(expectedTemplate1)),
                Arg.Is<string>(actualResult => actualResult.IsEquivalentTo("ReviewMe - " + model.AssessedPersonName)),
                Arg.Is<ReviewerFeedbackRequestedModel>(actualResult => actualResult.IsEquivalentTo(model)));
        }

        await emailSendingService.Received(2).SendEmail(
           Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string> { expectedResultRecipientsTo2.First() })),
           Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string>())),
           Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string>())),
           Arg.Is<string>(actualResult => actualResult.IsEquivalentTo(expectedTemplate2)),
           Arg.Is<string>(actualResult => actualResult.IsEquivalentTo("Doe John - Performance review")),
           Arg.Is<PerformanceReviewSessionReminderModel>(actualResult => actualResult.IsEquivalentTo(exprectedPrSessionReminderModel.First())));

    }

    [Test]
    public async Task NotifyAssessmentAfterUpdate()
    {
        // Arrange

        var emailSendingService = Substitute.For<IEmailSendingService>();
        var employeeRepository = Substitute.For<IEmployeesRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();
        var configuration = Substitute.For<IConfiguration>();
        var logger = Substitute.For<ILogger<AssessmentsNotificationService>>();

        var notifyAssessmentService =
            new AssessmentsNotificationService(emailSendingService, employeeRepository, currentUserService, configuration, logger);

        configuration.GetSection("EmailDomain").Value.Returns("@cngroup.dk");

        var confSection = Substitute.For<IConfigurationSection>();
        confSection["FeedbackUrl"].Returns("https://hakkastack-test.domain.local/reviews");
        confSection["DeclineUrl"].Returns("https://hakkastack-test.domain.local/reviews?declineAssessmentId=");

        configuration.GetSection("HakkastackUrl").Returns(confSection);

        currentUserService.UserNameWithoutDomain.Returns("doe");

        var employeeId = 1;
        var untilDate = DateTime.Now;
        var employee = new Employee
        {
            Login = "doe",
            SurnameFirstName = "Doe John",
            Assessments = new List<Assessment>
            {
                new()
                {
                    AssessmentState = AssessmentState.Open,
                    AssessmentDueDate = untilDate,
                    Id = 5
                }
            }
        };
        employeeRepository.Get(employeeId).Returns(employee);

        var reviewers = new List<int> { 10, 11, 12 };

        var canceledReviewers = new List<int> { 5, 6 };

        employeeRepository.Get(10).Returns(new Employee
        {
            Login = "newton",
            SurnameFirstName = "Newton Issac"
        });

        employeeRepository.Get(11).Returns(new Employee
        {
            Login = "einstain",
            SurnameFirstName = "Einstain Albert"
        });

        employeeRepository.Get(12).Returns(new Employee
        {
            Login = "feynman",
            SurnameFirstName = "Richard Feynman"
        });

        employeeRepository.Get(5).Returns(new Employee
        {
            Login = "frankl",
            SurnameFirstName = "Frankl Viktor"
        });

        employeeRepository.Get(6).Returns(new Employee
        {
            Login = "novak",
            SurnameFirstName = "Novak Jan"
        });

        var expectedResultRecipientsTo = new List<string>
        {
            "newton@cngroup.dk",
            "einstain@cngroup.dk",
            "feynman@cngroup.dk"
        };

        var expectedResultRecipientsTo2 = new List<string>
        {
            "frankl@cngroup.dk",
            "novak@cngroup.dk"
        };

        var expectedTemplate1 = "/EmailTemplates/ReviewerFeedbackRequested.cshtml";

        var expectedTemplate2 = "/EmailTemplates/ReviewerFeedbackCanceled.cshtml";

        var exprectedReviewerReminderModel = new List<ReviewerFeedbackRequestedModel>
        {
            new()
            {
                ReviewerName = "Newton Issac",
                AssessedPersonName = "Doe John",
                DeclineUrl = "https://hakkastack-test.domain.local/reviews?declineAssessmentId=5",
                FeedbackUrl = "https://hakkastack-test.domain.local/reviews/5",
                AssessmentDueDate = untilDate
            } ,
            new()
            {
                ReviewerName = "Einstain Albert",
                AssessedPersonName = "Doe John",
                DeclineUrl = "https://hakkastack-test.domain.local/reviews?declineAssessmentId=5",
                FeedbackUrl = "https://hakkastack-test.domain.local/reviews/5",
                AssessmentDueDate = untilDate
            } ,
            new()
            {
                ReviewerName = "Richard Feynman",
                AssessedPersonName = "Doe John",
                DeclineUrl = "https://hakkastack-test.domain.local/reviews?declineAssessmentId=5",
                FeedbackUrl = "https://hakkastack-test.domain.local/reviews/5",
                AssessmentDueDate = untilDate
            }
        };

        var expectedReviewerReminder = exprectedReviewerReminderModel.Zip(expectedResultRecipientsTo);

        var expectedCancelReminderModel = new List<ReviewerFeedbackCancelledModel>
        {
            new()
            {
                AssessedPersonName = "Doe John",
                ReviewerName = "Frankl Viktor"
            }
            ,
            new()
            {
                AssessedPersonName = "Doe John",
                ReviewerName = "Novak Jan"
            }
        };

        var expectedCancel = expectedCancelReminderModel.Zip(expectedResultRecipientsTo2);

        // Act

        await notifyAssessmentService.NotifyOnUpdateAssessment(employeeId, reviewers, canceledReviewers);

        // Assert

        foreach (var (model, email) in expectedReviewerReminder)
        {
            await emailSendingService.Received(1).SendEmail(
                Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string> { email })),
                Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string>())),
                Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string>())),
                Arg.Is<string>(actualResult => actualResult.IsEquivalentTo(expectedTemplate1)),
                Arg.Is<string>(actualResult => actualResult.IsEquivalentTo("ReviewMe - " + model.AssessedPersonName)),
                Arg.Is<ReviewerFeedbackRequestedModel>(actualResult => actualResult.IsEquivalentTo(model)));
        }


        foreach (var (model, email) in expectedCancel)
        {
            await emailSendingService.Received(1).SendEmail(
                Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string> { email })),
                Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string>())),
                Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string>())),
                Arg.Is<string>(actualResult => actualResult.IsEquivalentTo(expectedTemplate2)),
                Arg.Is<string>(actualResult => actualResult.IsEquivalentTo("ReviewMe - Doe John")),
                Arg.Is<ReviewerFeedbackCancelledModel>(actualResult => actualResult.IsEquivalentTo(model)));
        }
    }

    [Test]
    public async Task NotifyAsessmentAfterDelete()
    {
        // Arrange

        var emailSendingService = Substitute.For<IEmailSendingService>();
        var employeeRepository = Substitute.For<IEmployeesRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();
        var configuration = Substitute.For<IConfiguration>();
        var logger = Substitute.For<ILogger<AssessmentsNotificationService>>();

        var notifyAssessmentService =
            new AssessmentsNotificationService(emailSendingService, employeeRepository, currentUserService, configuration, logger);

        configuration.GetSection("EmailDomain").Value.Returns("@cngroup.dk");

        var employeeId = 1;

        var untilDate = DateTime.Now;
        var employee = new Employee
        {
            Login = "doe",
            SurnameFirstName = "Doe John",
            Assessments = new List<Assessment>
            {
                new()
                {
                    AssessmentState = AssessmentState.Open,
                    AssessmentDueDate = untilDate
                }
            }
        };
        employeeRepository.Get(employeeId).Returns(employee);

        var canceledReviewers = new List<int> { 5, 6 };

        employeeRepository.Get(5).Returns(new Employee
        {
            Login = "frankl",
            SurnameFirstName = "Frankl Viktor"
        });

        employeeRepository.Get(6).Returns(new Employee
        {
            Login = "novak",
            SurnameFirstName = "Novak Jan"
        });

        var expectedResultRecipientsTo2 = new List<string>
        {
            "frankl@cngroup.dk",
            "novak@cngroup.dk"
        };

        var expectedTemplate2 = "/EmailTemplates/ReviewerFeedbackCanceled.cshtml";

        var exprectedPrSessionReminderModel = new List<ReviewerFeedbackCancelledModel>
        {
            new()
            {
                AssessedPersonName = "Doe John",
                ReviewerName = "Frankl Viktor"
            },
            new()
            {
                AssessedPersonName = "Doe John",
                ReviewerName = "Novak Jan"
            }
        };

        var exprectedPrSessionReminder = exprectedPrSessionReminderModel.Zip(expectedResultRecipientsTo2);

        // Act

        await notifyAssessmentService.NotifyOnDeleteAssessment(employeeId, canceledReviewers);

        // Assert

        foreach (var (model, email) in exprectedPrSessionReminder)
        {
            await emailSendingService.Received(1).SendEmail(
                Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string> { email })),
                Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string>())),
                Arg.Is<IEnumerable<string>>(actualResult => actualResult.IsEquivalentTo(new List<string>())),
                Arg.Is<string>(actualResult => actualResult.IsEquivalentTo(expectedTemplate2)),
                Arg.Is<string>(actualResult => actualResult.IsEquivalentTo("ReviewMe - Doe John")),
                Arg.Is<ReviewerFeedbackCancelledModel>(actualResult => actualResult.IsEquivalentTo(model)));
        }


    }
}