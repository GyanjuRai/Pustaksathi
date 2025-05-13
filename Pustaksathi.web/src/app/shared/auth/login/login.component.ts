import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { AuthService } from '../../../../core/services/auth.service';
import { UtilityService } from '../../../../core/services/utility.service';
import { AccountService } from '../../../../core/services/account.service';
import { NotificationService } from '../../../../core/services/notification.service';
import { CustomValidationService } from '../../../../core/services/custom-validation.service';
import { loginResponse, userIdParm, userInfoResponse, userLoginParam } from '../auth.model';
import { ResponseModel } from '../../../../core/model/base.model';
import { EnumResponse } from '../../../../core/enum/base.enum';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit, OnDestroy {

  private _unSubscribeAll: Subject<any>;
  formGroup!: FormGroup;
  showPassword = false;

  constructor(
    public fb: FormBuilder,
    public auth: AuthService,
    private util: UtilityService,
    private acc: AccountService,
    private ns: NotificationService,
  ) {
    this._unSubscribeAll = new Subject();
  }

  ngOnInit(): void {
    
    this.formGroup = this.fb.group({
      email: ['', [Validators.required, CustomValidationService.emailValidator]],
      password: ['', [Validators.required, CustomValidationService.passwordValidator]]
    });

    if(!this.auth.isAuthenticated()){
      this.getUserInfo()
    }
  }

  login() {
    this.util.formUtility(this.formGroup, 'validate');
    if(this.formGroup.dirty && this.formGroup.valid) {
      const value = this.util.formUtility(this.formGroup, 'value');
      const param = {
        email: value.email,
        passwordHash: value.password
      } as userLoginParam;

      this.acc.login(param)
      .pipe(takeUntil(this._unSubscribeAll))
      .subscribe((response: ResponseModel<loginResponse>) => {
        if(response.data != null && response.type === EnumResponse.success){
          this.auth.setSession(response.data);
          this.util.formUtility(this.formGroup, 'reset');
          this.ns.showSnakcbar(response.message, 'success');
        }
        else if(response.type === EnumResponse.failed){
          this.ns.showSnakcbar(response.message, 'warning');
        }
        else if(response.type === EnumResponse.noRecordFound) {
          this.ns.showSnakcbar(response.message, 'info');
        }
        else{
          this.ns.showSnakcbar(response.message, 'error');
        }
      })
    }
  }

  getUserInfo() {
    const userId = parseInt(this.auth.getLocalStorage('userId') ?? '0');
    const param = {
      userId: userId
    } as userIdParm;

    this.acc.getUserInfo(param)
    .pipe(takeUntil(this._unSubscribeAll))
    .subscribe((response: ResponseModel<userInfoResponse>) => {
      if(response.data != null && response.type === EnumResponse.success){
        this.auth.setUserInfo(response.data);
      }
    })
  }

    togglePasswordVisibility() : void {
    this.showPassword = !this.showPassword;
  }


  ngOnDestroy(): void {
    this._unSubscribeAll.next(null);
    this._unSubscribeAll.complete();
  }
}
