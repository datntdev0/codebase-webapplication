import {
  Component,
  Injector,
  OnInit,
  Output,
  EventEmitter,
  ChangeDetectorRef
} from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/app-component-base';
import {
  CreateTenantDto,
  TenantsServiceProxy,
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'create-tenant-dialog.component.html'
})
export class CreateTenantDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  tenant: CreateTenantDto = new CreateTenantDto();

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _tenantService: TenantsServiceProxy,
    public bsModalRef: BsModalRef,
    private cd: ChangeDetectorRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.tenant.isActive = true;
    this.cd.detectChanges();
  }

  save(): void {
    this.saving = true;

    this._tenantService.create(this.tenant).subscribe(
      () => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
        this.onSave.emit();
      },
      () => {
        this.saving = false;
      }
    );
  }
}
