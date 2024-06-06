namespace SimulationElevator.Interfaces
{
    using System.Collections.Generic;

    // Interface defining the operations for the elevator service
    public interface IElevatorService
    {
        // Retrieves the list of elevators managed by the service
        List<IElevator> GetElevators();

        // Raises an alert for an issue with a specified elevator
        void raiseElevatorAlert(int elevatorId);
    }
}
