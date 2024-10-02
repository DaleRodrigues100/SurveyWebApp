import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {Routes, RouterModule} from '@angular/router';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatTabsModule} from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { ChartsModule, WavesModule } from 'angular-bootstrap-md'



import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { AdminHomepageComponent } from './admin/admin-homepage/admin-homepage.component';
import { SurveyListComponent } from './admin/survey-list/survey-list.component';
import { UserSurveyListComponent } from './UserUI/user-survey-list/user-survey-list.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { UserListComponent } from './admin/user-list/user-list.component';
import { AddUserComponent } from './admin/add-user/add-user.component';
import { EditUserComponent } from './admin/edit-user/edit-user.component';
import { CreateSurveyComponent } from './admin/create-survey/create-survey.component';
import { EditSurveyComponent } from './admin/edit-survey/edit-survey.component';
import { UserTakeSurveyComponent } from './UserUI/user-take-survey/user-take-survey.component';
import { ViewResultsComponent } from './admin/view-results/view-results.component';

const appRoutes: Routes = [
  {path: '', component: LoginComponent},
  { path: 'admin', component: AdminHomepageComponent },
  { path: 'admin/surveys', component: SurveyListComponent },
  { path: 'admin/users', component: UserListComponent },
  { path: 'admin/addUser', component: AddUserComponent },
  { path: 'admin/editUser', component: EditUserComponent },
  { path: 'admin/createSurvey', component: CreateSurveyComponent },
  { path: 'admin/editSurvey', component: EditSurveyComponent },
  { path: 'admin/viewResults', component: ViewResultsComponent },
  { path: 'user/surveys', component: UserSurveyListComponent },
  { path: 'user/takeSurvey', component: UserTakeSurveyComponent }


]

@NgModule({
  declarations: [
    AppComponent,
      LoginComponent,
      AdminHomepageComponent,
      SurveyListComponent,
      UserSurveyListComponent,
      NavBarComponent,
      UserListComponent,
      AddUserComponent,
      EditUserComponent,
      CreateSurveyComponent,
      EditSurveyComponent,
      UserTakeSurveyComponent,
      ViewResultsComponent
    ],
  imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BrowserAnimationsModule,
      MatTabsModule,
      MatFormFieldModule,
      MatCheckboxModule,
      MatInputModule,
      MatRadioModule,
      RouterModule.forRoot(appRoutes),
      ChartsModule,
      WavesModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
