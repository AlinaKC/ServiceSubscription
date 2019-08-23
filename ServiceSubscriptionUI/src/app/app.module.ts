import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { routing } from './app-routing.module';

import { JwtInterceptor, ErrorInterceptor } from './_helpers';
import { HomeComponent } from './home';
import { AdminComponent } from './admin';
import { LoginComponent } from './login';
import {ReportComponent} from './report';
import {RegisterComponent} from './register';

import { AlertComponent } from './_directives';
import { AlertService, AuthenticationService, UserService } from './_services';
import { AuthGuard } from './_guards';

import { MustMatchDirective } from './_helpers/must-match.directive';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NotifierModule } from 'angular-notifier';

@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        FormsModule,
        HttpClientModule,
        routing,
        BrowserAnimationsModule,
        BsDatepickerModule.forRoot(),
        NotifierModule
    ],
    declarations: [
        AppComponent,
        HomeComponent,
        AdminComponent,
        LoginComponent,
        ReportComponent,
        RegisterComponent,
        MustMatchDirective,
        AlertComponent
    ],
    providers: [
        AuthGuard,
        AlertService,
        AuthenticationService,
        UserService,
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    ],
    bootstrap: [AppComponent]
})

export class AppModule { }