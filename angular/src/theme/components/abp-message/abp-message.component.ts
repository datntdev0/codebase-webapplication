import { Component } from '@angular/core';
import { MdbModalRef } from 'mdb-angular-ui-kit/modal';

@Component({
  selector: 'abp-message',
  templateUrl: './abp-message.component.html'
})
export class AbpMessageComponent {
  protected message: string;
  protected title: string;
  protected type: 'info' | 'success' | 'warn' | 'error' | 'confirm';

  constructor(public modalRef: MdbModalRef<AbpMessageComponent>) { }
}
