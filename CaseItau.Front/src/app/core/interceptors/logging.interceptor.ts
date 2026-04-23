import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { SpanStatusCode } from '@opentelemetry/api';
import { tap, catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { getTracer } from '../telemetry/telemetry';

export const loggingInterceptor: HttpInterceptorFn = (req, next) => {
  const span = getTracer().startSpan(`HTTP ${req.method} ${req.urlWithParams}`);
  span.setAttribute('http.method', req.method);
  span.setAttribute('http.url', req.urlWithParams);

  const start = Date.now();

  return next(req).pipe(
    tap(event => {
      if (event instanceof HttpResponse) {
        span.setAttribute('http.status_code', event.status);
        span.end();
        console.info(`[HTTP] ${req.method} ${req.urlWithParams} ${event.status} — ${Date.now() - start}ms`);
      }
    }),
    catchError(err => {
      span.setAttribute('http.status_code', err.status ?? 0);
      span.setStatus({ code: SpanStatusCode.ERROR });
      span.end();
      console.error(`[HTTP ERROR] ${req.method} ${req.urlWithParams} — ${err.status} (${Date.now() - start}ms)`);
      return throwError(() => err);
    })
  );
};
