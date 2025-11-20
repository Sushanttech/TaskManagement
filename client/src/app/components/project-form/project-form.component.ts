import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { RouterModule, Router } from '@angular/router';

@Component({
  selector: 'app-project-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, HttpClientModule, RouterModule],
  template: `
    <h3>New Project</h3>
    <form [formGroup]="form" (ngSubmit)="submit()">
      <input formControlName="title" placeholder="title" />
      <textarea formControlName="description" placeholder="desc"></textarea>
      <button type="submit">Create</button>
    </form>
  `
})
export class ProjectFormComponent {
  form: FormGroup;
  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
    this.form = this.fb.group({ title: '', description: '' });
  }

  submit() {
    this.http.post('http://localhost:5000/api/projects', this.form.value).subscribe({
      next: () => this.router.navigate(['/']),
      error: (e) => alert('Failed to create')
    });
  }
}
