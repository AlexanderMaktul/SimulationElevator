using SimulationElevator.Interfaces;
using SimulationElevator.Enums;

namespace SimulationElevator.Models
{
    // Class representing an elevator
    public class Elevator : IElevator
    {
        #region Variables and Constructor

        private readonly int bottomFloor;
        private readonly int id;
        private readonly double maxWeight;
        private readonly int topFloor;

        private int currentFloor { get; set; }
        private DoorStatus doorStatus { get; set; }
        private ElevatorOperationStatus operationStatus { get; set; }

        // Constructor to initialize elevator properties
        public Elevator(int _id, int _bottomFloor, int _topFloor, int _startFloor, double _maxWeight)
        {
            id = _id;
            bottomFloor = _bottomFloor;
            topFloor = _topFloor;
            maxWeight = _maxWeight;

            currentFloor = _startFloor;
            operationStatus = ElevatorOperationStatus.ONLINE; // Set the initial operational status
        }
        #endregion

        // Method to close the elevator doors
        public bool CloseDoors()
        {
            doorStatus = DoorStatus.CLOSED;

            Console.WriteLine($"Elevator {id} doors closed");

            return false; // Placeholder return value
        }

        // Method to get the bottom floor serviced by the elevator
        public int GetBottomFloor()
        {
            return bottomFloor;
        }

        // Method to get the current floor of the elevator
        public int GetCurrentFloor()
        {
            return currentFloor;
        }

        // Method to get the unique identifier of the elevator
        public int GetId()
        {
            return id;
        }

        // Method to get the operational status of the elevator
        public ElevatorOperationStatus GetOperationalState()
        {
            return operationStatus;
        }

        // Method to get the top floor serviced by the elevator
        public int GetTopFloor()
        {
            return topFloor;
        }

        // Method to move the elevator between floors asynchronously
        public async Task MoveBetweenFloors()
        {
            await Task.Delay(1000); // Simulate time taken to move between floors

            // Update current floor based on direction of travel
            currentFloor = operationStatus == ElevatorOperationStatus.UP
                ? currentFloor += 1
                : currentFloor -= 1;

            Console.WriteLine($"Elevator {id} status: {operationStatus} to Floor: {currentFloor}");
        }

        // Method to open the elevator doors
        public bool OpenDoors()
        {
            doorStatus = DoorStatus.OPEN;

            Console.WriteLine($"Elevator {id} doors open.");

            return true; // Placeholder return value
        }

        // Method to prepare the elevator for a journey to fulfill a request
        public void ReadyElevatorForJourney(Request request)
        {
            // If the doors are open, close them after all safety checks complete
            if (doorStatus == DoorStatus.OPEN)
            {
                while (!RunSafetyChecks())
                {
                    // Wait 1 second before retrying safety checks
                    Thread.Sleep(1000);
                }

                CloseDoors();
            }

            // Determine the direction of travel and update operation status accordingly
            operationStatus = request.Floor > currentFloor
            ? ElevatorOperationStatus.UP
            : ElevatorOperationStatus.DOWN;

            Console.WriteLine($"Elevator {id}from Floor: {currentFloor}");
        }

        // Method to prepare the elevator for new passengers after completing a journey
        public void ReadyElevatorForNewPassengers()
        {
            OpenDoors();

            SetElevatorAsAvailable();
        }

        // Method to execute the routine to pick up a passenger en route to a destination
        public void RunEnroutePickupRoutine(Request request)
        {
            OpenDoors();

            // Allow people time to board before weighing and closing the doors
            Task.Delay(1000);

            ReadyElevatorForJourney(request);
        }

        // Method to set the elevator as available for new requests
        public void SetElevatorAsAvailable()
        {
            operationStatus = ElevatorOperationStatus.ONLINE;

            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Elevator {id} status: {operationStatus}");
        }

        // Method to put the elevator into maintenance mode
        public void SetToMaintenanceMode()
        {
            operationStatus = ElevatorOperationStatus.MAINTENANCE;
        }

        // Method to perform safety checks before the elevator moves
        public bool RunSafetyChecks()
        {
            if (!WeighElevator())
            {
                operationStatus = ElevatorOperationStatus.OVERLOADED;

                Console.WriteLine($"Elevator {id} status: {operationStatus}.  weight limit: {maxWeight}");

                return false;
            }

            Console.WriteLine($"Elevator {id} successful");

            return true;
        }

        // Method to weigh the elevator to ensure it is not overloaded
        public bool WeighElevator()
        {
            // Simulate a 10% chance of being overloaded (for demonstration purposes)
            return new Random().Next(0, 10) != 1;
        }
    }
}
