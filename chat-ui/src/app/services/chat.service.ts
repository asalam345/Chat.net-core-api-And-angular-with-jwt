import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Common } from './common/common';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  constructor(private http: HttpClient) { }
   public sendMessage(data: any)
  {
    const body = JSON.stringify(data);
    //const reqHeader = new HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'True' });
    return this.http.post(Common.baseUrl + 'api/Chat', body);//, { headers: reqHeader }

  }
  public getMessage(data:any){
    //const reqHeader = new HttpHeaders({ 'No-Auth': 'True' });
    return this.http.get<any>(Common.baseUrl + 'api/Chat/?SenderId=' + data.SenderId + '&ReceiverId='  + data.ReceiverId);//, { headers: reqHeader }
  }
  public delete(id:number){
    //const reqHeader = new HttpHeaders({ 'Content-Type': 'application/json', 'No-Auth': 'True' });
    return this.http.delete<boolean>(Common.baseUrl + 'api/Chat/' + id);//, { headers: reqHeader }
  }
  public deleteOneSide(id:number, isDeleteFromReceiver:boolean){
    const data = {
      ChatId : id,
      IsDeleteFromReceiver: isDeleteFromReceiver
    }
    const body = JSON.stringify(data);
    const reqHeader = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.put(Common.baseUrl + 'api/Chat', body, { headers: reqHeader });//
  }
}
