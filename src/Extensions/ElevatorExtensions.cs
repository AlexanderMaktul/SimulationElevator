using SimulationElevator.Enums;
using SimulationElevator.Interfaces;

namespace SimulationElevator.Extensions
{
    public static class ElevatorExtensions
    {
        // Extension method to check if any elevator operates on the requested floor
        public static bool ElevatorsOperateOnThisFloor(this List<IElevator> elevators, int floorRequest)
        {
            // Returns true if any elevator's floor range includes the requested floor
            return elevators.Any(x => floorRequest >= x.GetBottomFloor() && floorRequest <= x.GetTopFloor());
        }

        // Extension method to check if an elevator is at its floor range limit (top or bottom floor)
        public static bool IsAtFloorRangeLimit(this IElevator elevator)
        {
            // Returns true if the elevator is at its top or bottom floor
            return elevator.GetCurrentFloor() == elevator.GetTopFloor() || elevator.GetCurrentFloor() == elevator.GetBottomFloor();
        }

        // Extension method to choose the most appropriate available elevator for a requested floor
        public static IElevator ChooseAppropriateAvailableElevator(this List<IElevator> elevators, int requestedFloor)
        {
            // Find the first elevator that is online (available)
            IElevator availableElevator = elevators.FirstOrDefault(x => x.GetOperationalState() == ElevatorOperationStatus.ONLINE);

            // If no elevator is available, return null
            if (availableElevator == null)
            {
                return null; // All elevators are busy or unavailable (maintenance, etc.)
            }

            // Calculate the initial smallest difference in floors
            int currentSmallestDifference = GetDifferenceBetweenFloors(availableElevator.GetCurrentFloor(), requestedFloor);

            // Iterate through the rest of the elevators (skipping the first available one)
            foreach (IElevator elevator in elevators.Skip(1))
            {
                // Check if the request is coming from below the current elevator's floor
                if (elevator.GetCurrentFloor() > requestedFloor)
                {
                    // If another elevator is currently traveling down, it will pick up the request
                    if (ElevatorIsOnACloserFloor(requestedFloor, elevator.GetCurrentFloor(), currentSmallestDifference)
                        && !elevators.Any(x => x.GetOperationalState() == ElevatorOperationStatus.UP))
                    {
                        availableElevator = elevator; // Update the best selection of elevator
                        currentSmallestDifference = elevator.GetCurrentFloor() - requestedFloor;
                    }
                }
                else
                {
                    // Check if the elevator is closer to the requested floor and update accordingly
                    if (ElevatorIsOnACloserFloor(elevator.GetCurrentFloor(), requestedFloor, currentSmallestDifference)
                        && !elevators.Any(x => x.GetOperationalState() == ElevatorOperationStatus.UP))
                    {
                        availableElevator = elevator;
                        currentSmallestDifference = requestedFloor - elevator.GetCurrentFloor();
                    }
                }
            }

            return availableElevator; // Return the best available elevator
        }

        // Helper method to determine if an elevator is on a closer floor
        private static bool ElevatorIsOnACloserFloor(int lowerFloor, int higherFloor, int currentSmallestDifference)
        {
            return (higherFloor - lowerFloor) < currentSmallestDifference; // Returns true if the elevator is closer
        }

        // Helper method to calculate the difference between two floors
        private static int GetDifferenceBetweenFloors(int currentElevatorFloor, int requestedFloor)
        {
            return currentElevatorFloor > requestedFloor
                ? currentElevatorFloor - requestedFloor
                : requestedFloor - currentElevatorFloor; // Returns the absolute difference
        }
    }
}
