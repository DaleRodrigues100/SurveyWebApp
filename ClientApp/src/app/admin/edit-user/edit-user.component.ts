import { ValueConverter } from '@angular/compiler/src/render3/view/template';
import { Component, OnInit } from '@angular/core';
import { NgForm, NgModel } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/auth.services';
import { Users } from '../../models/user';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.scss']
})
export class EditUserComponent implements OnInit {


  username: any;
  userEdited: boolean = false;
  error: boolean = false;
  missedField: boolean = false;


  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService) { }

  ngOnInit() {
  }

  Back() {
    this.router.navigate(["admin/users"]);

  }


  editUser() {
    this.router.navigate(["admin/editUser"]);
  }

  onSubmit(form: NgForm){
    
    this.userEdited = false;
    this.error = false;
    this.missedField = false;


    this.username = localStorage.getItem("userToEdit");
    var password = form.value.Password;

    if (password === "" || !password){

      this.missedField = true;
      return;

    }

    console.log(password);

    if (this.username != null){

      var formValue: Users ={ Username: this.username, Password: password};

      console.log(formValue);

      this.authService.editUser(formValue).subscribe(
        (response) => {
          console.log(response);
          this.userEdited = true;
        },

        (error) => {
          console.error(error.status);
          this.error = true;

        }
      );

    }

    else {

      console.error(401);

    }


  }

}

