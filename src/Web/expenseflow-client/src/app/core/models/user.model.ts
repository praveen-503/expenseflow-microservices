export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: 'Administrator' | 'User';
  token?: string;
  refreshToken?: string;
}
