using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace Devon4Net.Application.WebAPI.Configuration.Common
{
    public class CosmosConfigurationParams
    {
        public string Endpoint { get; set; }
        public string Key { get; set; }
        public string DatabaseName { get; set; }
    }
}