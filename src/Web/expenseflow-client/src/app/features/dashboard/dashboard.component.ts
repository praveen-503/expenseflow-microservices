import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { AuthService } from '../../core/services/auth.service';
import { ExpenseService } from '../../core/services/expense.service';
import { ExpenseSummary } from '../../core/models/expense.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatListModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  protected readonly authService = inject(AuthService);
  private readonly expenseService = inject(ExpenseService);

  protected readonly summary = signal<ExpenseSummary | null>(null);
  protected readonly errorMessage = signal<string>('');

  ngOnInit(): void {
    this.expenseService.getSummary().subscribe({
      next: (data) => this.summary.set(data),
      error: () => this.errorMessage.set('Could not load expense metrics.')
    });
  }
}
