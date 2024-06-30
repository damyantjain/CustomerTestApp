# Customer Test Application

## Overview
The Customer Test Application is a WPF-based client for managing customers, utilizing a gRPC backend service for efficient communication. The application supports CRUD operations (Create, Read, Update, Delete) and real-time filtering of customer data.

## Features
- Add, update, delete, and list customers.
- Real-time filtering of customers by name and email.
- Instant customer form validations
- Responsive and interactive UI with instant feedback.
- Efficient data handling using gRPC streaming.

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
