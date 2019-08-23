import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { environment } from '../../environments/environment';
import { User } from '../_models';

@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get<User[]>(`${environment.APIEndpoint}/users`);
    }

    getById(id: number) {
        return this.http.get<User>(`${environment.APIEndpoint}/users/${id}`);
    }

    hasUserSubscribed(id: number) {
        return this.http.get<boolean>(`${environment.APIEndpoint}/users/isSubscribe/${id}`);
    }
    addUserSubscription(id: number, subscribeType: number) {
        return this.http.post<any>(`${environment.APIEndpoint}/users/subscribe`, { id, subscribeType });
    }

    unsubscribe(id: number) {
        return this.http.post<any>(`${environment.APIEndpoint}/users/unsubscribe`, { id });
    }

    hasUserUnsubscribed(id: number) {
        return this.http.get<boolean>(`${environment.APIEndpoint}/users/isUnsubcribe/${id}`);
    }

    report(id: number, fromDate: Date, toDate: Date) {
        return this.http.post<any>(`${environment.APIEndpoint}/users/report`, { id, fromDate, toDate });
    }

    isEmailExist(userEmail: string) {
        return this.http.get<boolean>(`${environment.APIEndpoint}/users/isEmailExist/${userEmail}`);
    }

    register(user: User) {
        return this.http.post(`${environment.APIEndpoint}/users/register`, user);
    }

    resubscribe(id: number) {
        return this.http.post<any>(`${environment.APIEndpoint}/users/resubscribe`, { id });
    }
}