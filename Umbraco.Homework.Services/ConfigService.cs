using System;
using Umbraco.Homework.Model;

namespace Umbraco.Homework.Services
{
    public class ConfigService : IConfigService
    {
        // Temp hard coded for now
        public Config GetConfig() => new Config { MaxSubmissions = 2 };
    }
}
