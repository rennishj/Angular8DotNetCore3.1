import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpEventType } from '@angular/common/http';
import { LocationStrategy } from '@angular/common';
import { FileUploader } from 'ng2-file-upload';
import { AlertifyService } from '../_services/alertify.service';
import { FileuploadService } from '../_services/fileupload.service';

@Component({
  selector: 'app-enrollment',
  templateUrl: './enrollment.component.html',
  styleUrls: ['./enrollment.component.css'],
})
export class EnrollmentComponent implements OnInit {
  public uploader: FileUploader;
  public hasBaseDropZoneOver: boolean;
  public response: string;
  public fileToburst: any;
  public isEnrollment = false;
  public uploadMessage = 'Please select an Enrollment file for bursting.';
  private baseUrl = 'http:localhost:5000/api';
  public fileUploadResponse: any;
  public responseMessage: string;
  public progress: number;
  @Output() public UploadFinished = new EventEmitter();
  constructor(
    private fileuploadService: FileuploadService,
    private alertify: AlertifyService,
    private url: LocationStrategy
  ) {}

  ngOnInit() {
    if (this.url.path() === '/enrollment') {
      this.isEnrollment = true;
    } else {
      this.uploadMessage =
        'Please select a file containing LISP code to validate';
    }
  }

  public burstEnrollmentFile(files: any) {
    if (files.length === 0) return;

    const fileToupload = files[0] as any;
    const formData = new FormData();
    formData.append('file', fileToupload, fileToupload.name);
    const options = {
      reportProgress: true,
      observe: 'events',
      isEnrollment: this.isEnrollment,
    };
    this.fileuploadService.burstEnrollmentFile(formData, options).subscribe(
      (event) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progress = Math.round((100 * event.loaded) / event.total);
        } else if (event.type === HttpEventType.Response) {
          this.responseMessage = 'Upload success';
          this.fileUploadResponse = event.body;
          this.UploadFinished.emit(event.body); // event.body has the data from api.
        }
      },
      (error) => {
        this.alertify.error('Failed to upload enrollment file.');
      }
    );
  }
}
