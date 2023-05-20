import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Message, User } from '../login/user.model';
import { api } from '../app.api';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SignalRServiceService } from '../signal-rservice.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {
  userId = localStorage.getItem("userId");
  user: User = new User();
  users: User[] = [];
  messages: Message[] = [];
  message: Message = new Message();  

  signalRService = inject(SignalRServiceService)
  http = inject(HttpClient);
  activatedRote = inject(ActivatedRoute);
  router = inject(Router);

  ngOnInit(): void {
    this.getUsers();
    this.activatedRote.params.subscribe((res)=> {      
      if(res["userId"]!= undefined){
        this.getUser(res["userId"]);
      }
    });
  }

  ngOnDestroy(): void {
    this.signalRService.leaveGroup(this.message.chatId.toString());
  }

  selectUser(userId: number){
    this.signalRService.leaveGroup(this.message.chatId.toString());
    this.router.navigateByUrl("/" + userId);
  }

  getUser(userId: number){
    this.http.get<User>(api + "GetUser/" + userId).subscribe((res)=> {
      this.user = res;
        this.getMessages();      
    });
  }

  getUsers() {
    this.http.get<User[]>(api + "GetUsers/" + this.userId).subscribe((res) => {
      this.users = res;
    });
  }

  getMessages() {
    this.http.post<any>(api + "GetChatMessages", { userId: this.userId, toUserId: this.user.id }).subscribe((res) => {
      this.messages = res.messages;
      this.message.chatId = res.chatId;
      this.message.userId = +this.userId;  

      this.signalRService.startConnection(this.message.chatId.toString()).then(()=> {
        this.signalRService.addTransferChatDataListener((message)=> {
          this.messages.push(message)
        });
      });          
    });
  }

  postMessage(){
    this.http.post<any>(api + "PostMessage", this.message).subscribe(res=> {
      this.message.text = "";      
    });
  }
}
