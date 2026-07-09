import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { AuthService } from '../../core/services/auth.service';
import { ExpenseService } from '../../core/services/expense.service';
import { ExpenseSummary } from '../../core/models/expense.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule, 
    MatCardModule, 
    MatIconModule, 
    MatListModule,
    MatProgressBarModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
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

  // Calculate percentage of category amount compared to total expenses
  getCategoryPercentage(categoryAmount: number): number {
    const total = this.summary()?.totalExpense || 0;
    if (total === 0) return 0;
    return (categoryAmount / total) * 100;
  }
}
