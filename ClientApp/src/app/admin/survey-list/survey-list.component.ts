import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../auth.services';
import { QuestionInterface } from '../../interfaces/questionInterface';

@Component({
  selector: 'app-survey-list',
  templateUrl: './survey-list.component.html',
  styleUrls: ['./survey-list.component.scss']
})
export class SurveyListComponent implements OnInit {
  surveys: any;
  string_surveys: any;
  survey_response: any

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService) { }

  ngOnInit() {

    setTimeout(() => {
      this.Getall();
      this.authService.Refreshrequired.subscribe(response => {
        this.Getall();
      });
    },
      500);
    
    
  }

  Getall() {

    this.authService.getUserSurveys().subscribe(result => {

      this.string_surveys = JSON.stringify(result);

      this.survey_response = JSON.parse(this.string_surveys);



      this.surveys = [this.survey_response[0]];



      for (let i = 0; i < this.survey_response.length; i++) {



        if (this.surveys[this.surveys.length - 1].surveyId != this.survey_response[i].surveyId) {

          this.surveys.push(this.survey_response[i]);

        }


      }

    })

  }
  

  editSurvey(survey: any) {

    var surveyId = Object(survey)["surveyId"];


    this.authService.getSurvey(surveyId).subscribe(result => {
      console.log(result),
        this.authService.passSurvey(result)

    });
    setTimeout(() => {
      console.log("delay for 1");
      this.router.navigate(['admin/editSurvey'])
    },
      1000);

  }
  viewResults(surveyId: any) {
    localStorage.setItem("surveyId",surveyId)
    this.router.navigate(['admin/viewResults']);
  }

  createSurvey() {
    this.router.navigate(['admin/createSurvey']);
  }

  Delete(survey: any) {
    var surveyId = Object(survey)["surveyId"];
    this.authService.deleteSurvey(surveyId).subscribe(
      (response) => {

        console.log(response);

      },

      (error) => {
        console.error(error.status);

      }
    );

  }
  backButton() {
    this.router.navigate(['admin']);
  }
  

}
