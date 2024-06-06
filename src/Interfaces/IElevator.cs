using SimulationElevator.Enums;
using SimulationElevator.Models;

namespace SimulationElevator.Interfaces
{
    // Interface defining the operations and properties of an elevator
    public interface IElevator
    {
        // Closes the elevator doors
        bool CloseDoors();

        // Gets the lowest floor that the elevator services
        int GetBottomFloor();

        // Gets the current floor that the elevator is on
        int GetCurrentFloor();

        // Gets the unique identifier of the elevator
        int GetId();

        // Gets the current operational state of the elevator
        ElevatorOperationStatus GetOperationalState();

        // Gets the highest floor that the elevator services
        int GetTopFloor();

        // Moves the elevator between floors asynchronously
        Task MoveBetweenFloors();

        // Opens the elevator doors
        bool OpenDoors();

        // Prepares the elevator for a journey to fulfill a request
        void ReadyElevatorForJourney(Request request);

        // Prepares the elevator for new passengers after completing a journey
        void ReadyElevatorForNewPassengers();

        // Executes the routine to pick up a passenger en route to a destination
        void RunEnroutePickupRoutine(Request request);

        // Sets the elevator as available for new requests
        void SetElevatorAsAvailable();

        // Puts the elevator into maintenance mode
        void SetToMaintenanceMode();

        // Performs safety checks before the elevator moves
        bool RunSafetyChecks();

        // Weighs the elevator to ensure it is not overloaded
        bool WeighElevator();
    }
}
