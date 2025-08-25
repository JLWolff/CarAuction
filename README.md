# Car Auction API

 
## ✅ Core Requirements

- **Add Vehicle**: Ensure the vehicle's unique identifier is not already in use. Raise an error if a duplicate is detected.
- **Search Vehicles**: Search by type, manufacturer, model, or year. Return all matching vehicles.
- **Auction Management**: Start/close auctions for vehicles. Only one active auction per vehicle. Verify vehicle existence and auction status before starting.
- **Place Bids**: Validate that the auction is active and the bid amount exceeds the current highest bid.


## 📦 Business Assumptions

- **Currency**: All bids are in a single currency (e.g., USD).
- **Weights**: Vehicle weights and load capacities are specified in kilograms (kg).
- **Unique Identifiers**: Each vehicle must have a unique identifier; duplicates are not allowed.


## 🛠️ Technical & Design Decisions

- **Layered Architecture**: Clean separation of Presentation (API), Application (business logic), Domain (entities, interfaces), Infrastructure (data access), and Cross-Cutting (middleware).
- **Repository Pattern**: Abstracts data access for maintainability and testability.
- **Factory Pattern**: Used for flexible object creation, supporting multiple vehicle types.
- **Domain-Driven Design (DDD)**: Encapsulates business rules and logic in the Domain layer.
- **Exception Handling**: Custom exceptions and middleware provide error handling and clear API responses.


## 🏗️ Architecture

The solution follows a layered architecture with the following layers:

- **Presentation Layer** (`CarAuctionApi.Presentation`): Contains Web API controllers, endpoint routing, and configuration. Responsible for handling HTTP requests and responses.
- **Application Layer** (`CarAuctionApi.Application`): Implements business logic, application services, and Data Transfer Objects (DTOs). Coordinates domain operations and enforces use case rules.
- **Domain Layer** (`CarAuctionApi.Domain`): Defines core business entities, domain interfaces, and business rules. Encapsulates the heart of the auction logic and validation.
- **Infrastructure Layer** (`CarAuctionApi.Infrastructure`): Manages data access, persistence, and integration with external systems using Entity Framework Core. Implements repository interfaces defined in the domain.
- **Cross-Cutting Layer** (`CarAuctionApi.CrossCutting`): Provides shared concerns such as exception handling middleware, logging, and other utilities used across layers.

## 🧪 Unit Tests Overview

The project includes unit tests to ensure reliability and correctness:

- **Coverage**: Tests cover domain entities, business logic, application services and exception handling scenarios.
- **Tools**: Utilizes xUnit for test structure and Moq for mocking dependencies.
- **Approach**: Follows the AAA (Arrange, Act, Assert) pattern for clarity and maintainability.
- **Location**: All tests are located in the `tests/CarAuctionApi.UnitTests` directory.

Run all tests with:
```bash
dotnet test
```
## 🎨 Extra! Modern UI

- The project includes a full-featured UI built with **Next.js 15** using **React** from boilerplate code.
- Implements a **Back End for Front End (BFF)** pattern for secure and efficient API communication.
- Styled with **Tailwind CSS** for rapid, responsive design.
- The UI covers all core functionalities: vehicle management, searching, auction lifecycle, and bidding.

## 🚀 Running the Application

You can run both the client and server using the provided Makefile and Docker Compose setup:

### Using Makefile

- **Build all images:**
	```bash
	make build
	```
- **Run only the server:**
	```bash
	make run-server
	```
- **Run both client and server:**
	```bash
	make run-all
	```

The client will be available at [http://localhost:3000](http://localhost:3000) and the server API at [http://localhost:5001](http://localhost:5001).



