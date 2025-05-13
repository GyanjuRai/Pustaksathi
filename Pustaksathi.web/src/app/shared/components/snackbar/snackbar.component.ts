import { Component, Inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from '@angular/material/snack-bar';

@Component({
  selector: 'app-snackbar',
  templateUrl: './snackbar.component.html',
  styleUrl: './snackbar.component.css'
})
export class SnackbarComponent {
  colorClass: string;
  icon: string;

  constructor(
    public snackBarRef: MatSnackBarRef<SnackbarComponent>,
    @Inject(MAT_SNACK_BAR_DATA) public data: SnackbarData
  ) {
    switch (data.type) {
      case 'success':
        this.colorClass = 'text-success';
        this.icon = 'check_circle';
        break;
      case 'error':
        this.colorClass = 'text-danger';
        this.icon = 'error';
        break;
      case 'warning':
        this.colorClass = 'text-warning';
        this.icon = 'warning';
        break;
      case 'info':
        this.colorClass = 'text-info';
        this.icon = 'info';
        break;
      case 'pending':
        this.colorClass = 'text-warning';
        this.icon = 'hourglass_empty';
        break;
      case 'delete':
        this.colorClass = 'text-danger';
        this.icon = 'delete';
        break;
      default:
        this.colorClass = 'text-secondary';
        this.icon = 'info';
    }
  }
}

export interface SnackbarData {
  message: string;
  type: 'success' | 'error' | 'warning' | 'info' | 'pending' | 'delete';
}
