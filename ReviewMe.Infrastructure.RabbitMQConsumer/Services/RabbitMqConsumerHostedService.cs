using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using System.Net.Sockets;

namespace ReviewMe.Infrastructure.RabbitMQConsumer.Services;

internal class RabbitMqConsumerHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    private IQueueService? _queueService;

    private bool IsConsuming { get; set; }

    public RabbitMqConsumerHostedService(IServiceProvider serviceProvider, ILogger<RabbitMqConsumerHostedService> logger, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var rabbitMqSettingsSection = _configuration.GetSection("RabbitMq");
        var hostName = rabbitMqSettingsSection["HostName"];
        int.TryParse(rabbitMqSettingsSection["Port"], out var port);
        var delay = _configuration.GetValue<int>("TryToConsumeDelay");

        TryStartConsuming(hostName, port, delay);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (IsConsuming)
            _queueService?.StopConsuming();

        return Task.CompletedTask;
    }

    // We need to check first, if rabbitMq server is accessible,
    // otherwise app can fail because of rabbitMq dependency injection library (which doesn't has any good way to do it)
    private void TryStartConsuming(string hostName, int port, int delay)
    {
        Task.Run(() =>
        {
            while (IsConsuming == false)
            {
                if (PingHost(hostName, port))
                {
                    try
                    {
                        _queueService = GetIQueueService();
                        _queueService?.StartConsuming();
                        _logger.LogInformation("RabbitMq started consuming massages !");
                        IsConsuming = true;
                    }
                    catch (Exception e)
                    {
                        _logger.LogCritical($"Error: RabbitMq cannot connect to {port} port with exception: {e}");
                        IsConsuming = false;
                    }
                }
                Task.Delay(delay);
            }
        });
    }

    // We are using tcp client because standard Ping only tells you, if computer is accessible
    // Tcp client connect to specific port to a given IP Address
    // if connection is established, port is opened otherwise exception will be raised
    public bool PingHost(string hostUri, int portNumber)
    {
        try
        {
            using var client = new TcpClient(hostUri, portNumber);
            _logger.LogInformation($"Connection to {hostUri} on port {portNumber} successful");
            return true;
        }
        catch (SocketException socketException)
        {
            _logger.LogError($"Error: Server on Address: {hostUri} on Port: {portNumber} is not reachable with error: {socketException}");
            return false;
        }
    }

    private IQueueService GetIQueueService()
        => _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IQueueService>();
}