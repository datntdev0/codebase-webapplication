import { Component, OnInit } from '@angular/core';
import { AbpMessageComponent } from '@theme/components/abp-message/abp-message.component';
import { MdbModalService } from 'mdb-angular-ui-kit/modal';

@Component({
  selector: 'app-root',
  template: `<router-outlet></router-outlet>`
})
export class RootComponent implements OnInit {
  constructor(private _modalService: MdbModalService) { }

  ngOnInit(): void {
    this.initAbpMessage();
  }

  private initAbpMessage(): void {
    ['info', 'success', 'warn', 'error'].forEach(type => {
      abp.message[type] = (message: string, title?: string, options?: any) =>
        this._modalService.open(AbpMessageComponent,
          { data: { message, title, type, options } });
    });
  }
}
