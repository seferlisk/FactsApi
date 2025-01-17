# FactsApi

## Description
This is an API aggregation service that consolidates data from three external APIs and provides a unified endpoint to access the 
aggregated information. It is a scalable and efficient system that fetches data from various sources, aggregates it, and delivers it
through a single API interface.

## Features
- Fetches facts from multiple external APIs (e.g., Cat Facts, Dog Facts, Ninja Facts).
- Provides a unified API endpoint to access aggregated facts.
- Supports filtering by category and limits the number of results.
- Designed for scalability, allowing easy integration of new APIs.
- Unit tests implemented using **xUnit** for ensuring reliability.

---

## API Endpoints

### 1. `GET /api/Facts`
Retrieve aggregated facts from multiple sources.

#### Request Parameters
| Parameter | Type     | Description                                     | Required | Default |
|-----------|----------|-------------------------------------------------|----------|---------|
| `limit`   | `int`    | The maximum number of facts to retrieve.        | No       | 20      |
| `category`| `string` | The category to filter facts (e.g., "cats").    | No       | None    |

#### Example Request
GET /api/Facts?limit=10&category=cats

### Example Response
{
  "facts": [
    {
      "text": "Cats have five toes on their front paws but only four on their back paws.",
      "category": "Cats"
    },
    {
      "text": "Dogs' sense of smell is at least 40x better than humans.",
      "category": "Dogs"
    }
  ]
}

## Adding a New API Service
This API aggregation service is designed with scalability in mind. Future APIs can be seamlessly integrated into the existing framework.
Below are the steps to add new API services:

1. **Create a New Service:**
   - Implement a new service class in the `Services` directory that fetches facts from the new API.
   - Ensure the new service adheres to the `IFactsService` interface.

2. **Register the New Service:**
   - Register the new service in the `Program.cs` file.

3. **Update the Aggregator:**
   - Modify the existing `FactsAggregateService` to include the new service in the aggregation logic.

4. **Update the Controller:**
   - Update the corresponding controller actions to handle requests for the new category of facts.

Example for new service registration in `Program.cs`:

public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IFactService, CatFactService>();
    services.AddScoped<IFactService, DogFactService>();
    services.AddScoped<IFactService, NinjaFactService>();
    services.AddScoped<IFactService, NewApiFactService>(); // Register new API service
}


## Getting Started

### Prerequisites
.NET 6 or later
Serilog for logging

### Installation
Step-by-step instructions to install and set up your project.

# Clone the repository
git clone https://github.com/seferlisk/FactsApi.git
cd FactsApi

# Install dependencies
dotnet restore

# Set up environment variables
Add API keys for external APIs (e.g., Ninja Facts) to the appsettings.json file or your environment configuration.

# Run the project
dotnet run

# Access the API
Navigate to http://localhost:<port>/api/Facts in your browser or use a tool like Postman.

## Unit Testing
Unit tests are implemented using xUnit. These tests ensure the correctness and reliability of the API aggregation service.

### Running Tests
To run the tests, use the following command: dotnet test
