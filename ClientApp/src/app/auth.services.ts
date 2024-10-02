import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Users } from 'src/app/models/user';
import { Observable, Subject } from 'rxjs';
import {tap} from 'rxjs/operators';
import { Response } from './models/response';
import { QuestionInterface } from 'src/app/interfaces/questionInterface';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    userSurvey: any
    baseUrl = "https://localhost:8443";

    constructor(private http: HttpClient) { }

    private _refreshrequired = new Subject<void>();

    get Refreshrequired() {
      return this._refreshrequired;
    }

    authUser(user: Users) {
        return this.http.post(this.baseUrl + '/api/account/login', user);
    }


    addUser(user: Users) {
        return this.http.post(this.baseUrl + '/api/admin', user);
  }

  getSurvey(num: any) {

    var s = this.baseUrl + '/api/admin/survey/';
    var s = s.concat(num.toString());
    return this.http.get(s);
  }
  deleteSurvey(num: any) {
    console.log(num);
    var s = this.baseUrl + '/api/admin/survey/';
    var s = s.concat(num.toString());
    return this.http.delete(s).pipe(
      tap(() => {
        this.Refreshrequired.next();

      })
    );
  }
  //createSurveyPublish(sur: any) {
  //  return this.http.post("https://localhost:8443/api/admin/survey/createSurveyPublish", sur);
  //}
  //createSurveySave(sur: any) {
  //  return this.http.post("https://localhost:8443/api/admin/survey/createSurveySave",sur);
  //}
  passSurvey(survey: any) {
    this.userSurvey = survey;
  }
  getUserSurvey() {
    return this.userSurvey;
  }

    getUsers(): Observable<object> {
      return this.http.get(this.baseUrl + '/api/admin');
  }
  getUserSurveys(): Observable<object> {
    return this.http.get(this.baseUrl + "/api/admin/survey");
  }

    deleteUser(user: string) {
      return this.http.delete(this.baseUrl + '/api/admin/' + user).pipe(
        tap(() => {
          this.Refreshrequired.next();

        })
      );
  }

      editUser(user: Users) {
        return this.http.put(this.baseUrl + '/api/admin/edit', user);
  }

  addResponse(response: Response){

    return this.http.post(this.baseUrl + '/api/user', response);

  }

  checkResponse(user: string, surveyid: number){

    return this.http.get(this.baseUrl + '/api/user/' + user + '/' + surveyid);

  }

  getResponse(surveyId: number) {

    return this.http.get(this.baseUrl + '/api/admin/results/' + surveyId );

  }

  getResponseValues(surveyId: number, optionId: number, questionId: number) {
    return this.http.get(this.baseUrl + '/api/admin/results/' + surveyId + '/' + optionId + '/' + questionId);
  }


  getOptionResponse(surveyId: number, questionId: number) {

    return this.http.get(this.baseUrl + '/api/admin/results/charts/' + surveyId + '/' + questionId);

  }

  saveSurvey(questionList: QuestionInterface[]){

    return this.http.post(this.baseUrl + '/api/admin/survey/createSurveySave', questionList);

  }

  publishSurvey(questionList: QuestionInterface[]){

    return this.http.post(this.baseUrl + '/api/admin/survey/createSurveyPublish', questionList);


  }




}

