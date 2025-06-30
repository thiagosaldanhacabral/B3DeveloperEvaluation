import { TestBed, ComponentFixture, fakeAsync, tick } from '@angular/core/testing';
import { InvestmentCalculatorComponent } from './investment-calculator.component';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('InvestmentCalculatorComponent', () => {
  let component: InvestmentCalculatorComponent;
  let fixture: ComponentFixture<InvestmentCalculatorComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [InvestmentCalculatorComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(InvestmentCalculatorComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
    fixture.detectChanges();
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('deve mostrar erro se valor inicial <= 1', () => {
    component.initialAmount = 1;
    component.months = 5;
    component.calculate();
    expect(component.error).toBe('O valor inicial deve ser maior que 1.');
  });

  it('deve mostrar erro se meses < 2', () => {
    component.initialAmount = 100;
    component.months = 1;
    component.calculate();
    expect(component.error).toBe('O número de meses deve ser maior que 1.');
  });

  it('deve chamar API e preencher valores em caso de sucesso', fakeAsync(() => {
    component.initialAmount = 100;
    component.months = 12;
    component.calculate();

    const req = httpMock.expectOne('http://localhost:5100/calculate');
    expect(req.request.method).toBe('POST');

    req.flush({
      grossAmount: 120,
      netAmount: 110,
      taxAmount: 10
    });

    tick(); // avança o tempo simulado para processar o subscribe

    expect(component.grossAmount).toBe(120);
    expect(component.netAmount).toBe(110);
    expect(component.taxAmount).toBe(10);
    expect(component.finalAmount).toBe(110);
    expect(component.loading).toBe(false);
    expect(component.error).toBeNull();
  }));

  it('deve mostrar erro em caso de falha na API', fakeAsync(() => {
    component.initialAmount = 100;
    component.months = 12;
    component.calculate();

    const req = httpMock.expectOne('http://localhost:5100/calculate');
    req.error(new ErrorEvent('erro'));

    tick();

    expect(component.error).toBe('Erro ao calcular investimento.');
    expect(component.loading).toBe(false);
  }));
});
