import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ProductService, Product } from './product.service';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-product-form',
  imports: [FormsModule, RouterModule],
  templateUrl: './product-form.component.html'
})
export class ProductFormComponent {

  product: Product = {
    code: '',
    description: '',
    stock: 0
  };

  constructor(private productService: ProductService, private router: Router) {}

  save() {
  this.productService.create(this.product).subscribe({
    next: () => {
      alert('Product created successfully!');

      this.router.navigate(['/products']);

      this.product = { code: '', description: '', stock: 0 };
    },
    error: (err) => {
      alert(err.error || 'Error creating product');
    }
  });
}
}