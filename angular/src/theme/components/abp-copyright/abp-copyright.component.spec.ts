import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AbpCopyrightComponent } from './abp-copyright.component';

describe('AbpCopyrightComponent', () => {
  let component: AbpCopyrightComponent;
  let fixture: ComponentFixture<AbpCopyrightComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AbpCopyrightComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AbpCopyrightComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
