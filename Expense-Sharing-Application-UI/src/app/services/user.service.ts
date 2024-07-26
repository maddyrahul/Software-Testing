import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = "https://localhost:7141/api/Auth";
  constructor(private http: HttpClient) { }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}`);
  }

  getUser(userId: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${userId}`);
  }

  // updateUser(userId: number, user: User): Observable<any> {
  //   return this.http.put(`${this.apiUrl}/users/${userId}`, user);
  // }


  updateUser(userId: number, user: User): Observable<any> {
    const url = `${this.apiUrl}/users/${userId}`;
    return this.http.put(url, user, { responseType: 'text' });
  }

  deleteUser(userId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/users/${userId}`);
  }
}
