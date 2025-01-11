import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AuthRoutingModule } from './auth-routing.module';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
import { AuthComponent } from './auth.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { LanguagesComponent } from './layout/languages.component';
import { HeaderComponent } from './layout/header.component';
import { FooterComponent } from './layout/footer.component';

// tenants
import { TenantChangeComponent } from './tenant/tenant-change.component';
import { TenantChangeDialogComponent } from './tenant/tenant-change-dialog.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        SharedModule,
        ServiceProxyModule,
        AuthRoutingModule,
        ModalModule.forChild()
    ],
    declarations: [
        AuthComponent,
        LoginComponent,
        RegisterComponent,
        LanguagesComponent,
        HeaderComponent,
        FooterComponent,
        TenantChangeComponent,
        TenantChangeDialogComponent,
    ]
})
export class AuthModule { }
