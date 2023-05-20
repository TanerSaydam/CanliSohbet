import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr'
import { signalRApi } from './app.api';
import { Message } from './login/user.model';

@Injectable({
  providedIn: 'root'
})
export class SignalRServiceService {

  private hubConnection: signalR.HubConnection;
  
  public startConnection = (chatId: string) => {
    this.hubConnection = new signalR.HubConnectionBuilder()
                            .withUrl(signalRApi)
                            .build();
  return this.hubConnection
  .start()
  .then(()=> {
    console.log("Connection started");
    console.log(chatId);
    return this.hubConnection.invoke("JoinGroup", chatId);    
  })
  .catch(err=> {
    console.log("Error while starting connection:" + err)
    throw err;
  });
  }


  public leaveGroup = (chatId: string) => {
    this.hubConnection?.invoke("LeaveGroup", chatId)
    .catch(err=> console.error(err));
  }


  public addTransferChatDataListener = (callBack: (message: Message)=> void) =>{
    this.hubConnection.on("ReceiveMessage", (message)=> {
      callBack(message);
    });
  }
}
