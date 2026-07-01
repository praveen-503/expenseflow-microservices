export interface Notification {
  id: string;
  userId: string;
  title: string;
  message: string;
  type: 'Email' | 'SMS' | 'Push';
  status: 'Pending' | 'Sent' | 'Failed';
  createdAt: string;
}
