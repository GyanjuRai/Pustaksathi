import { Injectable } from '@angular/core';
import { WebApiService } from './web-api.service';
import { Observable } from 'rxjs';
import * as CryptoJS from "crypto-js";
import { ResponseModel } from '../model/base.model';
import { AttributeItem } from '../model/attribute.model';
import { AppConst } from '../../app/app.const';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';


@Injectable({
    providedIn: 'root'
})

export class UtilityService {

    secretKey: string;

    constructor(
        private api: WebApiService,
        private router: Router,
        private activeRoute: ActivatedRoute
    ) 
    { 
        this.secretKey = AppConst?.data?.secretKey ?? '';
    }

    getAttributeCategory(param?: object, showLoader = false, refresh = false): Observable<ResponseModel<AttributeItem[]>> {
        return this.api.get("Utility/AttributeItemsGetById", param, false, showLoader, refresh);
    }

     formUtility(form: FormGroup, type: string, fields?: string[]): any {

        if (form && type !== '') {
    
          if (type === 'validate') {
    
            const validateControl = (form: FormGroup) => {
              Object.keys(form.controls).forEach(field => {
                const control = form.get(field);
                if (control instanceof FormArray) {
                  (<FormArray>form.get(field)).controls.forEach((group: any) => {
                    (<any>Object).values(group.controls).forEach((formArrayCtrl: FormControl) => {
                      formArrayCtrl.markAsTouched({ onlySelf: true });
                      formArrayCtrl.setErrors(formArrayCtrl.errors);
                    })
                  });
                }
                if (control instanceof FormGroup) {
                  validateControl(control);
                  return;
                }
                control!.markAsTouched({ onlySelf: true });
                control!.setErrors(control!.errors);
              });
            }
            validateControl(form);
    
            return true;
          } else {
    
            const value = form.getRawValue();
    
            if (type === 'value' && (!fields || fields.length == 0)) {
    
              return value;
            } else if (type === 'value' && fields!.length > 0) {
    
              for (const field of fields!) {
    
                if (!value.hasOwnProperty(field)) {
                  delete value[field];
                }
              }
    
              return value;
            } else if (type === 'errors') {
    
              const errors = new Map();
    
              for (const field in fields) {
    
                if (!value.hasOwnProperty(field)) {
                  continue;
                }
    
                const control = form.get(field);
    
                if (control && control.dirty && !control.valid) {
                  errors.set(field, control.errors);
                }
              }
    
              return errors;
            } else if (type === 'changes') {
    
              const dirty = new Map();
              for (const field of fields!) {
    
                if (form.controls[field].dirty) {
    
                  dirty.set(field, value[field].trim());
                }
              }
    
              return dirty;
            }
          }
        }
      }

      isNullOrEmpty(data: any): boolean {

        if (([null, undefined, '', '{}', '[]'].includes(data)
          || (Array.isArray(data) && data.length == 0)
          || (!Array.isArray(data) && typeof (data) == 'object' && Object.keys(data).length == 0)
        ) && data instanceof Date == false) {
          return true;
        }
    
        return false;
      }

      encrypt(value: string): any {
        if (value) {
          return CryptoJS.AES.encrypt(value, this.secretKey).toString();
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

    navigateToUtl(url: string, ...params: string[]) {
      const fullPath = [url, ...params]; 
      this.router.navigate(fullPath, { replaceUrl: true });
    }

    async getValueFromRoute(key: string): Promise<any> {
      await this.activeRoute.paramMap.subscribe(params => {
        var dcryptResponse = this.decrypt(params.get(key) ?? '') ?? '';
        return dcryptResponse;
      });
    }

      /*
     !Merge two array of objects, removes duplicate objects
     =======================================
     Implementation:
                     TODO Input: arrayObj1  = [{name:"a"},{name:"b"}];
                     TODO Input: arrayObj2  = [{name:"a"},{name:"c"}];
                     mergedArrayObj = uniqueMergeArrayObj(arrayObj1, arrayObj2);
                     ? Output: [{name:"a"},{name:"b"},{name:"c"}]
  */
  uniqueMergeArrayObj(arr1: any[], arr2?: any[]) {

    const duplicateArray = [...arr1, ...arr2 ?? []];
    return [...new Set(duplicateArray.map(o => JSON.stringify(o)))].map(s => JSON.parse(s));
  }
}