# Microservices Invoice & Stock System

## Overview
<p>
This project is composed of two independent microservices built with ASP.NET Core:
</p>

<ul>
  <li><b>Stock Service</b>: manages products and stock levels</li>
  <li><b>Invoice Service</b>: handles invoice creation, updates, and validation through Stock Service</li>
</ul>

<p>
The system demonstrates microservices communication via HTTP, concurrency handling, fault tolerance, and basic frontend interaction with Angular.
</p>

---

## Architecture

<pre>
Frontend (Angular)
        ↓
Invoice Service
        ↓ (HTTP)
Stock Service
        ↓
SQL Server
</pre>

<p>
Each service is fully independent and communicates only through REST APIs. There is no shared database between services.
</p>

---

## Stock Service

<p><b>Responsible for product and stock management.</b></p>

### Features
<ul>
  <li>Create, update and delete products</li>
  <li>Retrieve product data</li>
  <li>Decrease stock with concurrency-safe logic</li>
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

## Invoice Service

<p><b>Responsible for invoice lifecycle and validation.</b></p>

### Features
<ul>
  <li>Create invoices with multiple items</li>
  <li>Update invoice data</li>
  <li>Delete invoices</li>
  <li>Validate products via Stock Service</li>
  <li>Generate invoice number automatically</li>
  <li>Finalize (print) invoices with stock validation</li>
  <li>Handle Stock Service unavailability</li>
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
  <li>Create invoices with multiple items</li>
  <li>Finalize (print) invoices</li>
  <li>Loading states for async operations (create & print)</li>
  <li>Basic user feedback and validation</li>
</ul>

---

## Service Communication

<p>Invoice Service communicates with Stock Service via HttpClient.</p>

<ul>
  <li>Validate product existence</li>
  <li>Retrieve product details</li>
  <li>Check and decrease stock</li>
</ul>

---

## Concurrency Handling

<p>Stock updates are handled using atomic database operations to prevent race conditions.</p>

<ul>
  <li>Prevents negative stock values</li>
  <li>Ensures safe concurrent updates</li>
  <li>Maintains consistency under parallel requests</li>
</ul>

---

## Fault Tolerance

<p>If Stock Service is unavailable:</p>

<ul>
  <li>Invoice Service returns HTTP 503 Service Unavailable</li>
  <li>Invoice operations are blocked to prevent inconsistent state</li>
</ul>

---

## Concurrency Test

<pre>
GET /api/invoices/test-concurrency/{productId}
</pre>

<p>
This endpoint simulates multiple simultaneous requests to validate race condition handling in stock updates.
</p>

---

## Data Model Notes

<ul>
  <li>Invoice items store ProductId, Quantity, and ProductDescription</li>
  <li>Product data is retrieved from Stock Service at creation time</li>
  <li>No shared database between microservices</li>
</ul>

---

## Example Flow

<ol>
  <li>User sends invoice creation request from frontend</li>
  <li>Invoice Service validates items via Stock Service</li>
  <li>Product data is fetched and stored in the invoice item</li>
  <li>Stock is decreased atomically when invoice is finalized</li>
  <li>Invoice is persisted in SQL Server</li>
  <li>Response is returned to the client</li>
</ol>

---

## Tech Stack

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

## Notes

<ul>
  <li>Microservices are fully independent</li>
  <li>Communication is done exclusively via HTTP</li>
  <li>No shared database between services</li>
  <li>Product data is resolved via API communication</li>
  <li>Frontend includes basic UX improvements like loading states</li>
</ul>