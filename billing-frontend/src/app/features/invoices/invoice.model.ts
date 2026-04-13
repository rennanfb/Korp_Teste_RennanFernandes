export interface InvoiceItem {
  productId: number;
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