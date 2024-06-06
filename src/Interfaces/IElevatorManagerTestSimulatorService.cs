namespace SimulationElevator.Interfaces
{
    // Interface defining the operations for the elevator manager test simulator service
    public interface IElevatorManagerTestSimulatorService
    {
        // Runs an automated simulation for testing the elevator manager
        void automatedSimulation();

        // Runs a manual simulation for testing the elevator manager
        void manualSimulation();
    }
}
