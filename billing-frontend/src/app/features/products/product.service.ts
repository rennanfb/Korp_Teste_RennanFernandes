import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Product {
  id?: number;
  code: string;
  description: string;
  stock: number;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private baseUrl = 'https://localhost:7170/api/products';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Product[]> {
  return this.http.get<Product[]>(this.baseUrl);
}
  create(product: Product): Observable<Product> {
    return this.http.post<Product>(this.baseUrl, product);
  }
}