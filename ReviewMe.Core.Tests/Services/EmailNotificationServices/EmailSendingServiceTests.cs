using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReviewMe.Core.DomainEntities.EmailTemplateModels;
using ReviewMe.Core.Services.EmailNotificationServices;
using ReviewMe.Core.Settings;

namespace ReviewMe.Core.Tests.Services.EmailNotificationServices;

public class EmailSendingServiceTests
{
    [Test]
    public async Task SendEmail()
    {
        //Arrange

        var emailSender = Substitute.For<IEmailSender>();
        var razorTemplateParsingService = Substitute.For<IRazorTemplateParsingService>();

        var logger = Substitute.For<ILogger<EmailSendingService>>();

        var recipientsTo = new List<string>
        {
            "johndoe@cngroup.dk"
        };

        var applicationSetting = new ApplicationSettings
        {
            UseTestEmailAddresses = false,
            TestEmailAddresses = recipientsTo
        };

        var someOptions = Options.Create(applicationSetting);

        var emailSendingService =
            new EmailSendingService(emailSender, logger, someOptions, razorTemplateParsingService);

        var reviewerReminderModel = new ReviewerFeedbackRequestedModel
        {
            AssessedPersonName = "John Doe",
            DeclineUrl = "https://www.dummy.com",
            FeedbackUrl = "https://www.dummy.com",
            AssessmentDueDate = DateTime.Now
        };

        var templateName = "Layout.cshtml";
        var folderName = "EmailTemplates";
        var embededStaticResource =
            "<!DOCTYPE html> <html> <head> <title>Body Tag</title> </head> <body> <h2>Example of body tag</h2> <p>{{tableBody}}</p> </body> </html>";
        razorTemplateParsingService.GetEmbeddedStaticTemplate(templateName, folderName).Returns(
            embededStaticResource);

        var finishedTamplate =
             "<b>Hi, this person will have PR soon, here is a list of a reviewers requested for that person, Thanks</b>";

        var subject = " @reviewersName - Performance review ";
        var templatePath = "folder1/folder2/temp.cshtml";
        razorTemplateParsingService.RenderAsync(templatePath, reviewerReminderModel).Returns(finishedTamplate);

        var expectedResult = new EmailMessage
        {
            To = recipientsTo,
            Bcc = new List<string>(),
            Cc = new List<string>(),
            Subject = subject,
            Content = "<!DOCTYPE html> <html> <head> <title>Body Tag</title> </head> <body> <h2>Example of body tag</h2> <p><b>Hi, this person will have PR soon, here is a list of a reviewers requested for that person, Thanks</b></p> </body> </html>",
        };

        //Act

        await emailSendingService.SendEmail<object>(recipientsTo, new List<string>(), new List<string>(), templatePath, subject, reviewerReminderModel);

        //Assert

        await emailSender.Received(1).SendAsync(Arg.Is<EmailMessage>(actualResult => actualResult.IsEquivalentTo(expectedResult)));

    }
}