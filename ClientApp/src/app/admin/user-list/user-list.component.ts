import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Users } from '../../models/user';
import { AuthService } from 'src/app/auth.services';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {

  users: any;
  string_users: any;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {

    this.Getall();

    this.authService.Refreshrequired.subscribe(response=>{
      this.Getall();
    });

  }

  Getall() {
    this.authService.getUsers().subscribe(result=>{
      this.string_users = JSON.stringify(result);
      this.users = JSON.parse(this.string_users);
      console.log(this.users);
    })
  }


  onDelete(username: string){

    this.authService.deleteUser(username).subscribe(
      (response) => {

        console.log(response);

      },

      (error) => {
        console.error(error.status);

      }
    );

  }

  onEdit(username: string){

    localStorage.setItem('userToEdit', username);
    this.router.navigate(['admin/editUser']);
  }


  onAdd(){
    this.router.navigate(['admin/addUser']);
  }

  Back() {
    this.router.navigate(['admin']);
  }


}
