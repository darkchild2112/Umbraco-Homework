using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Umbraco.Homework.API.Test
{
    public abstract class BaseTests
    {
        protected IConfiguration GetConfiguration(IDictionary<string, string> configValues = null)
            => new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

        protected T GetControllerResult<T>(Func<IActionResult> controllerAction)
        {
            IActionResult test = controllerAction();

            OkObjectResult okResult = test as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);

            return (T)okResult.Value;
        }

        protected async void GetControllerResultAsync(Func<Task<IActionResult>> controllerAction)
        {
            IActionResult test = await controllerAction();

            OkResult okResult = test as OkResult;

            Assert.Equal(200, okResult.StatusCode);
        }

        protected async void GetControllerResultAsync<T>(Func<T, Task<IActionResult>> controllerAction, T actionParam)
        {
            IActionResult test = await controllerAction(actionParam);

            OkResult okResult = test as OkResult;

            Assert.Equal(200, okResult.StatusCode);
        }

        protected async Task<T> GetControllerResultAsync<T>(Func<Task<IActionResult>> controllerAction)
        {
            IActionResult test = await controllerAction();

            OkObjectResult okResult = test as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);

            return (T)okResult.Value;
        }

        protected async Task<T> GetControllerResultAsync<T, T2>(Func<T2, Task<IActionResult>> controllerAction, T2 actionParam)
        {
            IActionResult test = await controllerAction(actionParam);

            OkObjectResult okResult = test as OkObjectResult;

            Assert.Equal(200, okResult.StatusCode);

            return (T)okResult.Value;
        }
    }
}
