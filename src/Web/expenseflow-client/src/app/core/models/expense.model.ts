export interface Category {
  id: string;
  name: string;
  description: string;
  userId?: string | null;
}

export interface Expense {
  id: string;
  title: string;
  amount: number;
  expenseDate: string;
  notes: string;
  categoryId: string;
  category?: Category;
  userId: string;
  createdAt?: string;
  lastModifiedAt?: string;
}

export interface CategorySummary {
  categoryName: string;
  totalAmount: number;
}

export interface ExpenseSummary {
  totalExpense: number;
  expensesByCategory: CategorySummary[];
  highestExpense: number;
  lowestExpense: number;
  averageExpense: number;
  currentMonthTotal: number;
  previousMonthTotal: number;
}
