import { Routes } from '@angular/router';
import { AuthService } from './login/auth.service';
import { inject } from '@angular/core';

export const routes: Routes = [
    {
        path: "",
        loadComponent: () => import("./home/home.component").then(c=> c.HomeComponent),
        canActivate: [()=> inject(AuthService).checkLogin()]
    },
    {
        path: ":userId",
        loadComponent: () => import("./home/home.component").then(c=> c.HomeComponent),
        canActivate: [()=> inject(AuthService).checkLogin()]
    }, 
    {
        path: "auth/login",
        loadComponent: ()=> import("./login/login.component").then(c=> c.LoginComponent)
    },
];
