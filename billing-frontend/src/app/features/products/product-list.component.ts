import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProductService, Product } from './product.service';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './product-list.component.html'
})
export class ProductListComponent implements OnInit {

  products: Product[] = [];

  constructor(
  private productService: ProductService,
  private cdr: ChangeDetectorRef
) {}

  ngOnInit(): void {
  this.loadProducts();

}

ngOnDestroy() {
}

  loadProducts() {

  this.productService.getAll().subscribe({
  next: (data) => {
    this.products = data;
    this.cdr.detectChanges();
  }
});
}
}

