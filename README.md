Microservices Invoice & Stock System
Overview
<p> This project is composed of two independent microservices built with ASP.NET Core: </p> <ul> <li><b>Stock Service</b>: manages products and stock levels</li> <li><b>Invoice Service</b>: handles invoice creation and validates products through Stock Service</li> </ul> <p> The system demonstrates microservices communication via HTTP, concurrency handling, and fault tolerance in distributed services. </p>
Architecture
<pre> Frontend (Angular) ↓ Invoice Service ↓ (HTTP) Stock Service ↓ SQL Server </pre> <p> Each service is fully independent and communicates only through REST APIs. There is no shared database between services. </p>
Stock Service
<p><b>Responsible for product and stock management.</b></p> <h3>Features</h3> <ul> <li>Create, update and delete products</li> <li>Retrieve product data</li> <li>Decrease stock with concurrency-safe logic</li> </ul> <h3>Endpoints</h3> <pre> GET /api/products/{id} POST /api/products PUT /api/products/{id} DELETE /api/products/{id} POST /api/products/{id}/decrease </pre>
Invoice Service
<p><b>Responsible for invoice creation and validation.</b></p> <h3>Features</h3> <ul> <li>Create invoices with multiple items</li> <li>Validate products via Stock Service</li> <li>Generate invoice number automatically</li> <li>Handle Stock Service unavailability</li> </ul> <h3>Endpoints</h3> <pre> POST /api/invoices GET /api/invoices GET /api/invoices/{id} GET /api/invoices/test-concurrency/{productId} </pre>
Service Communication
<p> Invoice Service communicates with Stock Service via HttpClient. </p> <ul> <li>Validate product existence</li> <li>Retrieve product details</li> <li>Decrease stock during invoice creation</li> </ul>
Concurrency Handling
<p> Stock updates are handled using atomic database operations to prevent race conditions. </p> <ul> <li>Prevents negative stock values</li> <li>Ensures safe concurrent updates</li> <li>Maintains consistency under parallel requests</li> </ul>
Fault Tolerance
<p> If Stock Service is unavailable: </p> <ul> <li>Invoice Service returns HTTP 503 Service Unavailable</li> <li>Invoice creation is blocked to prevent inconsistent state</li> </ul>
Concurrency Test
<pre> GET /api/invoices/test-concurrency/{productId} </pre> <p> This endpoint simulates multiple simultaneous requests to validate race condition handling in stock updates. </p>
Data Model Notes
<ul> <li>Invoice items store only ProductId and Quantity</li> <li>Product name is retrieved from Stock Service at runtime</li> <li>No shared database between microservices</li> </ul>
Example Flow
<ol> <li>User sends invoice creation request from frontend</li> <li>Invoice Service validates items via Stock Service</li> <li>Stock is decreased atomically in the database</li> <li>Invoice is persisted in SQL Server</li> <li>Response is returned to the client</li> </ol>
Tech Stack
<ul> <li>ASP.NET Core Web API</li> <li>Entity Framework Core</li> <li>SQL Server</li> <li>AutoMapper</li> <li>HttpClient</li> <li>Angular</li> <li>Swagger</li> </ul>
Notes
<ul> <li>Microservices are fully independent</li> <li>Communication is done exclusively via HTTP</li> <li>No shared database between services</li> <li>Product data is resolved at request time</li> </ul>