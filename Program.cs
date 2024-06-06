// Import necessary namespaces
using SimulationElevator.Utilities;
using SimulationElevator.Interfaces;
using SimulationElevator.Services;
using Microsoft.Extensions.DependencyInjection;

// Define the namespace for the program
namespace SimulationElevator
{
    // Define the Program class
    class Program
    {
        // Main method, entry point of the program
        static void Main(string[] args)
        {
            // Set up dependency injection using Microsoft.Extensions.DependencyInjection
            ServiceProvider serviceProvider = new ServiceCollection()
                    // Register the ElevatorManager as a singleton service
                    .AddSingleton<IElevatorManager, ElevatorManager>()
                    // Register the ElevatorService as a singleton service for managing elevator data
                    .AddSingleton<IElevatorService, ElevatorService>()
                    // Register the ElevatorManagerTestSimulatorService to handle simulation
                    .AddSingleton<IElevatorManagerTestSimulatorService, ElevatorManagerTestSimulatorService>()
                    // Build the service provider, enabling dependency injection
                    .BuildServiceProvider();

            // Retrieve the IElevatorManager and IElevatorManagerTestSimulatorService instances from the service provider
            IElevatorManager manager = serviceProvider.GetService<IElevatorManager>();
            IElevatorManagerTestSimulatorService simulator = serviceProvider.GetService<IElevatorManagerTestSimulatorService>();

            try
            {
                // Choose whether to run in automated or manual mode
                bool automatedMode = true; // Set to true for automated mode
                // Provide help text if in automated mode
                string automatedModeHelp = automatedMode ? " automated mode.press keys to generate  requests ..." : null;

                // Start the ElevatorManager on a separate thread
                Task task = Task.Factory.StartNew(async () =>
                {
                    // Run the ElevatorManager and get the exit code
                    int exitCode = await manager.RunElevatorManager();
                    // Exit the application with the given exit code
                    Environment.Exit(exitCode);
                });

                // Inform the user of the key press options
                Console.WriteLine($"Press Escape at any point to Exit. Press any Key to Continue {automatedModeHelp}");
                // Exit the application if the Escape key is pressed
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }

                // Run the appropriate simulation mode based on the automatedMode flag
                if (automatedMode)
                {
                    // Run the automated simulation
                    simulator.automatedSimulation();
                }
                else
                {
                    // Run the manual simulation
                    simulator.manualSimulation();
                }
            }
            catch (Exception)
            {
                // Handle exceptions by logging error or raising an event, then exit with code -1
                Environment.Exit(-1);
            }
        }   }
}
