# Server Monitoring and Notification System

This project implements a server monitoring and notification system using C# with ASP.NET Core, SignalR, RabbitMQ, and MongoDB. The system collects server statistics, detects anomalies, and sends real-time alerts to connected clients.

## Features

- **Server Statistics Collection**: Collects memory usage, available memory, and CPU usage at configurable intervals.
- **Message Queueing**: Publishes server statistics to a message queue.
- **Data Persistence**: Stores server statistics in a MongoDB instance.
- **Anomaly Detection**: Monitors server performance and sends alerts based on configurable thresholds.
- **Real-time Alerts**: Sends alerts via SignalR for anomalies and high usage.
- **Client Subscription**: A console client that listens for alerts and prints them to the console.

## Technologies Used

- ASP.NET Core
- SignalR
- RabbitMQ
- MongoDB
- C#

## Project Structure

```
.
├── ServerStatisticsCollector      # Contains the service that collects and publishes server statistics
│   ├── Program.cs                 # Entry point for the statistics collection service
│   ├── ServerStatistics.cs         # Class definition for server statistics
│   └── appsettings.json            # Configuration for sampling intervals and server identifier
│
├── MessageProcessingService        # Contains the service for processing messages and detecting anomalies
│   ├── Program.cs                 # Entry point for the message processing service
│   ├── AnomalyDetector.cs          # Logic for detecting anomalies
│   ├── MongoRepository.cs          # Abstraction for MongoDB interactions
│   └── appsettings.json            # Configuration for anomaly detection thresholds
│
├── SignalRAlertService             # Contains the service for sending alerts via SignalR
│   ├── Program.cs                 # Entry point for the SignalR alerting service
│   ├── AlertHub.cs                # Definition of the SignalR hub
│   └── appsettings.json            # Configuration for SignalR
│
├── SignalRClient                   # Contains the console client for receiving alerts
│   └── Program.cs                 # Entry point for the SignalR client
│
└── README.md                      # Project documentation (this file)
```

## Prerequisites

To run this application, you will need:

- .NET 6 SDK or later (download from [here](https://dotnet.microsoft.com/download)).
- MongoDB installed and running locally or remotely.
- RabbitMQ installed and running locally or remotely.

## Getting Started

### Clone the Repository

Clone the repository to your local machine:

```bash
git clone https://github.com/<YourUsername>/ServerMonitoringSystem.git
cd ServerMonitoringSystem
```

### Run the Server Statistics Collector

1. Navigate to the `ServerStatisticsCollector` directory:

   ```bash
   cd ServerStatisticsCollector
   ```

2. Install dependencies:

   ```bash
   dotnet restore
   ```

3. Run the server statistics collector:

   ```bash
   dotnet run
   ```

   This service will start collecting server statistics and publishing them to the message queue.

### Run the Message Processing Service

1. Open a new terminal or command line window.

2. Navigate to the `MessageProcessingService` directory:

   ```bash
   cd MessageProcessingService
   ```

3. Install dependencies:

   ```bash
   dotnet restore
   ```

4. Run the message processing service:

   ```bash
   dotnet run
   ```

   This service will listen for messages from the message queue, persist them to MongoDB, and check for anomalies.

### Run the SignalR Alert Service

1. Open a new terminal or command line window.

2. Navigate to the `SignalRAlertService` directory:

   ```bash
   cd SignalRAlertService
   ```

3. Install dependencies:

   ```bash
   dotnet restore
   ```

4. Run the SignalR alert service:

   ```bash
   dotnet run
   ```

   This service will host a SignalR hub that clients can connect to for receiving alerts.

### Run the SignalR Client

1. Open a new terminal or command line window.

2. Navigate to the `SignalRClient` directory:

   ```bash
   cd SignalRClient
   ```

3. Install dependencies:

   ```bash
   dotnet restore
   ```

4. Run the SignalR client:

   ```bash
   dotnet run
   ```

   The client will connect to the SignalR hub and listen for alerts, printing them to the console.

## Configuration

Each service has a configuration file named `appsettings.json`. Here are the main configuration options:

### ServerStatisticsCollector `appsettings.json`
```json
{
  "ServerStatisticsConfig": {
    "SamplingIntervalSeconds": 60,
    "ServerIdentifier": "linux1"
  }
}
```

### MessageProcessingService `appsettings.json`
```json
{
  "AnomalyDetectionConfig": {
    "MemoryUsageAnomalyThresholdPercentage": 0.4,
    "CpuUsageAnomalyThresholdPercentage": 0.5,
    "MemoryUsageThresholdPercentage": 0.8,
    "CpuUsageThresholdPercentage": 0.9
  },
  "SignalRConfig": {
    "SignalRUrl": "http://localhost:5000/alertHub"
  }
}
```

### SignalRAlertService `appsettings.json`
```json
{
  "SignalRConfig": {
    "SignalRUrl": "http://localhost:5000/alertHub"
  }
}
```




