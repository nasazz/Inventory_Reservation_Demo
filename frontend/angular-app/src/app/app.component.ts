import { Component } from '@angular/core';
import { ItemsComponent } from './items/items.component';
// import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ItemsComponent],
  templateUrl: './app.component.html',
  // styleUrls: ['./app.component.css']
})
export class AppComponent {}
