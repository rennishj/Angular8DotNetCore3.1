/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { FileuploadService } from './fileupload.service';

describe('Service: Fileupload', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [FileuploadService]
    });
  });

  it('should ...', inject([FileuploadService], (service: FileuploadService) => {
    expect(service).toBeTruthy();
  }));
});
