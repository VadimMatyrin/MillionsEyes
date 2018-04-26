import { Injectable } from "@angular/core"
import { Http } from "@angular/http";
import { Metric } from "./metric.model";
import { Observable } from "rxjs/Observable";
import * as moment from 'moment';

import "rxjs/add/operator/map"
import { Point } from "./point.model";
import { environment } from "../../environments/environment";

@Injectable()
export class MetricsService
{
    constructor(private http: Http)
    { }

        get() : Observable<Array<Metric>>{
        return  this.http.get(environment.apiUrl + "/1/1").map(response =>
        {
            return this.formMetrics(response);
        });   
    }

    getForLastHours(hoursCount: number, interval: number): Observable<Array<Metric>>{
        return this.http.get(environment.apiUrl + "/" + hoursCount + "/" + interval).map(response =>
            {
                return this.formMetrics(response);
            });   
    }

    getForTimeInterval(date1: Date, date2:Date, interval:number){
        let date1String = moment(date1.toString()).format("YYYY-MM-DDTHH:MM:SS") + "Z";
        let date2String = moment(date2.toString()).format("YYYY-MM-DDTHH:MM:SS") + "Z";

        return this.http.get(environment.apiUrl + "/timespan/" + date1String + "/" + date2String + "/" + interval).map(response =>
            {
                return this.formMetrics(response);
            }); 
    }

    formMetrics(response){
        let metrics: Array<Metric> = new Array<Metric>(2);

        let data = response.json();

        for (let i = 0; i < data.length; i++)
        {
            let metric: Metric = {metricName: "", points: new Array<Point>()};

            metric.metricName = data[i].MetricName;

            for (let j = 0; j < data[i].Points.length; j++)
            {
                metric.points.push({date: new Date(data[i].Points[j].Time), count: data[i].Points[j].Count});
            }

            metrics[i] = metric;
        }

        return metrics;
    }
}