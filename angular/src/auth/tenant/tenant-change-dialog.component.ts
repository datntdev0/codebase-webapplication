import { Component, Injector } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/app-component-base';
import { AppTenantAvailabilityState } from '@shared/AppEnums';
import { IdentityServiceProxy } from '@shared/service-proxies/service-proxies';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';

@Component({
  templateUrl: './tenant-change-dialog.component.html'
})
export class TenantChangeDialogComponent extends AppComponentBase {
  saving = false;
  tenancyName = '';

  constructor(
    injector: Injector,
    private _identityService: IdentityServiceProxy,
    protected _modalRef: MdbModalRef<TenantChangeDialogComponent>
  ) {
    super(injector);
  }

  save(): void {
    if (!this.tenancyName) {
      abp.multiTenancy.setTenantIdCookie(undefined);
      location.reload();
      return;
    }

    this.saving = true;
    this._identityService.getTenantStatus(this.tenancyName).subscribe(
      (result) => {
        switch (result.status) {
          case AppTenantAvailabilityState.Available:
            abp.multiTenancy.setTenantIdCookie(result.tenantId);
            location.reload();
            return;
          case AppTenantAvailabilityState.InActive:
            this.message.warn(this.l('TenantIsNotActive', this.tenancyName));
            break;
          case AppTenantAvailabilityState.NotFound:
            this.message.warn(
              this.l('ThereIsNoTenantDefinedWithName{0}', this.tenancyName)
            );
            break;
        }
      },
      () => {
        this.saving = false;
      }
    );
  }
}
