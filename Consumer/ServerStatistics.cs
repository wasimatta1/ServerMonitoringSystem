namespace Consumer
{
    public class ServerStatistics
    {
        public string ServerIdentifier { get; set; }
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; } // In MB
        public double AvailableMemory { get; set; } // In MB
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return $"ServerIdentifier: {ServerIdentifier}, CpuUsage: {CpuUsage}, MemoryUsage: {MemoryUsage}, AvailableMemory: {AvailableMemory}, Timestamp: {Timestamp}";

        }
    }
}
