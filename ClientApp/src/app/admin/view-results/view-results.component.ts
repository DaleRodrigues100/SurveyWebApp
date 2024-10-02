import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { QuestionInterface } from 'src/app/interfaces/questionInterface';
import { AuthService } from '../../auth.services';

@Component({
  selector: 'app-view-results',
  templateUrl: './view-results.component.html',
  styleUrls: ['./view-results.component.scss']
})
export class ViewResultsComponent implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router, private authService: AuthService) { }
  survey: any;
  surveyId: any;
  surveys: any;
  response_string: any;
  response: any;
  temp: any;
  add_user: any;
  displayChart = false;
  question_index: any;
  chart_type = "bar";
  toggle_view = true;
  toggle_table = true;


  question_title: any;

  ngOnInit() {
    this.surveyId = localStorage.getItem("surveyId");
    this.GetSurveyResponses();
    this.authService.Refreshrequired.subscribe(() => {
      this.GetSurveyResponses();
      this.setUpCharts();
    });
    this.getQuestionOptions();

    this.question_title
    // this.setUpCharts();
  }

  showCharts(index: number){
    this.displayChart = true;
    this.question_index = index;
    this.toggle_view = false;
  }

  closeCharts(){
    this.displayChart = false;
    this.toggle_view = true;
  }

  toggleChart(){
    if (this.chart_type === "bar"){
      this.chart_type = "pie";
    }
    else{
      this.chart_type = "bar";
    }
  }


  hideTable(){
    this.toggle_table = !this.toggle_table
  }

  GetSurveyResponses() {
    //@ts-ignore
    this.authService.getResponse(localStorage.getItem("surveyId")).subscribe(result => {
      this.response_string = JSON.stringify(result);
      this.response = JSON.parse(this.response_string);
      this.temp = []
      console.log(this.response);
      for(let i = 0; i <this.response.length; i++) {
        //@ts-ignore
        this.authService.getResponseValues(localStorage.getItem("surveyId"), this.response[i]["optionId"], this.response[i]["questionId"]).subscribe(result => {
          this.survey = JSON.stringify(result);
          this.add_user = JSON.parse(this.survey);
          this.add_user[0]["username"] = this.response[i]["username"];
          this.temp.push(this.add_user[0]);
          console.log(this.temp);
        });
      }
      this.surveys = this.temp;
      console.log(this.surveys);
    })
  }

  backButton() {
    this.router.navigate(['admin/surveys']);
  }

  chartDatasets = [[{ data: [65, 59, 80, 81, 56, 55, 40], label: 'My First dataset' }]];
  option_res: any[] = [];
  option_res_string: any;
  res: any;
  res_string: any;
  store_survey: any; // store_survey size is total number of options
  chartLabels : any[] = []; //chartlabels size is total number of questions, chartLabels[QuestionNumber-1] is the number of options for each question

  getQuestionOptions(){
    this.authService.getSurvey(localStorage.getItem("surveyId")).subscribe(result => {
      this.store_survey = result;
      var temp = this.store_survey[0]['questionId'];
      var store_options = [];
      for (let i = 0; i <this.store_survey.length; i++) {
        if (temp === this.store_survey[i]['questionId']){
          store_options.push(this.store_survey[i]['optionList']);
        }
        else{
          this.chartLabels.push(store_options);
          store_options = [this.store_survey[i]['optionList']];
          temp = this.store_survey[i]['questionId'];
        }
      }
      this.chartLabels.push(store_options);
      this.setUpCharts();
      this.chartDatasets.pop();
    });
  }


  setUpCharts(){
    for (let i = 0; i < this.chartLabels.length; i++) {
      this.chartDatasets.push([{ data: [65, 59, 80, 81, 56, 55, 40], label: 'My First dataset' }]);
      //@ts-ignore
      this.authService.getOptionResponse(localStorage.getItem("surveyId"), i+1).subscribe(result => {
        let store_data: number[] = [];
        this.option_res_string =  JSON.stringify(result);
        this.option_res =  JSON.parse(this.option_res_string);
        var question_options = this.chartLabels[i];
        // iterate through each option label, call api to see if theres any associated data where x = optionId
        for (let x = 0; x < question_options.length; x++){
          var notFound = true;
          // check for responses
          for (let y = 0; y < Object.keys(this.option_res).length; y++){
            var optionId = this.option_res[y]['optionId'];
            if (optionId === x+1){
              store_data.push(this.option_res[y]['surveyId']);
              notFound = false;
            }
          }
          if (notFound){
            store_data.push(0);
          }
        }
        let empty_array: number[] = []
        let data = [{ data: empty_array, label: 'Response'}];
        for (let y = 0; y < store_data.length; y++){
          data[0].data.push(store_data[y])
        }
        this.chartDatasets[i] = data;
      })
    }
    console.log(this.chartDatasets);
    }

  chartType = 'pie';
  chartColors = [
    {
      backgroundColor: [
        'rgba(255, 99, 132, 0.2)',
        'rgba(54, 162, 235, 0.2)',
        'rgba(255, 206, 86, 0.2)',
        'rgba(75, 192, 192, 0.2)',
        'rgba(153, 102, 255, 0.2)',
        'rgba(255, 159, 64, 0.2)'
      ],
      borderColor: [
        'rgba(255,99,132,1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(75, 192, 192, 1)',
        'rgba(153, 102, 255, 1)',
        'rgba(255, 159, 64, 1)'
      ],
      borderWidth: 2,
    }
  ];
  chartOptions: any = {
    responsive: true
  };

  chartClicked(event: any): void {
    console.log(event);
  }

  chartHovered(event: any): void {
    console.log(event);
  }
}


