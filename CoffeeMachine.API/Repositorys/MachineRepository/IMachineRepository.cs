using CoffeeMachine.API.Models.Request;

namespace CoffeeMachine.API.Repositorys.MachineRepository;

public interface IMachineRepository
{
    Task<CoffeeRequest> GetCoffeeRequestAsync();
    Task UpdateCoffeeRequestAsync(CoffeeRequest request);
}
