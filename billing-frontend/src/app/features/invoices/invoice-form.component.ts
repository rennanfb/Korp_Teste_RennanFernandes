import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { InvoiceService } from './invoice.service';
import { Invoice, InvoiceItem } from './invoice.model';
import { ProductService, Product } from '../products/product.service';
import { ChangeDetectorRef } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-invoice-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './invoice-form.component.html'
})
export class InvoiceFormComponent implements OnInit {

  products: Product[] = [];

  items: InvoiceItem[] = [];

  selectedProductId: number | null = null;
  quantity: number = 1;

  constructor(
    private invoiceService: InvoiceService,
    private productService: ProductService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit(): void {
  this.productService.getAll().subscribe({
    next: (data) => {
      this.products = data;

      this.cdr.detectChanges();
    }
  });
}

  addItem() {
  if (!this.selectedProductId) {
    alert('Select a Product');
    return;
  }

  if (this.quantity <= 0) {
    alert('Quantity must be greater than 0');
    return;
  }

  const product = this.products.find(p => p.id === this.selectedProductId);

  this.items.push({
  productId: this.selectedProductId,
  quantity: this.quantity,
  description: product?.description
});
  this.selectedProductId = null;
  this.quantity = 1;

  this.cdr.detectChanges();
}

  save() {
    const invoice = {
  items: this.items
};

    this.invoiceService.create(invoice).subscribe({
  next: (res) => {
    alert(`Invoice ${res.number} created!`);

    this.router.navigate(['/invoices']);

    this.items = [];
  },
  error: (err) => {
  alert(err.error || 'Failed to create invoice');
}
});
  }
}