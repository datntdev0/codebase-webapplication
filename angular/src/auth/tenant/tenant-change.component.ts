import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { TenantChangeDialogComponent } from './tenant-change-dialog.component';
import { MdbModalService } from 'mdb-angular-ui-kit/modal';

@Component({
  selector: 'tenant-change',
  templateUrl: './tenant-change.component.html'
})
export class TenantChangeComponent extends AppComponentBase implements OnInit {
  protected name = '';
  protected tenancyName = '';

  constructor(injector: Injector, private _modalService: MdbModalService) {
    super(injector);
  }

  get isMultiTenancyEnabled(): boolean {
    return abp.multiTenancy.isEnabled;
  }

  ngOnInit() {
    if (this.appSession.tenant) {
      this.name = this.appSession.tenant.name;
      this.tenancyName = this.appSession.tenant.tenancyName;
    }
  }

  showChangeModal(): void {
    this._modalService.open(TenantChangeDialogComponent);
  }
}
