using SimulationElevator.Enums;
using SimulationElevator.Extensions;
using SimulationElevator.Interfaces;
using SimulationElevator.Models;

namespace SimulationElevator
{
    // The ElevatorManager class manages elevators and handles elevator requests.
    public class ElevatorManager : IElevatorManager
    {
        #region Variables and Constructor

        private readonly IElevatorService _elevatorService;


   
        // List of elevators controlled by the elevator manager.
        private List<IElevator> controlledElevators { get; set; }

        // List of pending elevator requests.
        private List<Request> requestList { get; set; }

        // Constructor to inject the elevator service dependency.
        public ElevatorManager(IElevatorService elevatorService)
        {
            _elevatorService = elevatorService;
        }
        #endregion

        // Makes a request to the elevator manager.
        public void MakeElevatorManagerRequest(Request request)
        {
            // Check if the request is valid and add it to the request list.
            if (!requestList.Any(x => x.Floor == request.Floor) && controlledElevators.ElevatorsOperateOnThisFloor(request.Floor))
            {
                requestList.Add(request);

                // Print a confirmation message for the added request.
                Console.ForegroundColor = ConsoleColor.Green;
                string panelRequestString = request.ElevatorId != 0 ? $". Elevator Id: {request.ElevatorId}" : null;
                Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {request.RequestType} Request added for Floor {request.Floor}{panelRequestString}\n");
                Console.ForegroundColor = ConsoleColor.White;

                return;
            }

            // Print a message for an invalid request.
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Invalid request: {request.RequestType} Request for Floor {request.Floor}. Ignored\n");
        }

