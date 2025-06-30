import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-investment-calculator',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './investment-calculator.component.html',
  styleUrls: ['./investment-calculator.component.css']
})
export class InvestmentCalculatorComponent {
  initialAmount = 0;
  months = 12;
  finalAmount: number | null = null;
  grossAmount: number | null = null;
  netAmount: number | null = null;
  taxAmount: number | null = null;
  loading = false;
  error: string | null = null;

  constructor(private http: HttpClient) {}

  calculate() {
    // Validação dos campos
    if (this.initialAmount <= 0) {
      this.error = 'O valor inicial deve ser maior que zero.';
      return;
    }
    if (this.months < 2) {
      this.error = 'O número de meses deve ser maior que 1.';
      return;
    }

    this.loading = true;
    this.error = null;
    this.grossAmount = null;
    this.netAmount = null;
    this.taxAmount = null;

    const request = {
      amount: this.initialAmount,
      months: this.months
    };

    this.http.post<any>('http://localhost:5100/calculate', request).subscribe({
      next: (response) => {
        this.grossAmount = response.grossAmount;
        this.netAmount = response.netAmount;
        this.taxAmount = response.taxAmount;
        this.finalAmount = response.netAmount;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Erro ao calcular investimento.';
        this.loading = false;
      }
    });
  }
}
