import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/auth.services';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Users } from '../models/user';



@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})



export class LoginComponent implements OnInit {



  hasError: boolean = false;
  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) { }



  ngOnInit() {
    localStorage.clear();
  }



  onReset(loginForm: NgForm) {
    loginForm.resetForm();
  }



  onLogin(loginForm: NgForm) {
    //console.log(loginForm.value);



    this.authService.authUser(loginForm.value).subscribe(
      (response: Users) => {
        //console.log(response);
        localStorage.setItem('username', loginForm.value.Username);

        let check_admin = loginForm.value.Username;


        if (check_admin === "Admin"){
          check_admin = "admin";

        }

        if (check_admin === "admin") {
          this.router.navigate(['admin']); // else users surveys page
        }

        else {
          this.router.navigate(['user/surveys']); // route to user page
        }



      },



      (error) => {
        console.error(error.status);
        this.hasError = true;
        loginForm.resetForm();
      }
    );




  }
}
