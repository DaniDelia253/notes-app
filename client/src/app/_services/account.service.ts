import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = 'http://localhost:5273/api/';

  //this observable will be equal to the user if someone is logged in or null if no one is logged in
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {}

  setCurrentUser(user: User | null) {
    this.currentUserSource.next(user);
  }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response) => {
        const user = response;
        if (user) {
          //save to browser storage
          localStorage.setItem('user', JSON.stringify(user));
          //set an observable property of this service equal to the user that is logged in
          this.setCurrentUser(user);
        }
      })
    );
  }
  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user) => {
        if (user) {
          //save to browser storage
          localStorage.setItem('user', JSON.stringify(user));
          //set an observable property of this service equal to the user that is logged in
          this.setCurrentUser(user);
        }
      })
    );
  }

  logout() {
    //clear browser storage
    localStorage.removeItem('user');
    //set observable of logged in user back to null
    this.setCurrentUser(null);
  }
}
