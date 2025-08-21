import { Component, OnInit } from '@angular/core';
import { InventoryService } from '../services/inventory.service';
import { InventoryItem } from '../models/item';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { v4 as uuidv4 } from 'uuid';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-items',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './items.component.html'
})
export class ItemsComponent implements OnInit {
  items: InventoryItem[] = [];
  creating = false;
  form: FormGroup;
  loading = false;
  error = '';

  constructor(private svc: InventoryService, private fb: FormBuilder) {
    this.form = this.fb.group({
      sku: ['', Validators.required],
      name: ['', Validators.required],
      quantityOnHand: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.svc.list().subscribe({
      next: items => {
        this.items = items.map(i => ({ ...i, available: i.quantityOnHand - i.reservedQuantity }));
        this.loading = false;
      },
      error: err => {
        this.error = err?.error?.detail ?? err.message;
        this.loading = false;
      }
    });
  }

  create(): void {
    if (this.form.invalid) return;
    const id = uuidv4();
    const payload = {
      id,
      sku: this.form.value.sku,
      name: this.form.value.name,
      quantityOnHand: this.form.value.quantityOnHand
    };
    this.svc.create(payload).subscribe({
      next: () => { this.form.reset(); this.load(); },
      error: e => this.error = e?.error?.detail ?? e.message
    });
  }

  reserve(item: InventoryItem): void {
    const qty = 1; // simple quick action, or prompt the user
    this.svc.reserve(item.id, qty).subscribe({
      next: () => this.load(),
      error: e => this.error = e?.error?.detail ?? e.message
    });
  }

  adjust(item: InventoryItem, delta: number): void {
    this.svc.adjust(item.id, delta).subscribe({
      next: () => this.load(),
      error: e => this.error = e?.error?.detail ?? e.message
    });
  }

  cancel(item: InventoryItem): void {
    const qty = 1;
    this.svc.cancel(item.id, qty).subscribe({
      next: () => this.load(),
      error: e => this.error = e?.error?.detail ?? e.message
    });
  }

  // New helper methods for dashboard stats
  getTotalInventory(): number {
    return this.items.reduce((sum, item) => sum + item.quantityOnHand, 0);
  }

  getTotalReserved(): number {
    return this.items.reduce((sum, item) => sum + item.reservedQuantity, 0);
  }
}
