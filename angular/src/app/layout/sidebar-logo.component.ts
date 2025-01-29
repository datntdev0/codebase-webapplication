import { Component, ChangeDetectionStrategy, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'sidebar-logo',
  templateUrl: './sidebar-logo.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SidebarLogoComponent extends AppComponentBase implements OnInit {
  protected displayName: string;

  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
    this.displayName = this.appSession.tenant ? 
      this.appSession.tenant.tenancyName : this.appName;
  }
}
