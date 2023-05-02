import { Injectable } from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {SessionsService} from "./sessions.service";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class InterceptorService implements HttpInterceptor {
  constructor(private sessions: SessionsService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.sessions.getToken();

    request = request.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });

    return next.handle(request);
  }
}
