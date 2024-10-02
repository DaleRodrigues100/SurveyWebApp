import { Component, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../auth.services';

@Component({
  selector: 'app-user-survey-list',
  templateUrl: './user-survey-list.component.html',
  styleUrls: ['./user-survey-list.component.scss']
})
export class UserSurveyListComponent implements OnInit {

  surveys: any;
  string_surveys: any;
  survey_response: any

  username: any;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {

    this.Getall();

    this.authService.Refreshrequired.subscribe(respone => {
      this.Getall();
    });

    this.username = localStorage.getItem('username');

  }

  Start(survey: any) {
    var surveyId = Object(survey)["surveyId"];


    this.authService.getSurvey(surveyId).subscribe(result => {
      console.log(result),
    this.authService.passSurvey(result)

    });
    setTimeout(() => {
      console.log("delay for 1");
      this.router.navigate(['user/takeSurvey']);
    },

      1000);



  }
  Getall() {

    this.authService.getUserSurveys().subscribe(result => {

      this.string_surveys = JSON.stringify(result);

      this.survey_response = JSON.parse(this.string_surveys);

      this.authService.checkResponse(this.username, this.survey_response[0].surveyId).subscribe(result =>{
        this.survey_response[0]["surveyStatus"] = result;
        console.log(this.survey_response[0])
      })


      this.surveys = [this.survey_response[0]];


      for (let i = 0; i < this.survey_response.length; i++) {

        if (this.surveys[this.surveys.length - 1].surveyId != this.survey_response[i].surveyId && this.username != null) {


          result = this.authService.checkResponse(this.username, this.survey_response[i].surveyId).subscribe(result =>{

            this.survey_response[i]["surveyStatus"] = result;

            console.log(this.survey_response[i])


              }


            )


          // this.survey_response[i]["surveyStatus"] = result;

          // console.log(this.survey_response[i])
          this.surveys.push(this.survey_response[i]);

          }



        }




      })


  }
}
