import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { User } from './user.model';
import { Router } from '@angular/router';
import { api } from '../app.api';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  userName: string = "";

  http = inject(HttpClient);
  router = inject(Router);
  
  login(){
    this.http.post<User>(api + "login", {userName: this.userName}).subscribe({
      next: (res: User)=> {
        localStorage.setItem("userId", res.id.toString());
        this.router.navigateByUrl("");
      },
      error: (err: HttpErrorResponse)=> {
        alert(err.error.message);
      }
    })
  }
}
