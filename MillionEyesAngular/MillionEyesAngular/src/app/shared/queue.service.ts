import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Metric } from './metric.model';
import { Observable } from 'rxjs/Observable';
import * as moment from 'moment';

import 'rxjs/add/operator/map';
import { Point } from './point.model';
import { environment } from '../../environments/environment';

@Injectable()
export class QueueService {
    constructor(private http: Http) { }

    get(): Observable<Array<Metric>> {
        return this.http.get(environment.queueUrl + 'getmetricsforhours?hour=' + 1 + '&interval=' + 1).map(response => {
            return this.formQueuesMetrics(response);
        });
    }

    getForLastHours(hoursCount: number, interval: number): Observable<Array<Metric>> {
        return this.http.get(environment.queueUrl + 'getmetricsforhours?hour=' + hoursCount + '&interval=' + interval).map(response => {
                return this.formQueuesMetrics(response);
            });
    }

    getForTimeInterval(date1: Date, date2: Date, interval: number) {

        let date1String = moment(date1.toString()).format('YYYY-MM-DDTHH:MM:SS') + 'Z';
        let date2String = moment(date2.toString()).format('YYYY-MM-DDTHH:MM:SS') + 'Z';

        return this.http.get(environment.queueUrl + 'getmetricsforperiod?startTime=' + date1String + '&endTime=' + date2String + '&interval=' + interval).map(response => {
                return this.formQueuesMetrics(response);
            });
    }

    formQueuesMetrics(response) {
        let queuesMetrics: Array<Metric> = new Array<Metric>();

        let queuesData = response.json();

        for (let i = 0; i < queuesData.QueueMetrics.length; i++)
        {
            let metric: Metric = {metricName: "", points: new Array<Point>()};

            metric.metricName = queuesData.QueueMetrics[i].QueueName;

            for (let j = 0; j < queuesData.QueueMetrics[i].QueueMetrics.length; j++)
            {
                metric.points.push({date: new Date(queuesData.QueueMetrics[i].QueueMetrics[j].Time), count: queuesData.QueueMetrics[i].QueueMetrics[j].Count});
            }

            console.log(metric);

            queuesMetrics.push(metric);
        }

        return queuesMetrics;
    }
}
