Invoice System with Microservices
Overview
<p> This project is composed of two independent microservices developed with ASP.NET Core: </p> <ul> <li><b>Stock Service</b>: responsible for product management and stock control</li> <li><b>Invoice Service</b>: responsible for the creation, updating and validation of invoices through the Stock Service</li> </ul> <p> The system demonstrates communication between microservices via HTTP, concurrency handling, fault tolerance and integration with an Angular frontend. </p>
Architecture
<pre> Frontend (Angular) ↓ Invoice Service ↓ (HTTP) Stock Service ↓ SQL Server </pre> <p> Each service is fully independent and communicates exclusively through REST APIs. There is no database sharing between the services. </p>
Stock Service
<p><b>Responsible for product and stock management.</b></p>
Features
<ul> <li>Create, update and delete products</li> <li>Retrieve product data</li> <li>Decrease stock with concurrency-safe logic</li> </ul>
Endpoints
<pre> GET /api/products/{id} POST /api/products PUT /api/products/{id} DELETE /api/products/{id} POST /api/products/{id}/decrease </pre>
Invoice Service
<p><b>Responsible for the invoice lifecycle and validations.</b></p>
Features
<ul> <li>Create invoices with multiple items</li> <li>Update invoice data</li> <li>Delete invoices</li> <li>Validate products via Stock Service</li> <li>Automatically generate invoice number</li> <li>Finalize (print) invoices with stock validation</li> <li>Handle Stock Service unavailability</li> </ul>
Endpoints
<pre> POST /api/invoices GET /api/invoices GET /api/invoices/{id} PUT /api/invoices/{id} DELETE /api/invoices/{id} POST /api/invoices/{id}/print GET /api/invoices/test-concurrency/{productId} </pre>
Frontend (Angular)
Features
<ul> <li>Create products</li> <li>Create invoices with multiple items</li> <li>Finalize (print) invoices</li> <li>Loading states for asynchronous operations (creation and printing)</li> <li>Basic user feedback and validations</li> </ul>
Communication Between Services
<p>The Invoice Service communicates with the Stock Service using HttpClient.</p> <ul> <li>Validation of product existence</li> <li>Retrieval of product data</li> <li>Stock verification and reduction</li> </ul>
Concurrency Handling
<p>Stock updates are performed using atomic operations in the database to avoid race conditions.</p> <ul> <li>Prevents negative stock values</li> <li>Ensures safe updates in concurrent scenarios</li> <li>Maintains consistency in parallel requests</li> </ul>
Fault Tolerance
<p>When the Stock Service is unavailable:</p> <ul> <li>The Invoice Service returns HTTP 503 (Service Unavailable)</li> <li>Invoice operations are blocked to avoid data inconsistency</li> </ul>
Concurrency Testing
<pre> GET /api/invoices/test-concurrency/{productId} </pre> <p> This endpoint simulates multiple simultaneous requests to validate concurrency handling in stock. </p>
Notes on the Data Model
<ul> <li>Invoice items store ProductId, Quantity and ProductDescription</li> <li>Product data is obtained from the Stock Service at the moment of creation</li> <li>There is no shared database between the microservices</li> </ul>
Workflow
<ol> <li>The user sends a request to create an invoice through the frontend</li> <li>The Invoice Service validates the items via the Stock Service</li> <li>Product data is retrieved and stored in the invoice item</li> <li>Stock is reduced atomically when finalizing the invoice</li> <li>The invoice is persisted in SQL Server</li> <li>The response is returned to the client</li> </ol>
Technologies Used
<ul> <li>ASP.NET Core Web API</li> <li>Entity Framework Core</li> <li>SQL Server</li> <li>AutoMapper</li> <li>HttpClient</li> <li>Angular</li> <li>Swagger</li> </ul>
Technical Details
Angular Lifecycle
<p> The lifecycle hook <b>ngOnInit</b> was used to perform the initial data loading when components are initialized, such as fetching products and invoices from the APIs. </p>
RxJS Usage
<p> RxJS is used indirectly through Angular's HttpClient. All HTTP requests return Observables, which are consumed using <b>subscribe()</b> to handle responses and errors asynchronously. </p>
Libraries Used
<ul> <li><b>HttpClient (Angular)</b>: communication with backend APIs</li> <li><b>ASP.NET Core Web API</b>: implementation of backend services</li> <li><b>Entity Framework Core</b>: data access and persistence in the database</li> <li><b>AutoMapper</b>: mapping between entities and DTOs</li> <li><b>Swagger</b>: documentation and testing of API endpoints</li> </ul>
Visual Components
<p> No external UI library was used. The frontend was built with standard Angular components, HTML and custom CSS. </p>
Dependency Management
<p> Backend dependencies are managed via NuGet and defined in the <b>.csproj</b> files of each microservice. </p>
Frameworks Used in Backend
<p> The backend was developed using ASP.NET Core Web API together with Entity Framework Core. </p>
Error Handling
<p> Error handling is performed using try/catch blocks in critical operations. In case of communication failures with the Stock Service, exceptions such as <b>HttpRequestException</b> are handled and a <b>503 Service Unavailable</b> response is returned to avoid data inconsistency. </p>
LINQ Usage
<p> LINQ is used together with Entity Framework Core for database queries, including methods such as <b>AnyAsync</b>, <b>FindAsync</b> and filtering operations for validation and data retrieval. </p>
Final Notes
<ul> <li>The microservices are fully independent</li> <li>Communication occurs exclusively via HTTP</li> <li>There is no shared database between the services</li> <li>Product data is obtained via communication between APIs</li> <li>The frontend includes basic user experience improvements, such as loading states</li> </ul>