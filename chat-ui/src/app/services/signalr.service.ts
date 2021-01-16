import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr'
import { from } from 'rxjs';
import { tap } from 'rxjs/operators';
import { chatMesage } from '../data/ChatMessage';
import { MessagePackHubProtocol } from '@microsoft/signalr-protocol-msgpack'
import { Common } from './common/common';
@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private hubConnection: HubConnection
  public messages: chatMesage[] = [];
  private connectionUrl = 'https://localhost:44319/signalr';
  private apiUrl = 'https://localhost:44319/api/chat';
  name: string;

  constructor(private http: HttpClient) {
    const fName = localStorage.getItem('firstName');
    const lName = localStorage.getItem('lastName');
    this.name = fName + ' ' + lName;
   }

  public connect = () => {
    this.startConnection();
    this.addListeners();
  }
  public sendMessage(message: string, sender:number, receiver:number)
  {
    const data = {
      Message : message,
      SenderId: sender,
      ReceiverId : receiver
    }
    const body = JSON.stringify(data);
    const reqHeader = new HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'True' });
    return this.http.post(Common.baseUrl + 'api/Chat', body, { headers: reqHeader });

  }
  public sendMessageToApi(chatId:number,message: string, sender:number, receiver:number) {
    return this.http.post(this.apiUrl, this.buildChatMessage(chatId,message,sender,receiver))
      .pipe(tap(_ => console.log("message sucessfully sent to api controller")));
  }

  public sendMessageToHub(chatId:number, message: string, sender:number, receiver:number) {
    var promise = this.hubConnection.invoke("BroadcastAsync", this.buildChatMessage(chatId,message,sender,receiver))
      .then(() => { console.log('message sent successfully to hub'); })
      .catch((err) => console.log('error while sending a message to hub: ' + err));
    return from(promise);
  }

  private getConnection(): HubConnection {
    return new HubConnectionBuilder()
      .withUrl(this.connectionUrl)
      .withHubProtocol(new MessagePackHubProtocol())
      //  .configureLogging(LogLevel.Trace)
      .build();
  }

  private buildChatMessage(chatId:number, message: string, sender:number, receiver:number): chatMesage {
    //;
    return {
      ConnectionId: this.hubConnection.connectionId,
      Text: message,
      DateTime: new Date(),
      SenderId: sender,
      ReceiverId: receiver,
      Time: '',
      ChatId: chatId,
      IsDeleteFromReceiver:false,
      IsDeleteFromSender:false,
      IsChnaged:false
    };
  }

  private startConnection() {
    this.hubConnection = this.getConnection();

    this.hubConnection.start()
      .then(() => console.log('connection started'))
      .catch((err) => console.log('error while establishing signalr connection: ' + err))
  }

  private addListeners() {
    this.hubConnection.on("messageReceivedFromApi", (data: chatMesage) => {
      console.log("message received from API Controller");
      this.messages.push(data);
    })
    this.hubConnection.on("messageReceivedFromHub", (data: chatMesage) => {
      console.log("message received from Hub");
      if(!data.IsChnaged){
      this.messages.push(data);
      console.log(this.messages);
      }else{
        this.messages = this.messages.filter(f => f.ChatId != data.ChatId);
        console.log(this.messages);
      }
    })
    this.hubConnection.on("newUserConnected", _ => {
      console.log("new user connected")
    })
  }
}



