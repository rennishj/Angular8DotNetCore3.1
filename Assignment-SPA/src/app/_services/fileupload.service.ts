import { Injectable } from '@angular/core';
import { map, tap, catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class FileuploadService {
  private apiBaseUrl: any = 'http://localhost:5000/api/';
  constructor(private http: HttpClient) {}

  public burstEnrollmentFile(enrollmentFile: any, options: any) {
    const fileData = enrollmentFile;
    const apiUrl =
      this.apiBaseUrl +
      (options.isEnrollment ? 'enrollment/burst' : 'lisp/validate');
    console.log(apiUrl);
    return this.http.post<any>(apiUrl, fileData, {
      reportProgress: true,
      observe: 'events',
    });
  }
}
