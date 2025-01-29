import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { MdbCollapseModule } from 'mdb-angular-ui-kit/collapse';
import { MdbDropdownModule } from 'mdb-angular-ui-kit/dropdown';
import { MdbModalModule } from 'mdb-angular-ui-kit/modal';
import { MdbRippleModule } from 'mdb-angular-ui-kit/ripple';

import { AbpMessageComponent } from './components/abp-message/abp-message.component';
import { AbpCopyrightComponent } from './components/abp-copyright/abp-copyright.component';

@NgModule({
  declarations: [
    AbpMessageComponent,
    AbpCopyrightComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
  ],
  exports: [
    AbpMessageComponent,
    AbpCopyrightComponent,
  ]
})
export class ThemeModule { 

  public static MdbModules = [
    MdbCollapseModule,
    MdbDropdownModule,
    MdbModalModule,
    MdbRippleModule,
  ];

}
