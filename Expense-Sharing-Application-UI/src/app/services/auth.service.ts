import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7141/api/Auth';
  private readonly USER_ID_KEY = 'user_id';
  private readonly USER_ROLE_KEY = 'user_role';
  public userId: number =0;
  private userRole: string | null = null;

  constructor(private http: HttpClient) {
    const storedUserId = localStorage.getItem(this.USER_ID_KEY);
    const storedUserRole = localStorage.getItem(this.USER_ROLE_KEY);

    if (storedUserId) {
      this.userId = parseInt(storedUserId, 10);
    }
    if (storedUserRole) {
      this.userRole = storedUserRole;
    }
  }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { email, password }).pipe(
      map(response => {
        if (response && response.token) {
          localStorage.setItem('token', response.token);
          const decodedToken = this.decodeToken(response.token);
          const userId = decodedToken.sub;
          const userRole = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']; 
          this.setUserId(userId);
          this.setUserRole(userRole);
        }
        return response;
      }),
      catchError(error => {
        console.error('Login error:', error);
        return of(error);
      })
    );
  }

  logout(): void {
    this.clearUserId();
    this.clearUserRole();
    localStorage.removeItem('token');
  }

  setUserId(userId: number): void {
    this.userId = userId;
    localStorage.setItem(this.USER_ID_KEY, userId.toString());
  }

  getUserId(): number {
    return this.userId;
  }

  setUserRole(userRole: string): void {
    this.userRole = userRole;
    localStorage.setItem(this.USER_ROLE_KEY, userRole);
  }

  getUserRole(): string | null {
    return this.userRole;
  }

  clearUserRole(): void {
    this.userRole = null;
    localStorage.removeItem(this.USER_ROLE_KEY);
  }

  private decodeToken(token: string): any {
    const payload = token.split('.')[1];
    return JSON.parse(atob(payload));
  }
  clearUserId(): void {
    this.userId = 0;
    localStorage.removeItem(this.USER_ID_KEY);
  }
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }
}
