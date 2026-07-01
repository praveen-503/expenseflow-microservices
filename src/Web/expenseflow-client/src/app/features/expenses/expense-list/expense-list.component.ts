import { Component, inject, signal, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Expense } from '../../../core/models/expense.model';
import { ExpenseService } from '../../../core/services/expense.service';
import { ExpenseFormComponent } from '../expense-form/expense-form.component';

@Component({
  selector: 'app-expense-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatPaginatorModule,
    MatSortModule,
    MatInputModule,
    MatFormFieldModule,
    ExpenseFormComponent
  ],
  templateUrl: './expense-list.component.html',
  styleUrl: './expense-list.component.css'
})
export class ExpenseListComponent implements OnInit, AfterViewInit {
  private readonly expenseService = inject(ExpenseService);
  private readonly router = inject(Router);

  protected readonly dataSource = new MatTableDataSource<Expense>([]);
  protected readonly displayedColumns = ['title', 'category', 'amount', 'expenseDate', 'actions'];
  protected readonly showForm = signal(false);
  protected readonly errorMessage = signal('');

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    this.loadExpenses();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    
    // Custom filter predicate to scan category name as well as title
    this.dataSource.filterPredicate = (data: Expense, filter: string) => {
      const search = filter.trim().toLowerCase();
      const matchTitle = data.title.toLowerCase().includes(search);
      const matchCategory = data.category?.name.toLowerCase().includes(search) || false;
      const matchNotes = data.notes?.toLowerCase().includes(search) || false;
      return matchTitle || matchCategory || matchNotes;
    };
  }

  loadExpenses() {
    this.expenseService.getExpenses().subscribe({
      next: (data) => {
        this.dataSource.data = data;
      },
      error: () => this.errorMessage.set('Could not load expenses.')
    });
  }

  addExpense(newExpense: Omit<Expense, 'id' | 'userId'>) {
    this.expenseService.createExpense(newExpense).subscribe({
      next: () => {
        this.loadExpenses();
        this.showForm.set(false);
      },
      error: () => this.errorMessage.set('Failed to save expense.')
    });
  }

  deleteExpense(id: string, event: Event) {
    event.stopPropagation(); // Avoid triggering route details navigation
    if (confirm('Are you sure you want to delete this expense?')) {
      this.expenseService.deleteExpense(id).subscribe({
        next: () => this.loadExpenses(),
        error: () => this.errorMessage.set('Failed to delete expense.')
      });
    }
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  navigateToDetails(id: string) {
    this.router.navigate(['/expenses', id]);
  }
}
