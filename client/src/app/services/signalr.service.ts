import * as signalR from '@microsoft/signalr';
import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private hubConnection?: signalR.HubConnection;
  constructor(private auth: AuthService) {}

  startConnection() {
    const token = this.auth.getToken();
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/taskhub', { accessTokenFactory: () => token ?? '' })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(err => console.error('SignalR connect error', err));
  }

  on(event: string, cb: (data: any) => void) {
    this.hubConnection?.on(event, cb);
  }

  stop() {
    this.hubConnection?.stop();
  }
}
