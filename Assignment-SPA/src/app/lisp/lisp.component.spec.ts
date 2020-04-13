import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LispComponent } from './lisp.component';

describe('LispComponent', () => {
  let component: LispComponent;
  let fixture: ComponentFixture<LispComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LispComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LispComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
