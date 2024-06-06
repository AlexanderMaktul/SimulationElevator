// IMetricsManager.cs
namespace SimulationElevator.Interfaces
{
    public interface IMetricsManager
    {
        void CollectMetric(string metricName, double value);
        void ExportToExcel(string filePath);
    }
}
