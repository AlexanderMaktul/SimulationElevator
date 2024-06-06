namespace SimulationElevator.Enums
{
    // Defines the type of elevator request
    public enum RequestType
    {
        WALL = 1,  // Request made from a wall-mounted panel (e.g., outside the elevator)
        PANEL = 2  // Request made from an inside panel (e.g., inside the elevator)
    }
}
