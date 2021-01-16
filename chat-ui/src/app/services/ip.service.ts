import { Injectable } from '@angular/core';  
import { HttpClient  } from '@angular/common/http'; 
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IpService {

  constructor(private http:HttpClient) { }  
  public getIPAddress()  
  {  
    return this.http.get("http://api.ipify.org/?format=json");  
  }  
   
  public getIpCliente() {
    return this.http.get<{ip:string}>('https://jsonip.com');
    // .subscribe( data => {
    //   console.log('th data', data);
    // })
  }
  
}
