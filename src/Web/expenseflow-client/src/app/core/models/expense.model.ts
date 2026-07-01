export interface Category {
  id: string;
  name: string;
  description: string;
}

export interface Expense {
  id: string;
  description: string;
  amount: number;
  date: string;
  categoryId: string;
  category?: Category;
  userId: string;
}
