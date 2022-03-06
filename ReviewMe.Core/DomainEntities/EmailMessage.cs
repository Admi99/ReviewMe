namespace ReviewMe.Core.DomainEntities;

public class EmailMessage
{
    public string Subject { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public IEnumerable<string> To { get; set; } = new List<string>();

    public IEnumerable<string> Cc { get; set; } = new List<string>();

    public IEnumerable<string> Bcc { get; set; } = new List<string>();

}