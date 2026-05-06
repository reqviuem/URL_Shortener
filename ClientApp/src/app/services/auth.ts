import { jwtDecode } from 'jwt-decode';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5068/api/auth';

  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasToken());
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(private http: HttpClient) { }

  private hasToken(): boolean {
    return !!localStorage.getItem('jwt_token');
  }

  login(credentials: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials).pipe(
      tap((response: any) => {
        localStorage.setItem('jwt_token', response.token);
        if (response.username) localStorage.setItem('username', response.username);

        this.isLoggedInSubject.next(true);
      })
    );
  }

  logout() {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('username');
    this.isLoggedInSubject.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem('jwt_token');
  }

  getUsername(): string | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const decoded: any = jwtDecode(token);
      return decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || decoded.unique_name || null;
    } catch {
      return null;
    }
  }

  isAdmin(): boolean {
    const token = this.getToken();
    if (!token) return false;
    try {
      const decoded: any = jwtDecode(token);
      return decoded.isAdmin === 'True' || decoded.isAdmin === true || decoded['isAdmin'] === 'True';
    } catch {
      return false;
    }
  }
}
