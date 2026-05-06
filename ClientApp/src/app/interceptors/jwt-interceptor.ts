import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth'; // Ensure path is correct!

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  // Use inject() to get the AuthService inside a functional interceptor
  const authService = inject(AuthService);
  const token = authService.getToken();

  // If we have a token, clone the request and add the Authorization header
  if (token) {
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(authReq);
  }

  // If no token (like when logging in), just send the normal request
  return next(req);
};
