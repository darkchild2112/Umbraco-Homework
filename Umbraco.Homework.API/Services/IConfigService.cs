using System;
using Umbraco.Homework.API.Models;

namespace Umbraco.Homework.API.Services
{
    public interface IConfigService
    {
        Config GetConfig();
    }
}
