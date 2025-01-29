import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '../../../shared/app-component-base';

@Component({
  selector: 'abp-copyright',
  templateUrl: './abp-copyright.component.html'
})
export class AbpCopyrightComponent extends AppComponentBase implements OnInit {
  protected currentYear: number;
  protected versionNumber: string;
  protected releaseDate: string;
  
  constructor(injector: Injector) {
    super(injector);
  }

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();
    this.versionNumber = this.appSession.application.version;
    this.releaseDate = this.appSession.application.releaseDate.format("YYYYDDMM");
  }
}
