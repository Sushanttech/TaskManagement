import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { SignalRService } from './signalr.service';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private tasks$ = new BehaviorSubject<any[]>([]);
  tasksObservable$ = this.tasks$.asObservable();

  constructor(private http: HttpClient, private signalR: SignalRService) {
    this.signalR.startConnection();
    this.signalR.on('TaskUpdated', (payload: any) => {
      this.loadAll();
    });
    this.loadAll();
  }

  loadAll() {
    this.http.get<any[]>('http://localhost:5000/api/tasks').subscribe({
      next: data => this.tasks$.next(data),
      error: err => console.error('Failed to load tasks', err)
    });
  }

  createTask(task: any) {
    return this.http.post('http://localhost:5000/api/tasks', task);
  }

  updateTask(id: number, task: any) {
    return this.http.put(`http://localhost:5000/api/tasks/${id}`, task);
  }
}
