import { Injectable } from '@angular/core';
import { map, tap, catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
// componets are automatically injectable and services are not. Thats why we have the @Injectable attribute on services
@Injectable({
  providedIn: 'root',
})
export class ProviderService {
  private apiBaseUrl: any = 'http://localhost:5000/api/';
  constructor(private http: HttpClient) {}

  public registerProvider(provider: any) {
    return this.http.post(this.apiBaseUrl + 'provider/create', provider);
  }

  public getAllStates(): Observable<any[]> {
    return this.http.get<any[]>(this.apiBaseUrl + 'register/states').pipe(
      tap((data) => data),
      catchError(this.handleError)
    );
  }

  private handleError(err: HttpErrorResponse) {
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      errorMessage = `An error occurred: ${err.message}`;
    } else {
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  }
}
