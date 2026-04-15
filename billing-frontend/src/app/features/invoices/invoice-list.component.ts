import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { InvoiceService } from './invoice.service';
import { Invoice, InvoiceItem } from './invoice.model';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-invoice-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './invoice-list.component.html'
})
export class InvoiceListComponent implements OnInit {

  loadingInvoiceId: number | null = null;

  invoices: Invoice[] = [];

  constructor(
  private invoiceService: InvoiceService,
  private cdr: ChangeDetectorRef
) {}

  ngOnInit(): void {
    this.load();
  }

  load() {
  this.invoiceService.getAll().subscribe({
    next: (data) => {
      this.invoices = data;
      this.cdr.detectChanges(); 
    },
    error: (err) => console.error(err)
  });
}

  print(id: number) {
  this.loadingInvoiceId = id;

  this.invoiceService.print(id).subscribe({
    next: () => {
      alert('Invoice printed and finalized');
      this.load();
    },
    error: (err) => {
      alert(err.error || 'Failed to print invoice');
      this.loadingInvoiceId = null;
      this.cdr.detectChanges();
    },
    complete: () => {
      this.loadingInvoiceId = null;
    }
  });
}
}