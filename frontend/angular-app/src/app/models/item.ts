export interface InventoryItem {
  id: string;
  sku: string;
  name: string;
  quantityOnHand: number;
  reservedQuantity: number;
  available: number; 
}
