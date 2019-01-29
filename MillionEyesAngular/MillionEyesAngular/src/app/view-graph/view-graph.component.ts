import { Component, OnInit } from '@angular/core';
import { MetricsService } from '../shared/metrics.service';
import { Metric } from '../shared/metric.model';
import { IMyDrpOptions, IMyDateRange } from 'mydaterangepicker';
import { IMyDateRangeModel } from 'mydaterangepicker';
import * as moment from 'moment';
import Highcharts = require('highcharts');
import { QueueService } from '../shared/queue.service';
import { getLocaleDateFormat } from '@angular/common';
import { HourOption } from '../shared/hourOption';
import { environment } from "../../environments/environment.prod";
import { options } from '../config/config';

@Component({
  selector: 'app-view-graph',
  templateUrl: './view-graph.component.html',
  styleUrls: ['./view-graph.component.css']
})

export class ViewGraphComponent implements OnInit {

  constructor(private serviceBusService: MetricsService, private queuesService: QueueService) {
    this.serviceBusOptions = {
      chart: {
        type: 'area',
        zoomType: 'x',
        panning: true,
        panKey: 'shift',
        height: 500,
      },
      title: {
        text: 'Service Bus Metrics',
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
          //stacking: 'normal',
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

    this.queuesOptions = {
      chart: {
        type: 'area',
        zoomType: 'x',
       // panning: true,
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
          //stacking: 'normal',
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

    for (let button of options.buttonOptions) {
      this.hourOptions.push(new HourOption(button.text, button.hoursCount));
    }
  }

  hourOptions: Array<HourOption> = [];
  metrics: Array<Metric> = [];
  interval: number = 1;
  hoursCount: number = 1;
  set dateRange(value: IMyDateRangeModel) {
    this._dateRange = value;
    this.hoursCount = 0;
  }
  get dateRange(): IMyDateRangeModel {
    return this._dateRange;
  }
  _dateRange: IMyDateRangeModel;

  metricName: string = "Any";
  isBusLoading: boolean = true;
  isQueuesLoading: boolean = true;
  serviceBusOptions;
  queuesOptions;

  serviceBusChart;
  queuesChart;

  myDateRangePickerOptions: IMyDrpOptions = {
    dateFormat: 'dd.mm.yyyy',
  };

  ngOnInit() {

    this.serviceBusService.get().subscribe(m => {
      this.updateServiceBusGraph(m);
    });

    this.queuesService.get().subscribe(m => {
      this.updateQueuesGraph(m);
    });

  }

  updateServiceBusGraph(m: Array<Metric>) {
    this.metrics = m;

    this.serviceBusOptions.series = this.formSeries(m);

    this.serviceBusChart = Highcharts.chart('container', this.serviceBusOptions);

    this.stopShowingBusLoading();
  }

  updateQueuesGraph(m: Array<Metric>) {
    this.metrics = m;

    this.queuesOptions.series = this.formSeries(m);

    this.queuesChart = Highcharts.chart('queue-container', this.queuesOptions);

    this.stopShowingQueuesLoading();
  }

  formSeries(m: Array<Metric>) {
    const series = [];

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
    this.hoursCount = hoursCount;
    this._dateRange = null;
  }

  changeInterval(interval: number) {
    this.interval = interval;
  }

  convertDateToUTC(date: Date) {
    return new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
  }

  startShowingLoading() {
    this.serviceBusChart.showLoading();
    this.queuesChart.showLoading();
    this.isBusLoading = true;
    this.isQueuesLoading = true;
  }

  stopShowingBusLoading() {
    this.isBusLoading = false;
  }

  stopShowingQueuesLoading() {
    this.isQueuesLoading = false;
  }

  applyFilters() {
    this.startShowingLoading();

    let startDate: Date;
    let endDate: Date;

    if (this._dateRange === undefined || this._dateRange === null) {
      startDate = this.convertDateToUTC(new Date());
      endDate = this.convertDateToUTC(new Date());
      startDate.setHours(startDate.getHours() - this.hoursCount);
    }
    else {
      startDate = this._dateRange.beginJsDate;
      endDate = this._dateRange.endJsDate;
    }

    this.queuesService.getForFilters(this.metricName, startDate, endDate, this.interval).subscribe(m => {
      this.updateQueuesGraph(m);
    });

    this.serviceBusService.getForFilters(this.metricName, startDate, endDate, this.interval).subscribe(m => {
      this.updateServiceBusGraph(m);
    });

  }

  changeMetricName(metricName: string) {

    this.metricName = metricName;

  }
}
