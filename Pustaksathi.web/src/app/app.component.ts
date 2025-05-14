import { Component } from '@angular/core';
import { WebApiService } from '../core/services/web-api.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  constructor(private api: WebApiService){
  }

  title = 'Pustaksathi';
}
