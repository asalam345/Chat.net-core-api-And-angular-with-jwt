import { AfterViewChecked, Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { chatMesage } from 'src/app/data/ChatMessage';
import { AuthService } from 'src/app/services/auth/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { SignalrService } from '../../services/signalr.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, AfterViewChecked {
  @ViewChild('scrollMe') private myScrollContainer: ElementRef;
  title = 'chat-ui';
  text: string = "";
  public users: any[];
  receiverId: number;
  receiverName: string;
  senderId:number;
  selected: boolean = false;
  public allMessages: chatMesage[];
  firstName:string;
  lastName:string;
  constructor(public signalRService: SignalrService, private router: Router
    , private authService: AuthService, private chatService: ChatService) {  }

  ngOnInit(): void {
    if (localStorage.getItem('email') == null){
      this.logout();
    }
    this.signalRService.connect();
    this.senderId = +localStorage.getItem('id');
    this.firstName = localStorage.getItem('firstName');
    this.lastName = localStorage.getItem('lastName');

    this.authService.getUsers(this.senderId).subscribe(s =>{
      this.users = s.data;
      this.receiverId = s.data[0].userId;
      this.onSelectUser(this.receiverId);
    });
    //this.scrollToBottom();
  }
  ngAfterViewChecked() {        
    this.scrollToBottom();        
} 

scrollToBottom(): void {
    try {
        this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;
    } catch(err) { }                 
}
logout(){
  localStorage.clear();
  this.authService.loginStatus(this.senderId, false);
  this.router.navigate(['']);
}
onEnter(){
  this.sendMessage();
}
onSelectUser(id:number){
  this.receiverId = id;
  this.senderId = +localStorage.getItem('id');
 
  this.getMessage();
}
getMessage():void{
  const data = {
    ReceiverId: this.receiverId,
    SenderId: this.senderId
  };
   this.chatService.getMessage(data).subscribe(s =>{
    const user:any = this.users.filter(f => +f.userId == +this.receiverId);
    this.receiverName = user[0].firstName + ' ' + user[0].lastName;
    this.signalRService.messages = s.data.map(m =>  {
      return {
        Text: m.message,
        DateTime: m.date,
        Time:m.time,
        SenderId: m.senderId,
        ReceiverId: m.receiverId,
        ChatId: m.chatId,
        IsDeleteFromReceiver: m.isDeleteFromReceiver,
        IsDeleteFromSender: m.isDeleteFromSender
      }
    });
    this.allMessages = this.signalRService.messages;
    console.log(this.signalRService.messages);
  });
}
sendMessage(): void {
  if(this.text.length == 0)return;
  this.signalRService.sendMessageToHub(0,this.text,this.senderId, this.receiverId).subscribe({
    next: _ => {this.text = ''; this.getMessage();},
    error: (err) => console.error(err)
  });
}
deleteMessage(id:number){
this.chatService.delete(id).subscribe(s =>{
   this.signalRService.messages = this.signalRService.messages.filter(item => item.ChatId !== id);
   this.signalRService.sendMessageToHub(id,this.text,this.senderId, this.receiverId).subscribe({
      next: _ => {this.text = ''; this.getMessage();},
      error: (err) => console.error(err)
    });
});
}
public deleteOneSide(id:number, sender:number){
  const isDeleteFromReceiver = sender === this.receiverId ? true : false;
  this.chatService.deleteOneSide(id, isDeleteFromReceiver).subscribe(s =>{
    this.signalRService.messages = this.signalRService.messages.filter(item => item.ChatId !== id);
    this.getMessage();
 });
}
public showpopup:Array<boolean> = [];
popup4remove(id) {
  this.showpopup[id] = !this.showpopup[id];// ? false : true;
}
}
