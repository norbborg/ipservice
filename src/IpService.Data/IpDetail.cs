using System;
using System.Collections.Generic;

namespace IpService.Data
{
    public partial class IpDetail
    {
        public string Ip { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Continent { get; set; } = null!;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
