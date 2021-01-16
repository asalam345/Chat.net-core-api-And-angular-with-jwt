import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  gotoUrl = '';
  public isRegistered = true;
  public email: string = '';
  constructor(private fb: FormBuilder,
              private router: Router,
              private toastr: ToastrService,
              private activatedRoute: ActivatedRoute,
              private authService: AuthService) {
                if (localStorage.getItem('email') != null){
                  this.router.navigate(['chat']);
                }
               }

  loginForm: FormGroup;

  loginFormEncrypt: FormGroup;

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      this.gotoUrl = params.returnUrl === undefined ? '' : params.returnUrl; // '/signup?returnUrl=' +
    });
    this.loginForm = this.fb.group({
      email: ['', [Validators.required]],
    });
  }
  onSubmit() {
    if (this.loginForm.invalid) {
      this.toastr.error('Carefully fill the from', 'Error!');
      return;
    }

    // this.loginFormEncrypt = this.fb.group({
    //   email: this.encryptObj.encryptData(this.loginForm.get('email').value),
    // });
    //console.log(this.loginFormEncrypt);
    this.authService.authenticate(this.loginForm.value)
    .subscribe(data => {
      if (localStorage.getItem('email') == this.loginForm.get('email').value){
        this.router.navigate(['chat']);
      }
      else{
        this.isRegistered = false;
        this.email = this.loginForm.get('email').value;
      }
    },
    error => {
      this.toastr.error(error.error, 'Login Failed');
      console.log(error);
    });
  }
  gotoSignUp(){
    this.router.navigate(['signup'], { queryParams: { returnUrl: this.gotoUrl }});
  }
}

