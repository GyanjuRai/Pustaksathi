import { Component } from '@angular/core';
import { OrderSignalRService } from '../../core/services/order-signalR.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  messages: string[] = [];

  constructor(private oss: OrderSignalRService) {}

  ngOnInit(): void {
    this.oss.receiveMessage().subscribe((message: string) => {
      this.messages.push(message);
    });
  }
}
