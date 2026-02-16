using Application.Repositories;

namespace Infrastructure.Services;

public class WalletNumberGenerator : IWalletNumberGenerator
{
    private const string prefix = "67";
    
    public string Generate()
    {
        var randomPart = Random.Shared.Next(100000000, 999999999).ToString();
        var timestampPart = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString().Substring(6);
        
        return $"{prefix}{timestampPart}{randomPart}";
    }
}