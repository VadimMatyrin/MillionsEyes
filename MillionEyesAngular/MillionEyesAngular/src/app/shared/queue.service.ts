import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Metric } from './metric.model';
import { Observable } from 'rxjs/Observable';
import * as moment from 'moment';

import 'rxjs/add/operator/map';
import { Point } from './point.model';
import { environment } from '../../environments/environment';
import { encode } from 'punycode';

@Injectable()
export class QueueService {
  constructor(private http: Http) { }

  get(): Observable<Array<Metric>> {
    return this.http.get(environment.queueUrl + 'getMetricsForHours?hour=1&interval=1').map(response => {
      return this.formQueuesMetrics(response);
    });
  }

  getForLastHours(hoursCount: number, interval: number): Observable<Array<Metric>> {
    return this.http.get(environment.queueUrl + 'getMetricsForHours?hour=' + hoursCount + '&interval=' + interval).map(response => {
      return this.formQueuesMetrics(response);
    });
  }

  getForFilters(metricName: string, date1: Date, date2: Date, interval: number) {
    let date1String = date1.toUTCString();
    let date2String = date2.toUTCString();

    return this.http.get(environment.queueUrl + "getMetrics?" + (metricName === "Any" ? "" : `metricName=${metricName}`) + "&startTime=" + encodeURI(date1String) + "&endTime=" + encodeURI(date2String) + "&interval=" + interval).map(response => {
      return this.formQueuesMetrics(response);
    });
  }

  formQueuesMetrics(response) {

    let queuesMetrics: Array<Metric> = new Array<Metric>();
    let queuesData = response.json().QueueMetrics;
    let now = new Date();
    let timeZoneOffsetMillis = (now.getTimezoneOffset() * 60000); //60 secs * 1000 mills = millis per minute
    for (let i = 0; i < queuesData.length; i++) {
      let metric: Metric = { metricName: queuesData[i].QueueName, points: new Array<Point>() };
      let offset = Math.random() * 35;
      for (let j = 0; j < queuesData[i].Metrics.length; j++) {
        metric.points.push({ date: new Date(new Date(queuesData[i].Metrics[j].Time).getTime() - timeZoneOffsetMillis), count: Math.floor(Math.random() * 5) + Math.floor(40 - offset) }); //queuesData[i].Metrics[j].Count });
      }

      queuesMetrics.push(metric);
    }
    console.log(queuesMetrics);
    return queuesMetrics;
  }
}
