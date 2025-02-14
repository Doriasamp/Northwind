# Northwind

Capstone Project from "C# 13 and .NET9 Modern Cross-Platform Development Fundamentals" text book.  

# Getting started
Clone the repo, the solution file in the root directory is named `Northwind.sln`. Open the solution in Visual Studio 2022 and run which project you like.  

# Runs
- Northwind.Web is the non-interactive web app that uses EF Core starting from a ASP.NET Core Empty project template.  
- Northwind.Blazor is the interactive web app that uses EF Core starting from a Blazor Web App project template.  
- Northwind.WebApi is the RESTful API that uses EF Core starting from a ASP.NET Core Web API project template.  
- Northwind.WebApi is the client Blazor App that uses the RESTful API Northwind.WebApi.  
	You first need to run the WebApi, and then this Blazor project so that it can render pages after making calls to the API.

# Data
The Sqlite database is part of the solution in the root directory. The database is named `Northwind.db` and contains the Northwind database schema.  