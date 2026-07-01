import { Component, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { Category, Expense } from '../../../core/models/expense.model';

@Component({
  selector: 'app-expense-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatButtonModule],
  templateUrl: './expense-form.component.html',
  styleUrl: './expense-form.component.css'
})
export class ExpenseFormComponent {
  @Output() save = new EventEmitter<Omit<Expense, 'id' | 'userId'>>();
  @Output() cancel = new EventEmitter<void>();

  protected readonly categories: Category[] = [
    { id: '1', name: 'Food & Dining', description: '' },
    { id: '2', name: 'Transportation', description: '' },
    { id: '3', name: 'Utilities', description: '' },
    { id: '4', name: 'Entertainment', description: '' },
    { id: '5', name: 'Housing', description: '' }
  ];

  protected readonly expenseForm: FormGroup;

  constructor(fb: FormBuilder) {
    this.expenseForm = fb.group({
      description: ['', [Validators.required]],
      amount: [null, [Validators.required, Validators.min(0.01)]],
      date: [new Date().toISOString().substring(0, 10), [Validators.required]],
      categoryId: ['', [Validators.required]]
    });
  }

  onSubmit() {
    if (this.expenseForm.invalid) return;

    const { description, amount, date, categoryId } = this.expenseForm.value;
    const category = this.categories.find(c => c.id === categoryId);

    this.save.emit({
      description,
      amount,
      date,
      categoryId,
      category
    });
  }
}
