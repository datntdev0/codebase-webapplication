import { CommonModule } from '@angular/common';
import { provideHttpClient, withInterceptorsFromDi, withJsonpSupport } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AuthRoutingModule } from './auth-routing.module';
import { AuthComponent } from './auth.component';
import { LanguagesComponent } from './languages/languages.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';

// tenants
import { ThemeModule } from '@theme/theme.module';
import { TenantChangeDialogComponent } from './tenant/tenant-change-dialog.component';
import { TenantChangeComponent } from './tenant/tenant-change.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    ThemeModule,
    ServiceProxyModule,
    AuthRoutingModule,
    ModalModule.forChild()
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi(), withJsonpSupport()),
  ],
  declarations: [
    AuthComponent,
    LoginComponent,
    RegisterComponent,
    LanguagesComponent,
    TenantChangeComponent,
    TenantChangeDialogComponent,
  ]
})
export class AuthModule { }
