import { Injectable } from '@angular/core';
import { environment } from '../environment/environment';
import { environment as environmentProd } from '../environment/environment.prod';

@Injectable({
    providedIn: 'root'
})
export class AppConst {
    static data: any;

    constructor() {

    }

    async load(): Promise<any> {

        const filePath = `/assets/${environment.production ? environmentProd.config : environment.config}?t=${new Date().getTime()}`;
        const response = await fetch(filePath);

        if(response.ok) {
            try{
                AppConst.data = await response.json();
            } catch (error: any) {
                console.error('============================ ERROR ============================');
                console.error(`Could not find file ${filePath}. \nMESSAGE: ${error.message}`);
                console.error('============================ ERROR ============================');
            }

            if(AppConst.data && !Object.keys((AppConst.data)).length) {
                console.error('============================ ERROR ============================');
                console.error(`No data found in file ${filePath}`);
                console.error('============================ ERROR ============================');
            }
            
        } else {
            console.error('============================ ERROR ============================');
            console.error(`Failed to load file ${filePath}. \nMESSAGE: Request failed with ${response.status} / ${response.statusText}`);
            console.error('============================ ERROR ============================');
        }
    }
}