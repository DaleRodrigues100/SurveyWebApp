import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionInterface } from 'src/app/interfaces/questionInterface';
import { AuthService } from '../../auth.services';

@Component({
  selector: 'app-create-survey',
  templateUrl: './create-survey.component.html',
  styleUrls: ['./create-survey.component.scss']
})
export class CreateSurveyComponent implements OnInit {

  constructor(private authService: AuthService, private route: ActivatedRoute, private router: Router, private http: HttpClient) {
  }

  ngOnInit() {

  }

  selected = new FormControl(0);
  question = {} as QuestionInterface;
  questionList: QuestionInterface[] = [
    { "surveyTitle": "", "question": "", "options": [] },

  ];

  formSubmit = false;




  addQuestion() {
    this.questionList.push({
      surveyTitle : this.question.surveyTitle,
      question : this.question.question,
      options: ['']
    })
    this.question.question = '';
    this.question.surveyTitle = '';
    this.selected.setValue(this.questionList.length - 1);
  }

  removeTab(index: number) {
    this.questionList.splice(index, 1);
  }

  addOptionToQuestion(question: QuestionInterface){
    question.options.push("");
  }
  trackByIdx(index: number, obj: any): any {

    return index;
  }

  removeOption(question: QuestionInterface, index: number){
    question.options.splice(index, 1);
  }

  Save() {
    for (var i = 0; i < this.questionList.length; i++) {
      this.questionList[i].surveyTitle = this.questionList[0].surveyTitle;
    }
    this.formSubmit = true;
    
    if (this.questionList.length ===0) {
      alert("Please input valid questions and options!")
      return 0;
    } else {
      return this.authService.saveSurvey(this.questionList).subscribe(() => {
        this.router.navigate(['admin/surveys'])

      });
    }
  }

  Publish() {
    for (var i = 0; i < this.questionList.length; i++) {
      this.questionList[i].surveyTitle = this.questionList[0].surveyTitle;
    }

    
    if (this.questionList.length === 0) {
      alert("Please input valid questions and options!")

      console.log("ERRORR");
      return 0;
    } else {
      return this.authService.publishSurvey(this.questionList).subscribe(() => {

        this.router.navigate(['admin/surveys'])

      });
    }

  }



  backButton() {

      this.router.navigate(['admin/surveys']);
  }

}
