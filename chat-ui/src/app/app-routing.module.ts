import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ChatComponent } from './components/chat/chat.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { AuthGuard } from './services/auth.guard';
//import { SignupComponent } from './components/signup/signup.component';
//import { AuthGuard } from './services/auth/auth.guard';
const routes: Routes = [
  {
    path: '', component: LoginComponent,
   },
  {
    path: 'login',  component: LoginComponent,
   },
   {
    path: 'signup',  component: SignupComponent
   },
  {
    path: 'chat',  component: ChatComponent, canActivate: [AuthGuard]
   },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }


