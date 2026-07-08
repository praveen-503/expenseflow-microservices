import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Notification } from '../../../core/models/notification.model';

@Component({
  selector: 'app-notification-list',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule],
  templateUrl: './notification-list.component.html',
  styleUrl: './notification-list.component.css'
})
export class NotificationListComponent {
  protected readonly notifications = signal<Notification[]>([
    { id: '1', userId: '1', title: 'Welcome to ExpenseFlow!', message: 'Welcome John! We are thrilled to welcome you to ExpenseFlow.', type: 'Email', status: 'Sent', createdAt: '2026-07-01 10:15' },
    { id: '2', userId: '1', title: 'Expense limit alert', message: 'You have spent 90% of your Food & Dining budget.', type: 'Push', status: 'Sent', createdAt: '2026-07-01 15:30' }
  ]);
}
