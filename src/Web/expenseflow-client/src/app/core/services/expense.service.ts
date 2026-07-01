import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Expense, Category, ExpenseSummary } from '../models/expense.model';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5062/api/v1';

  // --- Expenses CRUD ---
  getSummary(): Observable<ExpenseSummary> {
    return this.http.get<ExpenseSummary>(`${this.apiUrl}/expenses/summary`);
  }

  getExpenses(): Observable<Expense[]> {
    return this.http.get<Expense[]>(`${this.apiUrl}/expenses`);
  }

  getExpenseById(id: string): Observable<Expense> {
    return this.http.get<Expense>(`${this.apiUrl}/expenses/${id}`);
  }

  createExpense(expense: Omit<Expense, 'id' | 'userId'>): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/expenses`, expense);
  }

  updateExpense(id: string, expense: Omit<Expense, 'id' | 'userId'>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/expenses/${id}`, expense);
  }

  deleteExpense(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/expenses/${id}`);
  }

  // --- Categories CRUD ---
  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/categories`);
  }

  createCategory(category: Omit<Category, 'id'>): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/categories`, category);
  }

  updateCategory(id: string, category: Omit<Category, 'id'>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/categories/${id}`, category);
  }

  deleteCategory(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/categories/${id}`);
  }
}