        // Runs the elevator manager.
        public async Task<int> RunElevatorManager()
        {
            // Initialize elevator manager and check for any errors.
            if (!ElevatorManagerInitialisation())
            {
                return -1;
            }

            // Print the state of controlled elevators.
            PrintControlledElevatorState();

            // Print the remaining requests in the request list.
            PrintRemainingRequests(requestList);

            

            // The elevator manager continuously checks for new requests to pick up at set intervals.
            while (true)
            {
                try
                {
                    // Check if there are any pending requests.
                    if (requestList.Count == 0)
                    {
                        // Wait before checking the request list again.
                        Thread.Sleep(1000);
                        continue;
                    }

                    // Retrieve the first request in the list.
                    Request floorRequest = requestList[0];

                    // Choose an elevator based on the request type.
                    IElevator elevator = ChooseElevatorDependingOnRequestType(floorRequest);
                    if (elevator == null)
                    {
                        // Wait 1 second before checking for an available elevator again.
                        Thread.Sleep(1000);
                        continue;
                    }

                    // An elevator was found to service the request, remove the request from the list.
                    requestList.RemoveAt(0);

                    // Perform the elevator journey for the request.
                    await MakeElevatorJourney(floorRequest, elevator);

                    // Print the updated state of controlled elevators.
                    PrintControlledElevatorState();

                    // Print the remaining requests in the request list.
                    PrintRemainingRequests(requestList);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        // Choose an elevator depending on the type of request.
        private IElevator ChooseElevatorDependingOnRequestType(Request floorRequest)
        {
            // If a panel request comes in from the elevator panel, the corresponding elevator must go on the journey.
            // If it's a wall request, the best available elevator can pick it up.
            return floorRequest.RequestType == RequestType.PANEL
                ? controlledElevators.FirstOrDefault(x => x.GetId() == floorRequest.ElevatorId)
                : controlledElevators.ChooseAppropriateAvailableElevator(floorRequest.Floor);
        }

        // Run a journey for an elevator.
        private async Task RunJourney(Request request, IElevator elevator)
        {
            // Move the elevator in the correct direction until it reaches the request floor.
            while (elevator.GetCurrentFloor() != request.Floor)
            {
                await elevator.MoveBetweenFloors();

                // If there are any floors with requests in the elevator's direction, stop to pick them up and remove the request.
                if (ElevatorShouldMakeEnroutePickup(elevator))
                {
                    Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Elevator stopped to make {requestList.FirstOrDefault(x => x.Floor == elevator.GetCurrentFloor()).RequestType} request pickup " +
                        $"on Floor: {elevator.GetCurrentFloor()} en-route to Floor: {request.Floor}");

                    elevator.RunEnroutePickupRoutine(request);

                    requestList.RemoveAll(x => x.Floor == elevator.GetCurrentFloor());
                }

                // Elevator can't travel further in this direction.
                if (elevator.IsAtFloorRangeLimit())
                {
                    break;
                }
            }

            // Print a message indicating the completion of the request.
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Elevator {elevator.GetId()} completed {request.RequestType} request for Floor: {elevator.GetCurrentFloor()}");
        }

        // Initialize the elevator manager.
        private bool ElevatorManagerInitialisation()
        {
            try
            {
                // Retrieve the list of controlled elevators from the elevator service.
                controlledElevators = _elevatorService.GetElevators();

                // Initialize the request list.
                requestList = new List<Request>();

                return true;
            }
            catch (Exception)
            {
                // Log error or raise an error event.
                Console.WriteLine("Initialization Error\n\nPress any key to Exit..\n");

                return false;
            }
        }

        // Check if the elevator should make an enroute pickup.
        private bool ElevatorShouldMakeEnroutePickup(IElevator elevator)
        {
            return requestList.Any(x => x.Floor == elevator.GetCurrentFloor()
                                &&
                                (x.RequestType == RequestType.WALL || x.ElevatorId == elevator.GetId()));
        }

        // Perform an elevator journey for a request.
        private async Task MakeElevatorJourney(Request request, IElevator elevator)
        {
            // Print a message indicating the start of the journey cycle.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("-----Journey Cycle Started-----");
            Console.ForegroundColor = ConsoleColor.White;

            try
            {
                // Print a message indicating the response to the request.
                Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Elevator {elevator.GetId()} responding to {request.RequestType} request for floor {request.Floor}");

                // If the request is on the current floor, open the doors for new passengers.
                if (elevator.GetCurrentFloor() == request.Floor)
                {
                    elevator.ReadyElevatorForNewPassengers();
                    return;
                }

                // Ready the elevator for the journey.
                elevator.ReadyElevatorForJourney(request);

                // Run the journey.
                await RunJourney(request, elevator);

                // Ready the elevator for new passengers after completing the journey.
                elevator.ReadyElevatorForNewPassengers();
            }
            catch (Exception ex)
            {
                // Log error and raise maintenance alert.
                Console.WriteLine(ex.Message);
                elevator.SetToMaintenanceMode();
                _elevatorService.raiseElevatorAlert(elevator.GetId());
                throw ex;
            }

            // Print a message indicating the completion of the journey cycle.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Complete");
            Console.ForegroundColor = ConsoleColor.White;
        }

        // Print the state of controlled elevators.
        private void PrintControlledElevatorState()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nStatus");
            Console.WriteLine();
            foreach (Elevator elevator in controlledElevators)
            {
                Console.WriteLine($"Elevator Id: {elevator.GetId()}");
                Console.WriteLine($"Current Floor: {elevator.GetCurrentFloor()}");
                Console.WriteLine("");
            }
            Console.WriteLine("----");
            Console.ForegroundColor = ConsoleColor.White;
        }

        // Print the remaining requests in the request list.
        private void PrintRemainingRequests(List<Request> elevatorRequests)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (elevatorRequests.Count == 0)
            {
                Console.WriteLine("\nNo Requests. Stationary..\n");
                return;
            }

            Console.WriteLine("\noustanding requests\n");
            foreach (Request request in elevatorRequests)
            {
                string panelRequestString = request.ElevatorId != 0 ? $"Elevator Id: {request.ElevatorId}" : null;
                Console.WriteLine($"Floor: {request.Floor} Type: {request.RequestType} {panelRequestString}");
            }
            Console.WriteLine("\n----------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
