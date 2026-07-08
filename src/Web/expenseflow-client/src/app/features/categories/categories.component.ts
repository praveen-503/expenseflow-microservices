import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { Category } from '../../core/models/expense.model';
import { ExpenseService } from '../../core/services/expense.service';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatListModule,
    MatIconModule
  ],
  templateUrl: './categories.component.html',
  styleUrl: './categories.component.css'
})
export class CategoriesComponent implements OnInit {
  private readonly expenseService = inject(ExpenseService);
  private readonly fb = inject(FormBuilder);

  protected readonly categories = signal<Category[]>([]);
  protected readonly errorMessage = signal('');
  protected readonly showForm = signal(false);

  protected readonly categoryForm: FormGroup = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.maxLength(500)]]
  });

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories() {
    this.expenseService.getCategories().subscribe({
      next: (data) => this.categories.set(data),
      error: () => this.errorMessage.set('Could not load categories.')
    });
  }

  onSubmit() {
    if (this.categoryForm.invalid) return;

    this.expenseService.createCategory(this.categoryForm.value).subscribe({
      next: () => {
        this.loadCategories();
        this.categoryForm.reset();
        this.showForm.set(false);
        this.errorMessage.set('');
      },
      error: (err) => {
        this.errorMessage.set(err.message || 'Failed to save category. Note: duplicate names are not allowed.');
      }
    });
  }

  deleteCategory(id: string) {
    if (confirm('Are you sure you want to delete this category? Category can only be deleted if no expenses refer to it.')) {
      this.expenseService.deleteCategory(id).subscribe({
        next: () => {
          this.loadCategories();
          this.errorMessage.set('');
        },
        error: (err) => {
          this.errorMessage.set(err.message || 'Cannot delete category (likely referenced by expenses).');
        }
      });
    }
  }
}
