# Fiscal Notes System with Microservices

## Overview
<p>
This project is composed of two independent microservices developed with ASP.NET Core:
</p>

<ul>
  <li><b>Stock Service</b>: responsible for the management of products and stock control</li>
  <li><b>Billing Service</b>: responsible for the creation, updating and validation of fiscal notes through the Stock Service</li>
</ul>

<p>
The system demonstrates communication between microservices via HTTP, concurrency handling, fault tolerance and integration with frontend in Angular.
</p>

---

## Architecture

<pre>
Frontend (Angular)
        ↓
Billing Service
        ↓ (HTTP)
Stock Service
        ↓
SQL Server
</pre>

<p>
Each service is totally independent and communicates exclusively through REST APIs. There is no database sharing between the services.
</p>

---

## Stock Service

<p><b>Responsible for the management of products and stock.</b></p>

### Features
<ul>
  <li>Create, update and delete products</li>
  <li>Consult product data</li>
  <li>Reduce stock with logic safe for concurrency</li>
</ul>

### Endpoints
<pre>
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
POST   /api/products/{id}/decrease
</pre>

---

## Billing Service

<p><b>Responsible for the lifecycle of fiscal notes and validations.</b></p>

### Features
<ul>
  <li>Create fiscal notes with multiple items</li>
  <li>Update fiscal note data</li>
  <li>Delete fiscal notes</li>
  <li>Validate products via Stock Service</li>
  <li>Generate note number automatically</li>
  <li>Finalize (print) notes with stock validation</li>
  <li>Handle unavailability of the Stock Service</li>
</ul>

### Endpoints
<pre>
POST   /api/invoices
GET    /api/invoices
GET    /api/invoices/{id}
PUT    /api/invoices/{id}
DELETE /api/invoices/{id}
POST   /api/invoices/{id}/print
GET    /api/invoices/test-concurrency/{productId}
</pre>

---

## Frontend (Angular)

### Features
<ul>
  <li>Create products</li>
  <li>Create fiscal notes with multiple items</li>
  <li>Finalize (print) fiscal notes</li>
  <li>Loading states for asynchronous operations (creation and printing)</li>
  <li>Basic feedback to the user and validations</li>
</ul>

---

## Communication between Services

<p>The Billing Service communicates with the Stock Service using HttpClient.</p>

<ul>
  <li>Validation of the existence of products</li>
  <li>Retrieval of product data</li>
  <li>Verification and reduction of stock</li>
</ul>

---

## Concurrency Handling

<p>Stock updates are made using atomic operations in the database to avoid race conditions.</p>

<ul>
  <li>Avoids negative stock values</li>
  <li>Guarantees safe updates in concurrent scenarios</li>
  <li>Maintains consistency in parallel requests</li>
</ul>

---

## Fault Tolerance

<p>When the Stock Service is unavailable:</p>

<ul>
  <li>The Billing Service returns HTTP 503 (Service Unavailable)</li>
  <li>Fiscal note operations are blocked to avoid data inconsistency</li>
</ul>

---

## Concurrency Test

<pre>
GET /api/invoices/test-concurrency/{productId}
</pre>

<p>
This endpoint simulates multiple simultaneous requests to validate concurrency handling in stock.
</p>

---

## Notes about the Data Model

<ul>
  <li>Fiscal note items store ProductId, Quantity and ProductDescription</li>
  <li>Product data is obtained from the Stock Service at the moment of creation</li>
  <li>There is no shared database between the microservices</li>
</ul>

---

## Workflow

<ol>
  <li>The user sends a fiscal note creation request through the frontend</li>
  <li>The Billing Service validates the items via the Stock Service</li>
  <li>Product data is obtained and stored in the note item</li>
  <li>Stock is reduced atomically when finalizing the note</li>
  <li>The note is persisted in SQL Server</li>
  <li>The response is returned to the client</li>
</ol>

---

## Technologies Used

<ul>
  <li>ASP.NET Core Web API</li>
  <li>Entity Framework Core</li>
  <li>SQL Server</li>
  <li>AutoMapper</li>
  <li>HttpClient</li>
  <li>Angular</li>
  <li>Swagger</li>
</ul>

---

## Technical Details

### Angular Lifecycle

<p>
The lifecycle hook <b>ngOnInit</b> was used to perform the initial loading of data when components are initialized, such as fetching products and fiscal notes from APIs.
</p>

### RxJS Usage

<p>
RxJS is used indirectly through Angular's HttpClient. All HTTP requests return Observables, which are consumed using <b>subscribe()</b> to handle responses and errors asynchronously.
</p>

### Libraries Used

<ul>
  <li><b>HttpClient (Angular)</b>: communication with backend APIs</li>
  <li><b>ASP.NET Core Web API</b>: backend service implementation</li>
  <li><b>Entity Framework Core</b>: data access and persistence</li>
  <li><b>AutoMapper</b>: mapping between entities and DTOs</li>
  <li><b>Swagger</b>: API documentation and testing</li>
</ul>

### Visual Components

<p>
No external UI library was used. The frontend was built with standard Angular components, HTML and custom CSS.
</p>

### Dependency Management

<p>
Backend dependencies are managed via NuGet and defined in each microservice's <b>.csproj</b> files.
</p>

### Backend Frameworks

<p>
The backend was developed using ASP.NET Core Web API together with Entity Framework Core.
</p>

### Error Handling

<p>
Error handling is performed using try/catch blocks in critical operations. In case of communication failures with the Stock Service, exceptions such as <b>HttpRequestException</b> are handled and a <b>503 Service Unavailable</b> response is returned to avoid data inconsistency.
</p>

### LINQ Usage

<p>
LINQ is used together with Entity Framework Core for database queries, including methods such as <b>AnyAsync</b>, <b>FindAsync</b> and filtering operations for validation and data retrieval.
</p>

---

## Final Notes

<ul>
  <li>Microservices are totally independent</li>
  <li>Communication occurs exclusively via HTTP</li>
  <li>There is no shared database between services</li>
  <li>Product data is obtained via API communication</li>
  <li>The frontend includes basic user experience improvements, such as loading states</li>
</ul>