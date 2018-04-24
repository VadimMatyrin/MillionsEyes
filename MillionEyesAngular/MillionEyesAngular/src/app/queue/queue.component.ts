import { Component, OnInit } from '@angular/core';
import { QueueService } from '../shared/queue.service';
import { Metric } from '../shared/metric.model';
import {IMyDrpOptions, IMyDateRange} from 'mydaterangepicker';
import {IMyDateRangeModel} from 'mydaterangepicker';
import * as moment from 'moment';
import Highcharts = require('highcharts');

declare var jQuery: any;

@Component({
  selector: 'app-queue-graph',
  templateUrl: './queue.component.html',
  styleUrls: ['./queue.component.css']
})

export class QueueComponent implements OnInit {

  [x: string]: any;
  constructor(private service: QueueService) {
    this.options = {
      chart: {
        type: 'area',
        zoomType: 'x',
        panning: true,
        panKey: 'shift',
        height: 500,
      },
      title: {
          text: 'Queues Metrics',
          marginLeft: 'auto',
          marginRight: 'auto'
      },
      subtitle: {
          text: 'MillionsEyes',
          marginLeft: 'auto',
          marginRight: 'auto',
      },
      xAxis: {
        type: 'datetime'
      },
      tooltip: {
          pointFormat: '{series.name}: {point.y}'
      },
      loading: {
        hideDuration: 1000,
        showDuration: 1000,
        labelStyle: {
          backgroundImage: 'url("https://cdn.dribbble.com/users/43718/screenshots/1137873/loadinganimation1.gif")',
          display: 'block',
          width: '450px',
          height: '300px',
          color: 'white',
          top: '10%',
          marginLeft: 'auto',
          marginRight: 'auto'
        }
      },
      plotOptions: {
        area: {
         stacking: 'normal',
           marker: {
               enabled: false,
               symbol: 'circle',
               radius: 3,
               states: {
                   hover: {
                       enabled: true
                   }
               }
           }
        }
      }
    };
  }

  metrics: Array<Metric> = [];
  // tslint:disable-next-line:no-inferrable-types
  interval: number = 1;
  hoursCount: number;
  dataRange: IMyDateRangeModel;
  options;
  series;
  chart;
  myDateRangePickerOptions: IMyDrpOptions = {
    dateFormat: 'dd.mm.yyyy',
  };
  vodafone: string;

  ngOnInit() {
    this.service.get().subscribe(m => {
        this.updateGraph(m);
      });
  }

  updateGraph(m: Array<Metric>) {
    this.metrics = m;

    this.options.series = this.formSeries(m);

    this.chart = Highcharts.chart('container', this.options);
  }

  formSeries(m: Array<Metric>) {
    // tslint:disable-next-line:prefer-const
    let series = [];

    for (let i = 0; i < m.length; i++) {
        series.push({
            name: m[i].metricName,
            data: []
        });
        for (let j = 0; j < m[i].points.length; j++) {
            series[i].data.push([m[i].points[j].date.getTime(), m[i].points[j].count]);
        }
    }

    return series;
  }

  changeLastHoursCount(hoursCount: number) {
    this.chart.showLoading();
    this.hoursCount = hoursCount;
    this.dataRange = null;

    this.service.getForLastHours(hoursCount, this.interval).subscribe(m => {
          this.updateGraph(m);
        });
  }

  changeDateRange(dataRange: IMyDateRangeModel) {
    this.chart.showLoading();

    this.hoursCount = 0;
    this.dataRange = dataRange;
    // tslint:disable-next-line:prefer-const
    let date1: Date = dataRange.beginJsDate;
    // tslint:disable-next-line:prefer-const
    let date2: Date = dataRange.endJsDate;

    this.service.getForTimeInterval(this.date1, this.date2, this.interval).subscribe(m => {
          this.updateGraph(m);
        });
  }

  changeInterval(interval: number) {
    this.chart.showLoading();
    interval = interval;

    if (this.hoursCount === 0) {
        this.changeDateRange(this.dataRange);
    } else {
        this.changeLastHoursCount(this.hoursCount);
    }
  }
}
