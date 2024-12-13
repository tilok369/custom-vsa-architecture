# Custom Vertical Slice Architecture in .NET 8
This project is a blend of both Clean Archtecture and Vertical Slice Architecture.
## Clean Architecture
Clean Architecture is a software design philosophy and architectural approach that is easy to maintain, test and adapt over time. The Core business logic should not depend on external frameworks, tools, or UI implementations rather all should depends of Core.
## Vertical Slice Architecture (VSA)
Vertical Slice Architecture is a software design approach that focuses on structuring a system around "vertical slices" of functionality rather than traditional horizontal layers like presentation, business logic, and data access. Each slice represents a complete feature or use case, encompassing all layers required to deliver that functionality.

# Purpose of this Custome Architecture
Both Clean and VSA archteicture have some cons over their huge pros. I tried to blend both and leveraging the positive features to come up with a solution which is easy to maintain as well as quick to develop. One of the geart feature of VSA is, it organizes codebase around business goals rather than technical concerns which is maintaied here.

# Give it a START :star:
Loving this repository? Show your support by giving this project a star!

[![GitHub stars](https://img.shields.io/github/stars/tilok369/custom-vsa-architecture.svg?style=social&label=Star)](https://github.com/tilok369/custom-vsa-architecture)

## Patterns Used
- Mediator
- CQRS
- Generic Repository

## Technologies Used
- .NET 8 Web API (minimal API)
- MediatR
- Entity Framework Core
- Fluent Validation
- Mapster
- Serilog
- Asp.Versioning
- Carter
- Jwt Token Authorization
- Swagger

# Project Breakdown
## API
This projects exposes the endpoints for consuming. It has all startup services registered.
## Application
This is the main project which contains the business logic. It facilitates the VSA archtecture for each features. Check "Features" folder.
## Domain
This projects contains business specific domain entities.
## Infrastructure
This is the persistent infrastructure project contains the data Add, Update, Delete and GET functionality through Entity Framework as ORM. 

## Inspired By
- [Nadir Badnjevic's VerticalSliceArchitecture](https://github.com/nadirbad/VerticalSliceArchitecture)
- [Jason Taylor's Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture)
- [Milan's Vetical Slice Architecture Video Tutorial](https://www.youtube.com/watch?v=msjnfdeDCmo)


