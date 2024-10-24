
using Consumer.property;

namespace Consumer
{
    internal partial class Program
    {
        public class AnomalyDetector
        {
            private readonly double _memoryUsageAnomalyThresholdPercentage;
            private readonly double _cpuUsageAnomalyThreshold;
            private readonly double _memoryUsageThreshold;
            private readonly double _cpuUsageThreshold;

            public AnomalyDetector(AnomalyDetectionConfig config)
            {
                _memoryUsageAnomalyThresholdPercentage = config.MemoryUsageAnomalyThresholdPercentage;
                _cpuUsageAnomalyThreshold = config.CpuUsageAnomalyThresholdPercentage;
                _memoryUsageThreshold = config.MemoryUsageThresholdPercentage;
                _cpuUsageThreshold = config.CpuUsageThresholdPercentage;
            }
            //Memory High Usage Alert: if ((CurrentMemoryUsage / (CurrentMemoryUsage + CurrentAvailableMemory)) > MemoryUsageThresholdPercentage)
            //CPU High Usage Alert: if (CurrentCpuUsage > CpuUsageThresholdPercentage)
            public bool IsHighUsage(ServerStatistics stats)
            {
                return stats.CpuUsage > _cpuUsageThreshold
                    || (stats.MemoryUsage / (stats.MemoryUsage + stats.AvailableMemory)) > _memoryUsageThreshold;
            }

            // Memory Usage Anomaly Alert: if (CurrentMemoryUsage > (PreviousMemoryUsage* (1 + MemoryUsageAnomalyThresholdPercentage)))
            //CPU Usage Anomaly Alert: if (CurrentCpuUsage > (PreviousCpuUsage* (1 +CpuUsageAnomalyThresholdPercentage)))
            public bool IsAnomalous(ServerStatistics currentStats, ServerStatistics previousStats)
            {
                return currentStats.CpuUsage > previousStats.CpuUsage * (1 + _cpuUsageAnomalyThreshold)
                    || currentStats.MemoryUsage > previousStats.MemoryUsage * (1 + _memoryUsageAnomalyThresholdPercentage);
            }
        }

    }
}
