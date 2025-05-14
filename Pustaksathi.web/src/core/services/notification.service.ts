import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SnackbarComponent, SnackbarData } from '../../app/shared/components/snackbar/snackbar.component';

@Injectable({
  providedIn: 'root'
})

export class NotificationService {

    constructor(
        private snackBar: MatSnackBar,
    ) 
    { }

    showSnakcbar(message: string, type: SnackbarData['type']) {
        this.snackBar.openFromComponent(SnackbarComponent, {
            data: { message, type },
            duration: 5000,
            horizontalPosition: 'end',
            verticalPosition: 'top',
            panelClass: ['custom-snackbar-panel'] 
        });
    }
}