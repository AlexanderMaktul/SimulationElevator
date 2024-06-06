using SimulationElevator.Enums;

namespace SimulationElevator.Models
{
    // Class representing a request made to the elevator system
    public class Request
    {
        // Gets or sets the floor associated with the request
        public int Floor { get; set; }

        // Gets or sets the ID of the elevator associated with the request
        public int ElevatorId { get; set; }

        // Gets or sets the type of request (from wall or panel)
        public RequestType RequestType { get; set; }
    }
}
