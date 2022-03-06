namespace ReviewMe.Infrastructure.RabbitMQConsumer.Objects;

public class EmployeeRo : IEntityRo
{
    public int ContractId { get; set; }

    public int PpId { get; set; }

    public int? TimurId { get; set; }

    public int IsActive { get; set; }

    public string? SurnameFirstName { get; set; } = string.Empty;

    public string Branch { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public string? Login { get; set; } = string.Empty;

    public string? TeamLeaderLogin { get; set; } = string.Empty;

}