import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { TaskService } from '../../services/task.service';

@Component({
    standalone: true,
    imports: [CommonModule, HttpClientModule],
    template: `
    <h2>Dashboard</h2>

    <div>
      <h3>Projects</h3>
      <button (click)="refresh()">Refresh</button>

      <!-- debug: raw response -->
      <pre style="background:#f6f6f6;padding:8px;border:1px solid #ddd">{{ apiProjectsResponse | json }}</pre>

      <ul>
        <li *ngFor="let p of projects" style="margin:8px 0; display:flex; align-items:center; gap:8px;">
          <span style="font-weight:600;">{{ p.title }}</span>
          <span style="
            margin-left:8px;
            padding:2px 8px;
            border-radius:8px;
            background:#f1f5f9;
            font-size:12px;
            color:#111827;
          ">
            {{ p.tasks?.length || 0 }} total
          </span>
        </li>
      </ul>
    </div>

    <div>
      <h3>Tasks</h3>
      <ul>
        <li *ngFor="let t of tasks">{{ t.title }} - {{ t.completed ? 'Done' : 'Pending' }}</li>
      </ul>
    </div>
  `
})
export class DashboardComponent implements OnInit {
    projects: any[] = [];
    tasks: any[] = [];
    apiProjectsResponse: any = null;

    constructor(
        private http: HttpClient,
        private taskSvc: TaskService,
        private cdr: ChangeDetectorRef
    ) { }

    ngOnInit() {
        this.loadProjects();

        // subscribe to tasks stream — guard against null
        this.taskSvc.tasksObservable$.subscribe(list => {
            console.log('tasksObservable$ emitted', list);
            this.tasks = list ?? [];
            this.cdr.detectChanges();
        });
    }

    loadProjects() {
        // Use relative URL so proxy.conf.json (ng proxy) forwards it to backend
        this.http.get<any>('/api/projects').subscribe({
            next: res => {
                console.log('projects API response:', res);
                this.apiProjectsResponse = res;

                // If API returns wrapped object like { data: [...] } handle both cases
                this.projects = Array.isArray(res) ? res : (res.data ?? res.projects ?? []);
                this.cdr.detectChanges();
            },
            error: err => {
                console.error('Error loading projects', err);
            }
        });
    }

    refresh() {
        this.loadProjects();
        this.taskSvc.loadAll(); // ensure service actually triggers the observable
    }
}
