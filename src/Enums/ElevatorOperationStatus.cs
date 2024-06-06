namespace SimulationElevator.Enums
{
    // Defines the possible operational statuses for an elevator
    public enum ElevatorOperationStatus
    {
        UP = 0,                         // The elevator is moving upwards
        DOWN = 1,                       // The elevator is moving downwards
        MAINTENANCE = 2,                // The elevator is under maintenance
        ONLINE = 3,                     // The elevator is online and operational (changed from 1 to avoid conflict with DOWN)
        OVERLOADED = 4,                // The elevator is overloaded and cannot move
        ENROUTE = 5 // The elevator is en route to pick up a passenger
    }
}
