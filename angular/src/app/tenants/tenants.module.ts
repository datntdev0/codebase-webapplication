import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';
import { CreateTenantDialogComponent } from './create-tenant/create-tenant-dialog.component';
import { EditTenantDialogComponent } from './edit-tenant/edit-tenant-dialog.component';
import { TenantsRoutingModule } from './tenants-routing.module';
import { TenantsComponent } from './tenants.component';
import { ThemeModule } from '@theme/theme.module';

@NgModule({
    declarations: [
        CreateTenantDialogComponent,
        EditTenantDialogComponent,
        TenantsComponent,
    ],
    imports: [
        SharedModule,
        TenantsRoutingModule,
        CommonModule,
        ThemeModule,
        ThemeModule.MdbModules,
    ],
})
export class TenantsModule { }
