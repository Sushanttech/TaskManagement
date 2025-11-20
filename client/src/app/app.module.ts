import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

// Import standalone components
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ProjectFormComponent } from './components/project-form/project-form.component';

// Services / Guards / Interceptors
import { AuthService } from './services/auth.service';
import { AuthGuard } from './guards/auth.guard';
import { ApiInterceptor } from './services/api.interceptor';

@NgModule({
  // Standalone components go inside imports, NOT declarations
  declarations: [],

  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    AppRoutingModule,

    // Standalone components imported here
    AppComponent,
    LoginComponent,
    DashboardComponent,
    ProjectFormComponent
  ],

  providers: [
    AuthService,
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true }
  ],

  // AppComponent is fine as bootstrap even if it’s standalone
  bootstrap: [AppComponent]
})
export class AppModule {}
