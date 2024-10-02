import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { QuestionInterface } from 'src/app/interfaces/questionInterface';
import { AuthService } from '../../auth.services';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Response } from '../../models/response';

@Component({
  selector: 'app-user-take-survey',
  templateUrl: './user-take-survey.component.html',
  styleUrls: ['./user-take-survey.component.scss']
})
export class UserTakeSurveyComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router) { }


  string_surveys: any
  surveyData: any
  surveys: any
  thisFormat: QuestionInterface[] = [];
  question = {} as QuestionInterface;


  surveyform = new FormGroup({});
  username: any;

  mandatory = false;


  ngOnInit() {


    this.surveyform.reset();


    var sur = this.authService.getUserSurvey();

    this.string_surveys = JSON.stringify(sur);

    this.surveyData = JSON.parse(this.string_surveys);

    //this.surveys = [this.survey_response[0]];

    var initst = this.surveyData[0].surveyTitle;

    console.log(sur)


    const q: { [ques: string]: number } = {};


    var optionsArr = []

    for (let i = 0; i < this.surveyData.length; i++) {
      optionsArr.push(this.surveyData[i].optionList);
      q[this.surveyData[i].question] = i;
    }

    // console.log(q);
    // console.log(optionsArr);
    var counter = 0;

    for(var item in q){
      const questionObj: QuestionInterface = {} as QuestionInterface;
      questionObj.surveyTitle = initst;
      questionObj.question = item;
      questionObj.options = optionsArr.slice(counter, q[item] + 1);
      this.thisFormat.push(questionObj);
      counter = q[item] + 1;

    }

    console.log(this.thisFormat);

    for (let i = 0; i < this.questionList.length; i++) {
      this.surveyform.addControl(`${i}`, new FormControl('', Validators.required))
    }


  }

  selected_tab = new FormControl(0);

  //question = {} as QuestionInterface;
  //questionList: QuestionInterface[] =[{surveyTitle: "Color", question:"What is your favourite color?", options: ["Violet","Indigo","Blue","Green","Yellow","Orange","Red"]}
  //                                  , {surveyTitle: "Color", question:"What is your favourite brand?", options: ["Meta","Apple","Amazon","Microsoft","Google"]}];

  questionList = this.thisFormat;

  trackByIdx(index: number, obj: any): any {
    return index;
  }

  setValue(){

    // this.surveyform.get(ques).value


  }

  Back() {
    this.router.navigate(['user/surveys']);
  }

  Submit() {
    console.log(this.thisFormat)

    this.username = localStorage.getItem('username');


    if (this.username != null){

      for (let i = 0; i < this.thisFormat.length; i++){

      console.log(this.surveyform.value[i])


        if (this.surveyform.value[i] === ''){
          this.mandatory = true;
          return;
        }
      }



      for (let i = 0; i < this.thisFormat.length; i++) {



        var formValue: Response ={ Username: this.username, SurveyId: this.surveyData[0].surveyId, QuestionId: i+1, OptionId: this.surveyform.value[i]+1};

        console.log(formValue);

        this.authService.addResponse(formValue).subscribe(
          (response) => {
            console.log(response);
            this.mandatory = false;
          },

          (error) => {
            console.error(error.status);
            this.mandatory = true;
            return

          }
        );

        }





      }


    setTimeout(() => {
      console.log("delay for 1");
      this.router.navigate(['user/surveys']);
    },

      1000);

      



    }







  }
