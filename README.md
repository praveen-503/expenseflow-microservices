# ExpenseFlow - Enterprise Expense Tracker

A modern, enterprise-grade expense tracking system built with ASP.NET Core 10 and Angular, following Clean Architecture and Microservices principles.

## 🚀 Features

- **Multi-Tenant Architecture**: Supports multiple organizations with secure tenant isolation
- **Microservices**: Decoupled services for Expense, User, and Notification management
- **Event-Driven**: Asynchronous communication using Azure Service Bus for real-time updates
- **Advanced Authentication**: JWT-based authentication with role-based access control
- **Real-time Notifications**: Instant notifications for expense approvals and rejections
- **Audit Trail**: Comprehensive tracking of all system operations
- **Modern UI**: Professional and responsive Angular-based frontend

## 🛠️ Tech Stack

### Backend
- **Framework**: ASP.NET Core 10
- **Language**: C# 14
- **Architecture**: Clean Architecture with DDD
- **Database**: PostgreSQL
- **Messaging**: Azure Service Bus
- **Authentication**: JWT Bearer Tokens
- **Infrastructure**: Azure Container Registry, Azure Kubernetes Service

### Frontend
- **Framework**: Angular 21
- **Language**: TypeScript
- **UI Components**: Angular Material
- **State Management**: Angular Signals
- **Styling**: SCSS

### Development Tools
- **Containerization**: Docker, Docker Compose
- **CI/CD**: GitHub Actions
- **Monitoring**: Azure Application Insights
- **API Documentation**: Swagger/OpenAPI

## 📁 Project Structure

```
src/
├── Web/
│   └── expenseflow-client/    # Angular frontend application
├── Services/
│   ├── Expense/             # Expense management microservice
│   ├── Notification/        # Notification microservice
│   └── User/                # User management microservice
└── Shared/
    ├── ExpenseFlow.Shared.Abstractions/  # Shared abstractions
    ├── ExpenseFlow.Shared.Domain/       # Shared domain models
    ├── ExpenseFlow.Shared.Infrastructure/ # Shared infrastructure
    └── ExpenseFlow.Shared.IntegrationEvents/ # Integration events
```

## ⚙️ Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Azure Account](https://azure.microsoft.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/expenseflow-microservices.git
   cd expenseflow-microservices
   ```

2. **Configure Environment Variables**
   Copy the example environment file and fill in your credentials:
   ```bash
   cp .env.example .env
   # Edit .env with your actual values
   ```

3. **Start Backend Services**
   ```bash
   cd src/Services/Expense
   dotnet ef database update
   dotnet run
   ```
   Repeat for User and Notification services as needed.

4. **Start Frontend Application**
   ```bash
   cd src/Web/expenseflow-client
   npm install
   npm start
   ```

## 🔧 Development

### API Endpoints

| Service | Base URL | Description |
|---------|----------|-------------|
| Expense | `/api/v1/expenses` | Expense management |
| User | `/api/v1/users` | User management |
| Notification | `/api/v1/notifications` | Notification delivery |

### Authentication

To access protected endpoints, include the JWT token in the `Authorization` header:

```
Authorization: Bearer <your-jwt-token>
```

### Running Database Migrations

To apply database migrations:

```bash
cd src/Services/Expense
dotnet ef database update
```

## ☁️ Deployment to Azure

### Prerequisites
- Azure CLI installed and configured
- Azure Container Registry created
- Azure App Service deployed

### Deployment Steps

1. **Login to Azure**
   ```bash
   az login
   ```

2. **Build and Push Docker Image**
   ```bash
   cd src/Services/Expense
   docker build -t expenseflow/expense-api .
   docker push expenseflow/expense-api
   ```

3. **Deploy to Azure App Service**
   ```bash
   az webapp up --name expenseflow-expense-api --resource-group rg-expenseflow --sku B1 --docker-image expenseflow/expense-api
   ```

## 🔄 CI/CD

The project uses GitHub Actions for automated build and deployment. Check the `.github/workflows/` directory for pipeline configurations.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support

For issues, feature requests, or questions, please open an issue in the repository.

---

**Built with ❤️ for Enterprise Expense Management**
