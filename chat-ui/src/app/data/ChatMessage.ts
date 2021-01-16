export interface chatMesage {
  Text: string;
  ConnectionId: string;
  DateTime: Date;
  Time: string;
  SenderId: number;
  ReceiverId:number;
  ChatId: number;
  IsDeleteFromReceiver:boolean;
  IsDeleteFromSender:boolean;
  IsChnaged:boolean;
}
