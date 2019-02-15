import { Injectable } from "@angular/core"
import { Http } from "@angular/http";
import { Metric } from "./metric.model";
import { Observable } from "rxjs/Observable";

import "rxjs/add/operator/map"
import { Point } from "./point.model";
import { environment } from "../../environments/environment";

@Injectable()
export class MetricsService {
  constructor(private http: Http) { }

  get(): Observable<Array<Metric>> {
    return this.http.get(environment.busMetricsUrl + "getBusMetricsForHours?hour=1&interval=1").map(response => {
      return this.formServiceBusMetrics(response);
    });
  }

  getForLastHours(hoursCount: number, interval: number): Observable<Array<Metric>> {
    return this.http.get(environment.busMetricsUrl + "getBusMetricsForHours?hour=" + hoursCount + "&interval=" + interval).map(response => {
      return this.formServiceBusMetrics(response);
    });
  }

  getForFilters(metricName: string, date1: Date, date2: Date, interval: number) {
    let date1String = date1.toUTCString();
    let date2String = date2.toUTCString();

    return this.http.get(environment.busMetricsUrl + "getBusMetrics?" + (metricName === "Any" ? "" : `metricName=${metricName}`) + "&startTime=" + encodeURI(date1String) + "&endTime=" + encodeURI(date2String) + "&interval=" + interval).map(response => {
      return this.formServiceBusMetrics(response);
    });
  }

  formServiceBusMetrics(response) {
    let serviceBusMetrics: Array<Metric> = new Array<Metric>();
    let serviceBusData = response.json().ServiceBusModels;

    let now = new Date();
    let timeZoneOffsetMillis = (now.getTimezoneOffset() * 60000);
    for (let i = 0; i < serviceBusData.length; i++) {
      let metric: Metric = { metricName: "", points: new Array<Point>() };

      metric.metricName = serviceBusData[i].MetricName;
      for (let j = 0; j < serviceBusData[i].Metrics.length; j++) {
        metric.points.push({ date: new Date(new Date(serviceBusData[i].Metrics[j].Time).getTime() - timeZoneOffsetMillis), count: serviceBusData[i].Metrics[j].Count });
      }

      serviceBusMetrics.push(metric);
    }
    console.log(serviceBusMetrics);
    return serviceBusMetrics;
  }
}
