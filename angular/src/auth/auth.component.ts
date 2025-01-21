import {
  Component,
  Injector,
  OnInit,
  ViewEncapsulation
} from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  templateUrl: './auth.component.html',
  encapsulation: ViewEncapsulation.None
})
export class AuthComponent extends AppComponentBase implements OnInit {
  protected isMultiTenancy: boolean;
  protected currentYear: number;
  protected versionNumber: string;
  protected releaseDate: string;

  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();
    this.isMultiTenancy = abp.multiTenancy.isEnabled;
    this.versionNumber = this.appSession.application.version;
    this.releaseDate = this.appSession.application.releaseDate.format("YYYYDDMM");
  }
}
