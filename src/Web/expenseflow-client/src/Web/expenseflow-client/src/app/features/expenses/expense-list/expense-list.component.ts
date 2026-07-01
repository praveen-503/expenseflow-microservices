import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Expense } from '../../../core/models/expense.model';
import { ExpenseFormComponent } from '../expense-form/expense-form.component';

@Component({
  selector: 'app-expense-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, ExpenseFormComponent],
  templateUrl: './expense-list.component.html',
  styleUrl: './expense-list.component.css'
})
export class ExpenseListComponent {
  protected readonly expenses = signal<Expense[]>([
    { id: '1', description: 'Grocery Purchase', amount: 84.50, date: '2026-07-01', categoryId: '1', category: { id: '1', name: 'Food & Dining', description: '' }, userId: '1' },
    { id: '2', description: 'Uber Ride', amount: 15.20, date: '2026-06-30', categoryId: '2', category: { id: '2', name: 'Transportation', description: '' }, userId: '1' },
    { id: '3', description: 'Electricity Bill', amount: 110.00, date: '2026-06-28', categoryId: '3', category: { id: '3', name: 'Utilities', description: '' }, userId: '1' }
  ]);

  protected readonly displayedColumns = ['description', 'category', 'amount', 'date', 'actions'];

  protected showForm = signal(false);

  addExpense(newExpense: Omit<Expense, 'id' | 'userId'>) {
    const expense: Expense = {
      ...newExpense,
      id: crypto.randomUUID(),
      userId: '1'
    };
    this.expenses.update(exps => [expense, ...exps]);
    this.showForm.set(false);
  }

  deleteExpense(id: string) {
    this.expenses.update(exps => exps.filter(e => e.id !== id));
  }
}
