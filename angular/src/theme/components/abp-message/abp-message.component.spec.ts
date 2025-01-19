import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AbpMessageComponent } from './abp-message.component';

describe('AbpMessageComponent', () => {
  let component: AbpMessageComponent;
  let fixture: ComponentFixture<AbpMessageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AbpMessageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AbpMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
