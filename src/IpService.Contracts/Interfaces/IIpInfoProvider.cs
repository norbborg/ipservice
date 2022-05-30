using IpService.Contracts.Models;

namespace IpService.Contracts.Interfaces;

public interface IIpInfoProvider
{
    public Task<IpDetails> GetIpDetailsAsync(string ip);

    public Task UpdateIpDetailsAsync(IpDetails[] ipDetailsArray);
}