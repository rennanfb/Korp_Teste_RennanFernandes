export interface InvoiceItem {
  productId: number;
  description?: string;
  quantity: number;
}

export interface Invoice {
  id?: number;
  number?: string;
  status: 'Open' | 'Closed';
  items: InvoiceItem[];
}

export interface CreateInvoice {
  items: InvoiceItem[];
}