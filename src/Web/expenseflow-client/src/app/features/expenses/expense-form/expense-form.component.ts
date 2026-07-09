import { Component, Output, EventEmitter, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { Category, Expense } from '../../../core/models/expense.model';
import { ExpenseService } from '../../../core/services/expense.service';

@Component({
  selector: 'app-expense-form',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule, 
    MatCardModule, 
    MatFormFieldModule, 
    MatInputModule, 
    MatSelectModule, 
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule
  ],
  templateUrl: './expense-form.component.html',
  styleUrl: './expense-form.component.scss'
})
export class ExpenseFormComponent implements OnInit {
  private readonly expenseService = inject(ExpenseService);
  private readonly fb = inject(FormBuilder);

  @Output() save = new EventEmitter<Omit<Expense, 'id' | 'userId'>>();
  @Output() cancel = new EventEmitter<void>();

  protected readonly categories = signal<Category[]>([]);
  protected readonly expenseForm: FormGroup = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(150)]],
    amount: [null, [Validators.required, Validators.min(0.01)]],
    expenseDate: [new Date(), [Validators.required]],
    notes: ['', [Validators.maxLength(500)]],
    categoryId: ['', [Validators.required]]
  });

  ngOnInit(): void {
    this.expenseService.getCategories().subscribe(cats => {
      this.categories.set(cats);
    });
  }

  onSubmit() {
    if (this.expenseForm.invalid) return;
    
    // Format date value correctly to string format expected by API before emitting
    const formValue = { ...this.expenseForm.value };
    if (formValue.expenseDate instanceof Date) {
      formValue.expenseDate = formValue.expenseDate.toISOString();
    }
    
    this.save.emit(formValue);
  }
}
