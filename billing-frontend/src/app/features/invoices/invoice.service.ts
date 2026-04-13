import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Invoice, CreateInvoice } from './invoice.model';


@Injectable({
  providedIn: 'root'
})
export class InvoiceService {

  private baseUrl = 'https://localhost:7169/api/invoices';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Invoice[]> {
    return this.http.get<Invoice[]>(this.baseUrl);
  }

  create(invoice: CreateInvoice): Observable<Invoice> {
  return this.http.post<Invoice>(this.baseUrl, invoice);
}

  print(id: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/print`, {});
  }

  getById(id: number): Observable<Invoice> {
    return this.http.get<Invoice>(`${this.baseUrl}/${id}`);
  }
}