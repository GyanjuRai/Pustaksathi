import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { CustomValidationService } from '../../../../core/services/custom-validation.service';
import { UtilityService } from '../../../../core/services/utility.service';
import { user } from '../auth.model';
import { AccountService } from '../../../../core/services/account.service';
import { EnumResponse } from '../../../../core/enum/base.enum';
import { Router, RouterModule } from '@angular/router';
import { NotificationService } from '../../../../core/services/notification.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit, OnDestroy {
  private _unSubscribeAll: Subject<any>;
  showPassword = false;


  formGroup!: FormGroup; 
  constructor(
    private fb: FormBuilder,
    private util: UtilityService,
    private acc: AccountService,
    private router: Router,
    private ns: NotificationService,
  ) 
  { 
    this._unSubscribeAll = new Subject();
  }

  ngOnInit(): void {
    this.formGroup = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, CustomValidationService.emailValidator]],
      password: ['', [Validators.required, CustomValidationService.passwordValidator]],
    });
  }

  register() {
    this.util.formUtility(this.formGroup, 'validate');
    if(this.formGroup.dirty && this.formGroup.valid) {
        const value = this.util.formUtility(this.formGroup, 'value');
        const param = {
          fullName: value.fullName,
          email: value.email,
          passwordHash: value.password,
          roleId: 3,
          isDiscountApplied: false,
        } as user;

        this.acc.register(param)
        .pipe(takeUntil(this._unSubscribeAll))
        .subscribe((response) => {
          if(response.data != null && response.type === EnumResponse.success){
            this.util.formUtility(this.formGroup, 'reset');
            this.router.navigate(['/login']);
            this.ns.showSnakcbar(response.message, 'success');
          }
          else if(response.type === EnumResponse.someThingWentWrong){
            this.ns.showSnakcbar(response.message, 'error');
          }
          else{
            this.ns.showSnakcbar(response.message, 'info');
          }
        });
    }
  }

  togglePasswordVisibility() : void {
    this.showPassword = !this.showPassword;
  }

  ngOnDestroy(): void {
    this._unSubscribeAll.next(null);
    this._unSubscribeAll.complete();
  }
}
