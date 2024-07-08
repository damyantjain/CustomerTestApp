# Customer Application

## Overview
The Customer Application is a WPF-based client for managing customers, utilizing a gRPC backend service for efficient communication. The application supports CRUD operations (Create, Read, Update, Delete) and real-time filtering of customer data.

## Features
- Add, update, delete, and list customers.
- Real-time filtering of customers by name and email.
- Instant customer form validations
- Responsive and interactive UI with instant feedback.
- Efficient data handling using gRPC streaming.

## Project Setup
1. **Clone the repository**
2. **Configure Startup Projects**
   - In Visual Studio, right-click on the solution and select "Set Startup Projects..."
   - Choose "Multiple startup projects"
   - Set the Action to "Start" for both CustomerTestApp.Service and CustomerTestApp.WPF
   - Ensure CustomerTestApp.Service is higher in the order than CustomerTestApp.WPF
3. **Verify backend service port**
   - The backend service should run by default on http://localhost:7003
   - If it runs on a different port, update the appsettings.json file in the frontend project to point to the correct address.
4. **Run the Application**
   - Start the application. On the first run, the SQLite database will be created with some mock data.
     
## Technologies Used

### Frontend
- **WPF (Windows Presentation Foundation)**: Used for building the user interface.
- **MVVM (Model-View-ViewModel) Pattern**: Ensures separation of concerns.
- **Messenger**: Used for communication between View Models
- **gRPC**: For communication with the backend service.

### Backend
- **ASP.NET Core**: Used for building the gRPC service.
- **Entity Framework Core**: For Database access and management.
- **SQLite**: Lightweight database for storing customer data.
- **Serilog**: Logging framework for capturing and storing logs.
- **gRPC**: For efficient communication with the frontend client.

