import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {
  loggedInUser!: string | null;

  constructor() { }

  ngOnInit() {
  }

  isAdmin(){
    return localStorage.getItem('username') === "admin" || localStorage.getItem('username') === "Admin";
  }

  loggedin() {
    this.loggedInUser = localStorage.getItem('username');
    //console.log(this.loggedInUser);
    return this.loggedInUser;
  }

  onLogout() {
    localStorage.removeItem('username');
  }


}

