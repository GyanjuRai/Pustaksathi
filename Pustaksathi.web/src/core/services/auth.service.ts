import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AppConst } from '../../app/app.const';
import * as CryptoJS from "crypto-js";
import { loginResponse, userInfoResponse } from '../../app/shared/auth/auth.model';


@Injectable({ 
    providedIn: 'root'
})
export class AuthService {
    apiUrl: string;
    storageKey: string;
    secretKey: string;
    jwtHelper: JwtHelperService = new JwtHelperService();

    constructor(

    ) {
        this.apiUrl = `${AppConst?.data?.apiBaseUrl}${AppConst?.data?.apiSegment}`;
        this.storageKey = AppConst?.data?.storageKey;
        this.secretKey = AppConst?.data?.secretKey;
    }

    setSession(session: loginResponse) {
      const user = this.jwtHelper.decodeToken(session.token);
      this.setLocalStorage("accessToken", session.token);
      this.setLocalStorage("userId", user?.userId);
      this.setLocalStorage("userName", user?.fullName);
      this.setLocalStorage("email", user?.email);
      this.setLocalStorage("role", session.role);
      this.setLocalStorage("refreshToken", session.refreshToken);
    }

    setUserInfo(user: userInfoResponse) {
      this.setLocalStorage("isDiscountApplied", user?.isDiscountApplied);
      this.setLocalStorage("whiteListId", user?.whiteListId);
      this.setLocalStorage("cartId", user?.cartId);
    }

    setLocalStorage(key: string, value: any) {
        const data = localStorage.getItem(this.storageKey);

        if(data) {
            let dataDec = this.decrypt(data);
            let dataJson = JSON.parse(dataDec ?? "{}");
            dataJson = Object.assign(dataJson, { [key] : value });
            localStorage.setItem(this.storageKey, this.encrypt(JSON.stringify(dataJson)));
        } else {
          let dataJson = Object.assign({}, { [key]: value} );
          localStorage.setItem(this.storageKey, this.encrypt(JSON.stringify(dataJson)));
        }
    }

    getLocalStorage(key: string): any {
      const data = localStorage.getItem(this.storageKey);
      
      if(data) {
        let dataDec = this.decrypt(data);
        let dataJson = JSON.parse(dataDec ?? '{}');
        return dataJson[key] || null;
      } else {
        return null;
      }
    }

    encrypt(value: string): any {
      if (value) {
        return CryptoJS.AES.encrypt(value, this.secretKey);
      } else {
        return null;
      }
    }
    
    decrypt(value: string) {
      if (value) {
        let bytes = CryptoJS.AES.decrypt(value, this.secretKey);
        let decryptedText = bytes.toString(CryptoJS.enc.Utf8);
        return decryptedText;
      } else {
        return null;
      }
    }

    isAuthenticated() : boolean {
      let token = this.getLocalStorage("accessToken");
      if(token && !this.isTokenExpired()) {
        return true;
      } else {
        return false;
      }
    }

    isTokenExpired() : boolean {
      let token = (this.getLocalStorage("accessToken") ?? '') as string;
      if(token){
        return this.jwtHelper.isTokenExpired(token);
      } else {
        return true;
      }
    }

    clearAuth() {
      this.clearSession();
      this.clearStorage();
    }

    clearSession() {
      sessionStorage.clear();
    }

    clearStorage() {
      localStorage.clear();
    }


}