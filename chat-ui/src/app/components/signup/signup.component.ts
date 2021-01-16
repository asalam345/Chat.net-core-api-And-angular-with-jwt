import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {
  gotoUrl = '';
  @Input() public email: string;
  @ViewChild('email') emailAddress: Element;
  constructor(private fb: FormBuilder,
              private router: Router,
              private toastr: ToastrService,
              private activatedRoute: ActivatedRoute,
              private authService: AuthService) {
                 
               }

signupForm: FormGroup;
loginFormEncrypt: FormGroup;

ngOnInit(): void {
// this.activatedRoute.queryParams.subscribe(params => {
//   this.gotoUrl = params.returnUrl === undefined ? '' : params.returnUrl;
// });
this.signupForm = this.fb.group({
  FirstName: ['', [Validators.required]],
  LastName: ['', [Validators.required]],
  Email: [this.email, [Validators.email, Validators.required]],
});
}
// tslint:disable-next-line: typedef
onSubmit() {
  if(this.signupForm.get('Email').value == ''){
    this.signupForm.setValue({Email:this.email,FirstName:this.signupForm.get('FirstName').value,LastName:this.signupForm.get('LastName').value});
  //alert(this.signupForm.get('FirstName').value); alert(this.signupForm.get('Email').value);
}
  if (this.signupForm.invalid) {
  this.toastr.error('Carefully fill the from', 'Error!');
  return;
  }

  this.authService.signup(this.signupForm.value)
  .subscribe((user: any) => {
    if (user.data.email != null){
      this.authService.localStorageSet(user);
      this.authService.loginStatus(user.data.userId, true);
      this.router.navigate(['chat']);
    }
    else {
    const message = user.message;
    this.toastr.error(message, 'Signup Failed');
    }
  },
  error => {
  console.log(error);
  });
  }
}
