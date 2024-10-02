import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/auth.services';
import { Users } from '../../models/user';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent implements OnInit {

  hasError: boolean = false;
  userAdded: boolean = false;
  missedFields: boolean = false;

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService) { }

  ngOnInit() {
  }

  onForm(userForm: NgForm) {
    console.log(userForm.value);
    this.hasError = false;
    this.missedFields = false;
    this.userAdded = false;

    if (userForm.value["Username"] === '' || userForm.value["Password"] === '' || !userForm.value["Username"] || !userForm.value["Password"] ){
      this.missedFields = true;
      return;
    }

    this.authService.addUser(userForm.value).subscribe(
        (response: Users) => {
          console.log(response);
          userForm.resetForm();
          this.userAdded = true;
        },

        (error) => {
          console.error(error);
          userForm.resetForm();
          this.hasError = true;
        }
    );
      }

    onReset(userForm: NgForm){
      userForm.resetForm();
      this.router.navigate(["admin/users"]);
    }


}



