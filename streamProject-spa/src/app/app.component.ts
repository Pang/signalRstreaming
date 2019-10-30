import { Component, OnInit } from '@angular/core';

import { Chart } from 'chart.js';
import { HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  data = [];
  connection = new HubConnectionBuilder().withUrl('http://localhost:5000/streamHub').build();
  lineChart = [];
  ngOnInit() {
    this.connection.start().then(() => console.log("Connection Started"))
      .catch(err => console.log("error: " + err));

      this.connection.on("Itemz", data => {
        console.log("test1");
        this.initialiseData(this.lineChart, data);
      });
      setTimeout(() => {
        this.streamData();
      }, 1000);


    this.lineChart = new Chart('myChart', {
      type: 'line',
      data: {
        labels: this.data,
        datasets: [{
          label: "test",
          data: this.data,
          backgroundColor: 'rgba(54, 162, 235, 0.2)',
          borderColor: 'gray'
        }]
      },
      options: {
        scales: {
          yAxes: [{
            ticks: {
              beginAtZero: true
            }
          }]
        }
      }
    })
  }

  initialiseData(chart, data) {
    // chart.data.datasets.forEach((dataset) => {
    //   data.forEach(x => {
    //     dataset.data.shift(x);
    //   });
    // });

    chart.data.datasets.forEach((dataset) => {
      data.forEach(x => {
        dataset.data.push(x);
      });
    });
    chart.update();
  }

  addData(chart, data) {
    chart.data.datasets.forEach((dataset) => {
        dataset.data.shift();
    });

    chart.data.datasets.forEach((dataset) => {
        dataset.data.push(data);
    });
    chart.update();
  }

  streamData() {
    this.connection.on("Item", data => {
      console.log("test2");
      this.addData(this.lineChart, data);
    });
  }
}
