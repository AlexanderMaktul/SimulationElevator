using SimulationElevator.Models;
using SimulationElevator.Interfaces;
using SimulationElevator.Enums;


namespace SimulationElevator.Utilities
{
    // Service class for simulating elevator manager tests
    public class ElevatorManagerTestSimulatorService : IElevatorManagerTestSimulatorService
    {
        #region Variables and Constructor

        private readonly IElevatorManager _manager;

        // Constructor to inject the elevator manager dependency
        public ElevatorManagerTestSimulatorService(IElevatorManager manager)
        {
            _manager = manager;
        }
        #endregion

        // Method to run an automated simulation of elevator manager tests
        public void automatedSimulation()
        {
            Random randomRequest = new Random();

            // Generate random requests at every key press until Escape is pressed
            do
            {
                for (int i = 0; i < 5; i++)
                {
                    int randomRequestType = randomRequest.Next(1, 3);

                    _manager.MakeElevatorManagerRequest(new Request
                    {
                        Floor = randomRequest.Next(-3, 21),
                        RequestType = (RequestType)randomRequestType,
                        ElevatorId = randomRequestType == 2 // If generating a Panel request, generate a random Elevator Id
                            ? randomRequest.Next(1, 3)
                            : 0
                    });
                }

            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }

        // Method to run a manual simulation of elevator manager tests
        public void manualSimulation()
        {
            // Simulate triggering requests, the Elevator manager allocate Elevators to pick them up.
            // Elevator defaults set in ElevatorService.GetElevators() (Next Bookmark)
            // Modify any requests for Testing as necessary
            _manager.MakeElevatorManagerRequest(new Request { Floor = 2, RequestType = RequestType.WALL });
            _manager.MakeElevatorManagerRequest(new Request { Floor = 12, RequestType = RequestType.WALL });
            _manager.MakeElevatorManagerRequest(new Request { Floor = 7, RequestType = RequestType.PANEL, ElevatorId = 1, });
            _manager.MakeElevatorManagerRequest(new Request { Floor = 4, RequestType = RequestType.WALL });

            _manager.MakeElevatorManagerRequest(new Request { Floor = 1, RequestType = RequestType.WALL });
            // Simulates 2 people getting into Elevator 2 and pressing Floor 3 and 5 buttons on panel
            _manager.MakeElevatorManagerRequest(new Request { Floor = 3, RequestType = RequestType.PANEL, ElevatorId = 2, });
            _manager.MakeElevatorManagerRequest(new Request { Floor = 5, RequestType = RequestType.PANEL, ElevatorId = 2, });
        }
    }
}
