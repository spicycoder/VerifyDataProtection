using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace VerifyDataProtection;

public class KeyInspector : IHostedService
{
    private readonly IKeyManager _keyManager;
    private readonly ILogger<KeyInspector> _logger;

    public KeyInspector(
        IServiceProvider services,
        ILogger<KeyInspector> logger)
    {
        _keyManager = services.GetRequiredService<IKeyManager>();
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var keys = _keyManager.GetAllKeys();
        _logger.LogInformation("Current Keys:");
        foreach (var key in keys)
        {
            _logger.LogInformation("Key ID: {KeyId}", key.KeyId);
            _logger.LogInformation("Activation Date: {ActivationDate}", key.ActivationDate);
            _logger.LogInformation("Expiration Date: {ExpirationDate}", key.ExpirationDate);
            _logger.LogInformation("Is Revoked: {IsRevoked}", key.IsRevoked);
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}