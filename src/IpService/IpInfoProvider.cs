using IpService.Contracts.Interfaces;
using IpService.Contracts.Models;
using IpService.Data;
using IPStackIntegration;
using Microsoft.Extensions.Options;

namespace IpService;

public class IpInfoProvider : IIpInfoProvider
{
    private readonly IIpStackClient _client;
    private readonly ICacheStore _cacheStore;
    private readonly IIpServiceRepository _ipServiceRepository;
    private readonly IpStackOptions _ipStackOptions;

    public IpInfoProvider(IIpStackClient client, ICacheStore cacheStore, IIpServiceRepository ipServiceRepository, 
        IOptions<IpStackOptions> ipStackOptions)
    {
        _client = client;
        _cacheStore = cacheStore;
        _ipServiceRepository = ipServiceRepository;
        _ipStackOptions = ipStackOptions.Value;
    }

    public async Task<IpDetails> GetIpDetailsAsync(string ip)
    {
        var ipDetails = _cacheStore.Get<IpDetails>(CacheKey(ip));

        if (ipDetails is null)
        {
            var database = await _ipServiceRepository.GetIpDetails(ip);

            if (database is not null)
            {
                ipDetails = Map(database);
            }
            else
            {
                var result = await _client.GetIpDetailsAsync(ip, _ipStackOptions.ApiKey);
                ipDetails = Map(ip, result);

                await _ipServiceRepository.AddIpDetails(Map(ipDetails));
            }

            _cacheStore.Add(CacheKey(ip), ipDetails, TimeSpan.FromMinutes(1));
        }

        return ipDetails;
    }

    public async Task UpdateIpDetailsAsync(IpDetails[] ipDetailsArray)
    {
        var ipDetails = ipDetailsArray.Select(ipDetail => Map(ipDetail)).ToList();
        
        await _ipServiceRepository.UpdateIpDetails(ipDetails);
    }


    private static string CacheKey(string ip)
    {
        return $"IP_{ip}";
    }

    private static IpDetails Map(string ip, IpStackResponse response)
    {
        return new IpDetails
        {
            Ip = ip,
            City = response.City,
            Continent = response.Continent_name,
            Country = response.Country_name,
            Latitude = response.Latitude,
            Longitude = response.Longitude
        };
    }

    private static IpDetail Map(IpDetails ipDetails)
    {
        return new IpDetail
        {
            City = ipDetails.City,
            Continent = ipDetails.Continent,
            Country = ipDetails.Country,
            Ip = ipDetails.Ip,
            Latitude = ipDetails.Latitude,
            Longitude = ipDetails.Longitude
        };
    }

    private static IpDetails Map(IpDetail entity)
    {
        return new IpDetails
        {
            City = entity.City,
            Continent = entity.Continent,
            Country = entity.Country,
            Latitude = entity.Latitude,
            Longitude = entity.Longitude
        };
    }
}