using SimulationElevator.Interfaces;
using SimulationElevator.Models;

namespace SimulationElevator.Services
{
    // Service class for managing elevators
    public class ElevatorService : IElevatorService
    {
        // Retrieves the list of elevators from a persistent data store
        public List<IElevator> GetElevators()
        {
            // Simulated method to retrieve elevators from a data store
            // In reality, this method would fetch data from a database or another source
            return new List<IElevator>
            {
                new Elevator(1, -3, 20, 13, 75), // Create elevator 1 with specified parameters
                new Elevator(2, -3, 20, 6, 75)   // Create elevator 2 with specified parameters
            };
        }

        // Raises an alert for an issue with the specified elevator
        public void raiseElevatorAlert(int elevatorId)
        {
            // Placeholder method to raise an alert for an elevator issue
            // In a real application, this method would notify maintenance personnel or trigger an alerting system
            return;
        }
    }
}
