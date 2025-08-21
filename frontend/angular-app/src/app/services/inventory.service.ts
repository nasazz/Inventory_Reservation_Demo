import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InventoryItem } from '../models/item';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private api = `${environment.apiUrl}/items`;

  constructor(private http: HttpClient) {}

  list(): Observable<InventoryItem[]> {
    return this.http.get<InventoryItem[]>(this.api);
  }

  get(id: string) {
    return this.http.get<InventoryItem>(`${this.api}/${id}`);
  }

  create(payload: { id: string; sku: string; name: string; quantityOnHand: number }) {
    return this.http.post<{ id: string }>(this.api, payload);
  }

  reserve(id: string, quantity: number) {
    return this.http.post(`${this.api}/${id}/reserve`, { quantity });
  }

  adjust(id: string, delta: number) {
    return this.http.post(`${this.api}/${id}/adjust`, { delta });
  }

  cancel(id: string, quantity: number) {
    return this.http.post(`${this.api}/${id}/cancel`, { quantity });
  }
}
