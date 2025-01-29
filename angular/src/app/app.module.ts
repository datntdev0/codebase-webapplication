import { CommonModule } from '@angular/common';
import { HttpClientJsonpModule, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxPaginationModule } from 'ngx-pagination';
import { AppRoutingModule } from './app-routing.module';
import { ThemeModule } from '@theme/theme.module';
import { AppComponent } from './app.component';
// layout
import { SidebarLanguageMenuComponent } from './layout/sidebar-language-menu.component';
import { SidebarLogoComponent } from './layout/sidebar-logo.component';
import { SidebarMenuComponent } from './layout/sidebar-menu.component';
import { SidebarUserPanelComponent } from './layout/sidebar-user-panel.component';

@NgModule({
    declarations: [
        AppComponent,
        // layout
        SidebarLogoComponent,
        SidebarUserPanelComponent,
        SidebarLanguageMenuComponent,
        SidebarMenuComponent,
    ],
    imports: [
        AppRoutingModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        ModalModule.forChild(),
        BsDropdownModule,
        CollapseModule,
        TabsModule,
        ServiceProxyModule,
        NgxPaginationModule,
        SharedModule,
        ThemeModule,
        ThemeModule.MdbModules,
    ],
    providers: []
})
export class AppModule {}
