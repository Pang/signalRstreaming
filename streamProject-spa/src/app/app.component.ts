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
    setTimeout(() => {
      this.streamData();
    }, 100);

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
  addData(chart, data) {
    chart.data.datasets.forEach((dataset) => {
      dataset.data.push(data);
    });
    chart.update();
  }

  removeData(chart) {
    if (chart.data.labels.length > 25) {
      console.log(chart.data.datasets);
      chart.data.datasets.forEach((dataset) => {
        //dataset.data.shift();
        chart.data.labels.shift();
      });
      chart.update();
    }
  }

  streamData() {
    this.connection.on("Item", data => {
      console.log(data);
      this.addData(this.lineChart, data);
      this.removeData(this.lineChart);
    });
  }
}
