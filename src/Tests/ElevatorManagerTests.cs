using NUnit.Framework;
using Moq;
using SimulationElevator.Interfaces;
using SimulationElevator.Models;
using SimulationElevator.Enums;







namespace SimulationElevator.Tests
{
    [TestFixture]
    public class ElevatorManagerTests
    {
        [Test]
        public void TestMakeElevatorManagerRequest_ValidRequest()
        {
            // Arrange
            var elevatorServiceMock = new Mock<IElevatorService>();
            var elevatorManager = new ElevatorManager(elevatorServiceMock.Object);
            var request = new Request { Floor = 5, RequestType = RequestType.WALL };
            var elevators = new List<IElevator> { new Elevator(1, 1, 10, 1, 100) };
            elevatorServiceMock.Setup(e => e.GetElevators()).Returns(elevators);

            // Act
            elevatorManager.MakeElevatorManagerRequest(request);

            // Assert
            //Assert.That(elevatorManager.RequestList.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestMakeElevatorManagerRequest_InvalidRequest()
        {
            // Arrange
            var elevatorServiceMock = new Mock<IElevatorService>();
            var elevatorManager = new ElevatorManager(elevatorServiceMock.Object);
            var request = new Request { Floor = 15, RequestType = RequestType.WALL };
            var elevators = new List<IElevator> { new Elevator(1, 1, 10, 1, 100) };
            elevatorServiceMock.Setup(e => e.GetElevators()).Returns(elevators);

            // Act
            elevatorManager.MakeElevatorManagerRequest(request);

            // Assert
            //Assert.That(elevatorManager.RequestList, Is.Empty);
        }

        [Test]
        public void TestChooseElevatorDependingOnRequestType_PanelRequest()
        {
            // Arrange
            var elevatorServiceMock = new Mock<IElevatorService>();
            var elevatorManager = new ElevatorManager(elevatorServiceMock.Object);
            var request = new Request { Floor = 5, RequestType = RequestType.PANEL, ElevatorId = 1 };
            var elevators = new List<IElevator> { new Elevator(1, 1, 10, 1, 100) };
            elevatorServiceMock.Setup(e => e.GetElevators()).Returns(elevators);

            // Act
            //var chosenElevator = elevatorManager.ChooseElevatorDependingOnRequestType(request);

            // Assert
            //Assert.That(chosenElevator, Is.Not.Null);
            //Assert.That(chosenElevator.GetId(), Is.EqualTo(1));
        }
    }
}
