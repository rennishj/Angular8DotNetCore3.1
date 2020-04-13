import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { EnrollmentComponent } from './enrollment/enrollment.component';
import { NavComponent } from './nav/nav.component';
import { ProviderComponent } from './provider/provider.component';
import { ProviderService } from './_services/provider.service';
import { LispComponent } from './lisp/lisp.component';
import { appRoutes } from './routes';
import { FileUploadModule } from 'ng2-file-upload';

@NgModule({
  declarations: [
    AppComponent,
    ProviderComponent,
    NavComponent,
    LispComponent,
    EnrollmentComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    FileUploadModule,
    RouterModule.forRoot(appRoutes),
  ],
  providers: [ProviderService],
  bootstrap: [AppComponent],
})
export class AppModule {}
