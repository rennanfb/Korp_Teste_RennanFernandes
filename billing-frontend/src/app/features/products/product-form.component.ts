import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProductService, Product } from './product.service';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-product-form',
  imports: [FormsModule, RouterModule, CommonModule],
  templateUrl: './product-form.component.html'
})
export class ProductFormComponent {

isLoading = false;

  product: Product = {
    code: '',
    description: '',
    stock: 0
  };

  constructor(private productService: ProductService, private router: Router) {}

  save() {
  if (!this.product.code || !this.product.description) {
    alert('Fill all fields');
    return;
  }

  this.isLoading = true;

  this.productService.create(this.product).subscribe({
    next: () => {
      alert('Product created successfully!');
      this.product = { code: '', description: '', stock: 0 };
      this.router.navigate(['/products']);
    },
    error: (err) => {
      alert(err.error || 'Error creating product');
      this.isLoading = false;
    },
    complete: () => {
      this.isLoading = false;
    }
  });
}
}