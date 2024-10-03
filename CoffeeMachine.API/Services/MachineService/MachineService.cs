using CoffeeMachine.API.Models.Request;
using CoffeeMachine.API.Repositorys.MachineRepository;
using System.Net.Http;
using System.Text.Json;

namespace CoffeeMachine.API.Services.MachineService;

public class MachineService(IMachineRepository machineRepository, IHttpClientFactory httpClient) : IMachineService
{
    private readonly IMachineRepository _machineRepository = machineRepository;
    private readonly IHttpClientFactory _httpClient = httpClient;
   
    public async Task<(string message, bool isOutOfCoffee)> BrewCoffeeAsync(DateTime today)
    {
        if (today.Month == 4 && today.Day == 1)
        {
            throw new HttpRequestException("I'm a teapot", null, System.Net.HttpStatusCode.ServiceUnavailable);
        }

        var coffeeRequest = await _machineRepository.GetCoffeeRequestAsync();
        if (coffeeRequest == null)
        {
            coffeeRequest = new CoffeeRequest { RequestCount = 0, LastRequestDate = today };
        }

        coffeeRequest.RequestCount++;

        if (coffeeRequest.RequestCount % 5 == 0)
        {
            await _machineRepository.UpdateCoffeeRequestAsync(coffeeRequest);
            return (null, true)!;
        }

        var message = "Your piping hot coffee is ready";

        coffeeRequest.LastRequestDate = today;
        await _machineRepository.UpdateCoffeeRequestAsync(coffeeRequest);

        return (message, false);
    }
}
