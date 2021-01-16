import {HttpInterceptor, HttpRequest, HttpHandler, HttpUserEvent, HttpEvent, HttpErrorResponse} from '@angular/common/http';
import 'rxjs/add/operator/do';
import {Observable, throwError} from 'rxjs';
import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import { catchError, tap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor
{
    constructor(private router: Router)
    {}
    intercept(req: HttpRequest<any>, next: HttpHandler): import('rxjs').Observable<HttpEvent<any>> {
        if (req.headers.get('No-Auth') === 'True') {
            return next.handle(req.clone());
        }
        if (localStorage.getItem('token') != null) {
        const clonedreq = req.clone({
        headers: req.headers.set('Authorization', 'Bearer ' + localStorage.getItem('token'))
        });
        return next.handle(clonedreq)
        .pipe(
          catchError(error => {
            if (error instanceof HttpErrorResponse) {
              if (error.status === 401) {
                localStorage.clear();
 // this.authService.loginStatus(this.senderId, false);
  this.router.navigate(['']);
                return throwError(error.statusText);
              }
              const applicationError = error.headers.get('Application-Error');
              if (applicationError) {
                console.log(applicationError);
                return throwError(applicationError);
              }
              const serverError = error.error;
              let modalStateErrors = '';
              if (serverError && typeof serverError === 'object') {
                for (const key in serverError) {
                 if (serverError[key]) {
                   modalStateErrors += serverError[key] + '\n';
                 }
                }
              }
              return throwError(modalStateErrors || serverError || 'Server Error')
            }
          }),
          tap(
            event => succ => { },
            error => {
                  if (error.status === 401) {
                     this.router.navigateByUrl('');
                  }
                }
          )
        ).do(
            succ => { },
            err => {
              if (err.status === 401) {
                this.router.navigateByUrl('');
              }
            }
          );
      }
      else {
        this.router.navigateByUrl('');
      }
    }
}

// export const ErrorInterceptorProvider = {
//   provide: HTTP_INTERCEPTORS,
//   useClass: ErrorInterceptor,
//   multi: true
// }
