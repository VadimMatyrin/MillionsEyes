import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { ViewGraphComponent } from './view-graph/view-graph.component';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { MyDateRangePickerModule } from 'mydaterangepicker';
import { MetricsService } from './shared/metrics.service';


@NgModule({
  declarations: [
    AppComponent,
    ViewGraphComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,    
    FormsModule,
    HttpModule,
    MyDateRangePickerModule
  ],
  providers: [MetricsService],
  bootstrap: [AppComponent]
})
export class AppModule { }
