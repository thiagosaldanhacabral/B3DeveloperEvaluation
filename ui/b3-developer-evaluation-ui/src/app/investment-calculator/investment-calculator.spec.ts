import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvestmentCalculator } from './investment-calculator';

describe('InvestmentCalculator', () => {
  let component: InvestmentCalculator;
  let fixture: ComponentFixture<InvestmentCalculator>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InvestmentCalculator]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InvestmentCalculator);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
