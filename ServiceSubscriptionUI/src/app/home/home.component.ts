import { Component } from '@angular/core';
import { first } from 'rxjs/operators';

import { User, SubscibeType } from '../_models';
import { UserService, AuthenticationService } from '../_services';

@Component({ templateUrl: 'home.component.html'})
export class HomeComponent {
    currentUser: User;
    userFromApi: User;
    isCheckedMaintenance: boolean = false;
    isCheckedInstallation: boolean = false;
    isInstallationSubscribe: boolean = false;
    isMaintenanceSubscribe: boolean = false;
    subscribeTypeFromApi: SubscibeType;
    unSubscribe: boolean = false;
    constructor(
        private userService: UserService,
        private authenticationService: AuthenticationService
    ) {
        this.currentUser = this.authenticationService.currentUserValue;
    }

    ngOnInit() {
        this.userService.getById(this.currentUser.userId).pipe(first()).subscribe(user => {
            this.userFromApi = user;
        });

        this.subscribeStatus();
    }

    subscribeStatus() {
        this.userService.hasUserSubscribed(this.currentUser.userId).pipe(first()).subscribe((response: any) => {
            this.subscribeTypeFromApi = response;
            this.isInstallationSubscribe = this.subscribeTypeFromApi.installationSubscribe;
            this.isMaintenanceSubscribe = this.subscribeTypeFromApi.maintenanceSubscribe;
        });

        this.userService.hasUserUnsubscribed(this.currentUser.userId).pipe(first()).subscribe((response: any) => {
            this.unSubscribe = response;
        });

    }

    handleInstallationSelected($event) {
        if ($event.target.checked === true) {
            this.userService.addUserSubscription(this.currentUser.userId, 1).pipe(first()).subscribe((response: any) => {
                this.subscribeTypeFromApi = response;
                this.isInstallationSubscribe = this.subscribeTypeFromApi.installationSubscribe;
                this.isMaintenanceSubscribe = this.subscribeTypeFromApi.maintenanceSubscribe;
            });
        }
    }

    handleMaintenanceSelected($event) {
        if ($event.target.checked === true) {
            this.userService.addUserSubscription(this.currentUser.userId, 2).pipe(first()).subscribe((response: any) => {
                this.subscribeTypeFromApi = response;
                this.isInstallationSubscribe = this.subscribeTypeFromApi.installationSubscribe;
                this.isMaintenanceSubscribe = this.subscribeTypeFromApi.maintenanceSubscribe;
            });
        }
    }

    unsubscribeClick(event: Event) {
        this.userService.unsubscribe(this.currentUser.userId).pipe(first()).subscribe();
        this.unSubscribe = true;
    }

    resubscribeClick(event: Event) {
        this.userService.resubscribe(this.currentUser.userId).pipe(first()).subscribe();
        this.unSubscribe = false;
    }
}