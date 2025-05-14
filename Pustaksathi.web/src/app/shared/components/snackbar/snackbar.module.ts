import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SnackbarComponent } from './snackbar.component';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';



@NgModule({
  declarations: [
    SnackbarComponent
  ],
  imports: [
    CommonModule,
    MatIconModule,
    MatSnackBarModule,
    MatButtonModule
  ],
  exports: [
    SnackbarComponent
  ]
})
export class SnackbarModule { }
