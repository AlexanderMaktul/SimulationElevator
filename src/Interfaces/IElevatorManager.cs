using SimulationElevator.Models;

namespace SimulationElevator.Interfaces
{
    // Interface defining the operations of an elevator manager
    public interface IElevatorManager
    {
        // Processes a request to be handled by the elevator manager
        void MakeElevatorManagerRequest(Request request);

        // Runs the elevator manager to handle elevator operations asynchronously
        Task<int> RunElevatorManager();
    }
}
