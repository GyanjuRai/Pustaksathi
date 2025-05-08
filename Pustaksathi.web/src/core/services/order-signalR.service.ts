import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { AppConst } from '../../app/app.const';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class OrderSignalRService {
    private readonly hub = 'orderhub';
    private connection: signalR.HubConnection | null = null;
    private readonly hubUrl = `${AppConst?.data?.apiBaseUrl}${this.hub}` || 'https://localhost:5200/orderhub';

    constructor() {
        this.startConnection();
    }

    private startConnection() {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl(this.hubUrl)
            .build();

        this.connection.start()
            .then(() => console.log('SignalR connection established.'))
            .catch(err => console.error('Error establishing SignalR connection:', err));
    }

    public receiveMessage(): Observable<string> {
        if (this.connection) {
            return new Observable<string>(observer => {
                this.connection!.on('ReceiveMessage', (message: string) => {
                    observer.next(message);
                });
            });

        } else {
            throw new Error('SignalR connection is not established.');
        }
    }
}