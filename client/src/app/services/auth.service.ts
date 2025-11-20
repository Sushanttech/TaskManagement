import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

interface AuthState {
  token: string | null;
  username?: string;
  role?: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private authState$ = new BehaviorSubject<AuthState>({ token: null });

  constructor() {
    const stored = localStorage.getItem('auth');
    if (stored) this.authState$.next(JSON.parse(stored));
  }

  getAuthState(): Observable<AuthState> { return this.authState$.asObservable(); }

  setAuth(token: string, username: string, role: string) {
    const state = { token, username, role };
    localStorage.setItem('auth', JSON.stringify(state));
    this.authState$.next(state);
  }

  clearAuth() {
    localStorage.removeItem('auth');
    this.authState$.next({ token: null });
  }

  getToken(): string | null { return this.authState$.value.token; }
  isLoggedIn(): boolean { return !!this.getToken(); }
  getRole(): string | undefined { return this.authState$.value.role; }
}
