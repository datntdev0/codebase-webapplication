import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MdbRippleModule } from 'mdb-angular-ui-kit/ripple';

import { MdbModalModule } from 'mdb-angular-ui-kit/modal';

import { SharedModule } from '@shared/shared.module';

import { AbpMessageComponent } from './components/abp-message/abp-message.component';

@NgModule({
  declarations: [
    AbpMessageComponent,
  ],
  imports: [
    CommonModule,
    MdbRippleModule,
    MdbModalModule,
    SharedModule,
  ], 
  exports: [
    AbpMessageComponent,
  ]
})
export class ThemeModule { }
