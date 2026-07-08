import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Expense } from '../../../core/models/expense.model';
import { ExpenseService } from '../../../core/services/expense.service';

@Component({
  selector: 'app-expense-details',
  standalone: true,
  imports: [CommonModule, RouterModule, MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './expense-details.component.html',
  styleUrl: './expense-details.component.css'
})
export class ExpenseDetailsComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly expenseService = inject(ExpenseService);

  protected readonly expense = signal<Expense | null>(null);
  protected readonly errorMessage = signal('');

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.expenseService.getExpenseById(id).subscribe({
        next: (data) => this.expense.set(data),
        error: () => this.errorMessage.set('Could not fetch expense details.')
      });
    }
  }

  goBack() {
    this.router.navigate(['/expenses']);
  }
}
