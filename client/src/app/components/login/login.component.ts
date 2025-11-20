import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterModule],
  template: `
  <form [formGroup]="form" (ngSubmit)="submit()">
    <input formControlName="username" placeholder="username" />
    <input formControlName="password" placeholder="password" type="password" />
    <button type="submit">Login</button>
  </form>
  `
})
export class LoginComponent {
  form: FormGroup;
  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private auth: AuthService,
    private router: Router
  ) {
    this.form = this.fb.group({ username: '', password: '' });
  }

  submit() {
    this.http.post<any>('http://localhost:5000/api/auth/login', this.form.value).subscribe({
      next: res => {
        this.auth.setAuth(res.token, res.username, res.role);
        this.router.navigate(['/']);
      },
      error: err => alert('Login failed')
    });
  }
}
