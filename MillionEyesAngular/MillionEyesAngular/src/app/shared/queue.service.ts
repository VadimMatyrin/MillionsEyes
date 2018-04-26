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
        return this.http.get(environment.queueUrl).map(response => {
            return this.formMetrics(response);
        });
    }

    getForLastHours(hoursCount: number, interval: number): Observable<Array<Metric>> {
        return this.http.get(environment.queueUrl + '/' + hoursCount + '/' + interval).map(response => {
                return this.formMetrics(response);
            });
    }

    getForTimeInterval(date1: Date, date2: Date, interval: number) {
        // tslint:disable-next-line:prefer-const
        let date1String = moment(date1.toString()).format('YYYY-MM-DDTHH:MM:SS') + 'Z';
        // tslint:disable-next-line:prefer-const
        let date2String = moment(date2.toString()).format('YYYY-MM-DDTHH:MM:SS') + 'Z';

        return this.http.get(environment.queueUrl + '/timespan/' + date1String + '/' + date2String + '/' + interval).map(response => {
                return this.formMetrics(response);
            });
    }

    formMetrics(response) {
        // tslint:disable-next-line:prefer-const
        let metrics: Array<Metric> = new Array<Metric>(1);

        // tslint:disable-next-line:prefer-const
        let data = response.json();
        // tslint:disable-next-line:prefer-const
        let metric: Metric = {metricName: '', points: new Array<Point>()};
        metric.metricName = data.MetricName;
        for (let i = 0; i < metrics.length; i++) {
            for (let j = 0; j < data.QueueMetrics.length; j++) {
                metric.points.push({date: new Date(data.QueueMetrics[j].Time), count: data.QueueMetrics[j].Count});
            }
            metrics[i] = metric;
        }

        return metrics;
    }
}
