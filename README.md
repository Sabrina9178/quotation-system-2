# QuotationSystem2
A WPF-based desktop quotation system designed to minimize manual errors and improve workflow efficiency. 

Built with .NET Core, Entity Framework Core, and MS SQL Server, the application follows Clean Architecture with an MVVM pattern, incorporating layered design principles and localization support for multi-language use.

## Features
- **Role-based access**: Separate permissions for administrators and standard users (employees).
  <p align="left">
  <img src="Resources/Login.gif" 
      width="800">
  </p>
  <p align="center" >
    <table>
      <tr>
        <td>
          <figure>
            <img src="Resources/AccountManagement_Admin.jpg" width="500">
            <br>
            <figcaption align="center">Admin has access to edit, create, and delete user.</figcaption>
          </figure>
        </td>
        <td>
          <figure>
            <img src="Resources/AccountManagement_User.jpg" width="500">
            <br>
            <figcaption align="center">User has no access.</figcaption>
          </figure>
        </td>
      </tr>
      <tr>
        <td>
          <figure>
            <img src="Resources/Quotation_Admin.jpg" width="500">
            <br>
            <figcaption align="center">Admin has access to edit, delete, and export records.</figcaption>
          </figure>
        </td>
        <td>
          <figure>
            <img src="Resources/Quotation_User.jpg" width="500">
            <br>
            <figcaption align="center">User can only export records.</figcaption>
          </figure>
        </td>
      </tr>
    </table>
  </p>
  
- **Quotation operation**: Generate quotations by entering input values. Combobox options and prices update dynamically based on selected products and previous choices.
  <p align="left">
  <img src="Resources/QuotationOperation_speedx2.gif" 
      width="800">
  </p>
- **Quotation history**: View, edit, delete, or export past quotation records. Editing and deletion are restricted to administrators.
  <p align="left">
  <img src="Resources/QuotationDeleteAndExport_speedx1.8.gif" 
      width="800">
  </p>
    
- **Price table management** (in progress): Allows administrators to update product options and pricing.
- **Localization support**: Provides a multilingual interface, currently supporting English (EN-US) and Traditional Chinese (ZH-TW).
  

## Technical Stack
- **Frameworks & UI**: .NET Core WPF with XAML
- **Architecture**: Clean Architecture with layered design and MVVM pattern
- **Database**: SQL Server with Entity Framework Core
- **Practices**: Exception handling, asynchronous operations
- **Tools**: Git for version control, Visual Studio for development

*“System Architecture” diagram/section (even a simple ASCII sketch or description), so recruiters can visualize how your layers (UI, Application, Domain, Infrastructure) interact

## System Design & Implementation Details
- **Layered architecture**: Presentation, Application, Data Access layers
- **UI consistency**: UserControls and templates for reusable views
- **MVVM**: Data binding with CommunityToolkit.Mvvm
- **Localization**:
  - Static content via ResourceDictionary + localization extension
  - Dynamic content stored in database
- **Abstraction**: Interfaces and services to enforce architectural boundaries
- **Database**: Normalized relational schema with ER diagram
<p align="center">
  <img src="Resources/ERdiagram.png"
       alt="ER Diagram"
       width="1000">
</p>

## Getting Started
### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 + with WPF workload
- SQL Server

### Setup
1. **Clone the repository**
  ```sh  
  git clone https://github.com/username/quotation-system.git
  cd quotation-system
  ```
2. **Configure database connection**
   - Open the solution in Visual Studio.
   - Update the SQL Server connection string in appsettings.json.
3. **Apply migrations**
  ```sh  
  dotnet ef database update
  ```
  This will create the database schema automatically.
4. **Run**
  - Press F5 in Visual Studio
  - Or run from CLI:
  ```sh
  dotnet run --project QuotationSystem
  ```
