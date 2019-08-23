import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { User, Report } from '../_models';
import { UserService, AuthenticationService } from '../_services';

@Component({ templateUrl: 'report.component.html' })
export class ReportComponent implements OnInit {
    currentUser: User;
    users: User[] = [];
    dataRange: string;
    model: any = {};
    dateRangeModel: any = [];
    fromDate: Date;
    toDate: Date;
    reports: Report[] = [];
    isSubmitted = false;
    constructor(
        private userService: UserService,
        private authenticationService: AuthenticationService
    ) {
        this.currentUser = this.authenticationService.currentUserValue;
    }

    ngOnInit() {
    }

    onSubmit() {
        this.dateRangeModel = this.model.dateRange;
        this.fromDate = this.dateRangeModel[0];
        this.toDate = this.dateRangeModel[1];
        this.userService.report(this.currentUser.userId, this.fromDate, this.toDate).pipe(first()).subscribe((response: any) => {
            this.reports = response;
        });
        this.isSubmitted = true;
    }
}