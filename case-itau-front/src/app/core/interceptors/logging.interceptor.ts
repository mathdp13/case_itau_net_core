import { HttpInterceptorFn } from '@angular/common/http';
import { tap, catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

export const loggingInterceptor: HttpInterceptorFn = (req, next) => {
  const start = Date.now();
  console.info(`[HTTP] ${req.method} ${req.url}`);

  return next(req).pipe(
    tap(event => {
      const duration = Date.now() - start;
      console.info(`[HTTP] ${req.method} ${req.url} — ${duration}ms`);
    }),
    catchError(err => {
      const duration = Date.now() - start;
      console.error(`[HTTP ERROR] ${req.method} ${req.url} — ${err.status} (${duration}ms)`, err);
      return throwError(() => err);
    })
  );
};
