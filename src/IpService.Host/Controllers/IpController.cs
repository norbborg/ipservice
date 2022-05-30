using System.Threading.Tasks;
using Hangfire;
using IpService.Contracts.Interfaces;
using IpService.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace IpService.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class IpController : ControllerBase
{
    private readonly IIpInfoProvider _iIpInfoProvider;
    
    public IpController(IIpInfoProvider iIpInfoProvider)
    {
        _iIpInfoProvider = iIpInfoProvider;
    }

    [HttpGet("{ip}")]
    public async Task<IActionResult> Get(string ip)
    {
        var result = await _iIpInfoProvider.GetIpDetailsAsync(ip);

        return Ok(result);

    }

    [HttpPut]
    public IActionResult Update(IpDetails[] request)
    { 
        var jobId = BackgroundJob.Enqueue(() => _iIpInfoProvider.UpdateIpDetailsAsync(request));
        
        return Ok(jobId);
    }
    
    [HttpGet("job/{jobId}")]
    public IActionResult GetJob(string jobId)
    {
        var result = JobStorage.Current.GetMonitoringApi().JobDetails(jobId).History;

        return Ok(result);

    }
}