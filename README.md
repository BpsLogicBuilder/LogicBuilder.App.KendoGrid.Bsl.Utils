# LogicBuilder.App.KendoGrid.Bsl.Utils

A .NET library that provides utilities for dynamically generating and executing queries for Kendo UI Grid data sources with Entity Framework Core integration.

## Overview

This library takes a request for a Kendo Grid data source including entity and model types along with specifications for paging, grouping, sorting, filtering, and aggregation. Related components dynamically generate and execute the queries using LINQ expressions, finally returning the data source result.

## Features

- **Dynamic Query Generation**: Automatically generates LINQ queries based on Kendo Grid DataSourceRequest parameters
- **Entity Framework Core Integration**: Seamlessly works with EF Core through `LogicBuilder.EntityFrameworkCore`
- **Expression Extensions**: Leverages `LogicBuilder.Kendo.ExpressionExtensions` for advanced query building
- **BSL Business Logic Support**: Integrates with `LogicBuilder.App.KendoGrid.Bsl.Business` for business rule processing
- **Comprehensive Grid Operations**: Supports:
  - Paging
  - Sorting
  - Filtering
  - Grouping
  - Aggregations

## Dependencies

- **LogicBuilder.App.Common.Utils**
- **LogicBuilder.App.KendoGrid.Bsl.Business**
- **LogicBuilder.EntityFrameworkCore**
- **LogicBuilder.Kendo.ExpressionExtensions**

## Target Framework

- .NET 10.0

## Installation

	- dotnet add package LogicBuilder.App.KendoGrid.Bsl.Utils

Or via NuGet Package Manager:

- Install-Package LogicBuilder.App.KendoGrid.Bsl.Utils

## Links

- [GitHub Repository](https://github.com/BpsLogicBuilder/LogicBuilder.App.KendoGrid.Bsl.Utils)
- [LogicBuilder Project](https://github.com/BpsLogicBuilder/LogicBuilder)

## Copyright

Copyright © BPS 2026