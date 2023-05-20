import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  router = inject(Router);  

  checkLogin(){
    let userId = localStorage.getItem("userId");
    console.log(userId);
    if(userId == undefined){
      this.router.navigateByUrl("auth/login");
      return false;
    }
    return true;
  }
}
