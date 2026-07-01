import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { User } from '../../core/models/user.model';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatCardModule],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent {
  protected readonly users = signal<User[]>([
    { id: '1', email: 'admin@expenseflow.com', firstName: 'System', lastName: 'Admin', role: 'Administrator' },
    { id: '2', email: 'user@expenseflow.com', firstName: 'John', lastName: 'Doe', role: 'User' }
  ]);

  protected readonly displayedColumns = ['email', 'firstName', 'lastName', 'role'];
}
