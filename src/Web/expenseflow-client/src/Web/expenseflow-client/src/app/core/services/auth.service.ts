import { Injectable, signal, computed } from '@angular/core';
import { User } from '../models/user.model';
import { Observable, of } from 'rxjs';
import { delay, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
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

  login(email: string, password: string): Observable<User> {
    // Mock authentication
    if (email.startsWith('admin')) {
      const adminUser: User = {
        id: 'd4c6d678-0df2-46cc-950c-db26c4599c2e',
        email: 'admin@expenseflow.com',
        firstName: 'System',
        lastName: 'Admin',
        role: 'Administrator',
        token: 'mock-jwt-token-administrator'
      };
      return of(adminUser).pipe(
        delay(1000),
        tap(user => this.saveSession(user))
      );
    } else {
      const user: User = {
        id: 'e5b72cb6-5c58-45a8-ba42-e01d1c81729b',
        email: email,
        firstName: 'John',
        lastName: 'Doe',
        role: 'User',
        token: 'mock-jwt-token-user'
      };
      return of(user).pipe(
        delay(1000),
        tap(u => this.saveSession(u))
      );
    }
  }

  logout(): void {
    localStorage.removeItem('ef_user');
    this._currentUser.set(null);
  }

  register(email: string, firstName: string, lastName: string): Observable<User> {
    const newUser: User = {
      id: crypto.randomUUID(),
      email,
      firstName,
      lastName,
      role: 'User',
      token: 'mock-jwt-token-user'
    };
    return of(newUser).pipe(
      delay(1000),
      tap(user => this.saveSession(user))
    );
  }

  private saveSession(user: User): void {
    localStorage.setItem('ef_user', JSON.stringify(user));
    this._currentUser.set(user);
  }
}
