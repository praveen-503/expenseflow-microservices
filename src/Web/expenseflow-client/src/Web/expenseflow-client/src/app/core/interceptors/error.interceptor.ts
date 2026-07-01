import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  return next(req).pipe(
    catchError(err => {
      if ([401, 403].includes(err.status)) {
        authService.logout();
      }
      const error = err.error?.message || err.statusText;
      return throwError(() => new Error(error));
    })
  );
};
