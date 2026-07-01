import { Injectable, signal, computed, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';
import { Observable, throwError, of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';

interface AuthResponse {
  token: string;
  refreshToken: string;
  tokenExpiration: string;
  user: {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
  };
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5233/api/v1';

  private readonly _currentUser = signal<User | null>(null);
  
  public readonly currentUser = this._currentUser.asReadonly();
  public readonly isAuthenticated = computed(() => this._currentUser() !== null);
  public readonly isAdmin = computed(() => this._currentUser()?.role === 'Administrator');

  constructor() {
    const saved = localStorage.getItem('ef_user');
    if (saved) {
      try {
        this._currentUser.set(JSON.parse(saved));
      } catch {
        localStorage.removeItem('ef_user');
      }
    }
  }

  login(email: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, { email, password }).pipe(
      tap(res => {
        const role = this.getRoleFromToken(res.token);
        const user: User = {
          id: res.user.id,
          email: res.user.email,
          firstName: res.user.firstName,
          lastName: res.user.lastName,
          role: role,
          token: res.token,
          refreshToken: res.refreshToken
        };
        this.saveSession(user);
      })
    );
  }

  register(email: string, firstName: string, lastName: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/register`, { email, firstName, lastName, password }).pipe(
      tap(res => {
        const role = this.getRoleFromToken(res.token);
        const user: User = {
          id: res.user.id,
          email: res.user.email,
          firstName: res.user.firstName,
          lastName: res.user.lastName,
          role: role,
          token: res.token,
          refreshToken: res.refreshToken
        };
        this.saveSession(user);
      })
    );
  }

  refreshToken(): Observable<AuthResponse> {
    const user = this._currentUser();
    if (!user || !user.refreshToken || !user.token) {
      return throwError(() => new Error('No refresh token available'));
    }
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/refresh`, {
      token: user.token,
      refreshToken: user.refreshToken
    }).pipe(
      tap(res => {
        const role = this.getRoleFromToken(res.token);
        const updatedUser: User = {
          id: res.user.id,
          email: res.user.email,
          firstName: res.user.firstName,
          lastName: res.user.lastName,
          role: role,
          token: res.token,
          refreshToken: res.refreshToken
        };
        this.saveSession(updatedUser);
      }),
      catchError(err => {
        this.logout();
        return throwError(() => err);
      })
    );
  }

  logout(): void {
    const user = this._currentUser();
    if (user && user.refreshToken) {
      this.http.post(`${this.apiUrl}/auth/logout`, { refreshToken: user.refreshToken })
        .subscribe({
          error: (err) => console.error('Error logging out from server', err)
        });
    }
    localStorage.removeItem('ef_user');
    this._currentUser.set(null);
  }

  private saveSession(user: User): void {
    localStorage.setItem('ef_user', JSON.stringify(user));
    this._currentUser.set(user);
  }

  private getRoleFromToken(token: string): 'Administrator' | 'User' {
    try {
      const payloadBase64 = token.split('.')[1];
      const payloadJson = atob(payloadBase64);
      const payload = JSON.parse(payloadJson);
      
      const roleClaim = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      if (roleClaim === 'Administrator' || (Array.isArray(roleClaim) && roleClaim.includes('Administrator'))) {
        return 'Administrator';
      }
    } catch (e) {
      console.error('Error decoding JWT token role', e);
    }
    return 'User';
  }
}
