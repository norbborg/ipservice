using IpService.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IpService.Data;

public class IpServiceRepository : IIpServiceRepository
{
    private readonly IpServiceContext _ipServiceContext;

    public IpServiceRepository(IpServiceContext ipServiceContext)
    {
        _ipServiceContext = ipServiceContext;
    }

    public async Task<IpDetail?> GetIpDetails(string ip)
    {
        var ipDetail = await _ipServiceContext.IpDetails.SingleOrDefaultAsync(detail => detail.Ip.Equals(ip));

        return ipDetail;
    }

    public async Task<IpDetail> AddIpDetails(IpDetail ipDetail)
    {
        var result = await _ipServiceContext.IpDetails.AddAsync(ipDetail);
        await _ipServiceContext.SaveChangesAsync();
        
        return result.Entity;
    }

    public async Task UpdateIpDetails(IList<IpDetail> ipDetails)
    {
        _ipServiceContext.IpDetails.UpdateRange(ipDetails);

        await _ipServiceContext.SaveChangesAsync();
    }
}

public interface IIpServiceRepository
{
    Task<IpDetail?> GetIpDetails(string ip);

    Task<IpDetail> AddIpDetails(IpDetail ipDetails);

    Task UpdateIpDetails(IList<IpDetail> ipDetails);
}