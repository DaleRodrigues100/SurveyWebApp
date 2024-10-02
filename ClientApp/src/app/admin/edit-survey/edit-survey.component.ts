import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { QuestionInterface } from 'src/app/interfaces/questionInterface';
import { AuthService } from '../../auth.services';

@Component({
  selector: 'app-edit-survey',
  templateUrl: './edit-survey.component.html',
  styleUrls: ['./edit-survey.component.scss']
})
export class EditSurveyComponent implements OnInit {
  string_surveys: any
  surveyData: any
  surveys: any
  thisFormat: QuestionInterface[] = [];

  surveyIdVar: any
  baseUrl = "https://localhost:8443";
  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService, private http: HttpClient) { }

  ngOnInit() {


    var sur = this.authService.getUserSurvey();


    this.string_surveys = JSON.stringify(sur);

    this.surveyData = JSON.parse(this.string_surveys);


    var initst = this.surveyData[0].surveyTitle;

    this.surveyIdVar = this.surveyData[0].surveyId;


    const q: { [ques: string]: number } = {};



    var optionsArr = []

    for (let i = 0; i < this.surveyData.length; i++) {
      optionsArr.push(this.surveyData[i].optionList);
      q[this.surveyData[i].question] = i;
    }

    //console.log(q);
    //console.log(optionsArr);
    var counter = 0;
    for (var item in q) {
      const questionObj: QuestionInterface = {} as QuestionInterface;
      questionObj.surveyTitle = initst;
      questionObj.question = item;
      questionObj.options = optionsArr.slice(counter, q[item] + 1);
      this.thisFormat.push(questionObj);
      counter = q[item] + 1;

    }
    console.log(this.thisFormat);




  }

  selected = new FormControl(0);
  question = {} as QuestionInterface;

  questionList2: QuestionInterface[] = [{ surveyTitle: "Cat Survey", question: "What is your cat color?", options: ["Violet", "Indigo", "Blue", "Green", "Yellow", "Orange", "Red"] }
    , { surveyTitle: "Cat Survey", question: "What is your cat breed?", options: ["Siamese", "Persian", "Sphinx", "Spotted"] }];

  questionList = this.thisFormat;


  addQuestion() {
    this.questionList.push({
      surveyTitle: this.question.surveyTitle,
      question: this.question.question,
      options: ['']
    })
    this.question.question = '';
    this.question.surveyTitle = '';
    this.selected.setValue(this.questionList.length - 1);
  }

  removeTab(index: number) {
    this.questionList.splice(index, 1);
  }

  addOptionToQuestion(question: QuestionInterface) {
    question.options.push("");
  }
  trackByIdx(index: number, obj: any): any {
    //console.log(index, obj);
    return index;
  }

  removeOption(question: QuestionInterface, index: number) {
    question.options.splice(index, 1);
  }

  backButton() {
    this.router.navigate(['admin/surveys']);
  }

  Save() {
    for (var i = 0; i < this.questionList.length; i++) {
      this.questionList[i].surveyTitle = this.questionList[0].surveyTitle;
    }
    this.router.navigate(['admin/surveys']);
    console.log(this.surveyIdVar.toString());
    var q =  this.baseUrl + "/api/admin/survey/";
    q = q.concat(this.surveyIdVar.toString());
    return this.http.post(  this.baseUrl + "/api/admin/survey/createSurveySave", this.questionList).subscribe(res => {
      this.http.delete(q).subscribe();
    })
    //return this.http.delete(q).subscribe(res => {
    //  this.http.post("https://localhost:8443/api/admin/survey/createSurveySave", this.questionList).subscribe()
    //});
  }
  Publish() {
    for (var i = 0; i < this.questionList.length; i++) {
      this.questionList[i].surveyTitle = this.questionList[0].surveyTitle;
    }
    //this.authService.createSurveySave(this.questionList);
    this.router.navigate(['admin/surveys']);
    console.log(this.surveyIdVar.toString());
    var q = this.baseUrl    +  "/api/admin/survey/editPublish/";
    q = q.concat(this.surveyIdVar.toString());

    return this.http.post( this.baseUrl + "/api/admin/survey/createSurveyPublish", this.questionList).subscribe(res => {
      this.http.post(q, this.questionList).subscribe();
    })
    //return this.http.post(q, this.questionList).subscribe(res => {

    // this.http.post("https://localhost:8443/api/admin/survey/createSurveyPublish", this.questionList).subscribe()
    //});

  }

}
