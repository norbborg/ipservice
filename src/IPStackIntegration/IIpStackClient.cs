using Refit;

namespace IPStackIntegration;

public interface IIpStackClient
{
    [Get("/{ip}")]
    Task<IpStackResponse> GetIpDetailsAsync(string ip, [Query]string access_key);
}