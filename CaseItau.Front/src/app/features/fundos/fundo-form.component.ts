import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FundoService } from '../../core/services/fundo.service';
import { TipoFundo } from '../../core/models/fundo.model';

@Component({
  selector: 'app-fundo-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './fundo-form.component.html',
  styleUrl: './fundo-form.component.css'
})
export class FundoFormComponent implements OnInit {
  isEdit = false;
  codigo = '';
  nome = '';
  cnpj = '';
  codigoTipo = 0;
  tipos: TipoFundo[] = [];
  error = '';
  loading = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fundoService: FundoService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.fundoService.getTipos().subscribe({
      next: t => { this.tipos = t; this.cdr.detectChanges(); },
      error: () => {}
    });

    const codigoParam = this.route.snapshot.paramMap.get('codigo');
    if (codigoParam) {
      this.isEdit = true;
      this.codigo = codigoParam;
      this.fundoService.getByCodigo(codigoParam).subscribe({
        next: f => { this.nome = f.nome; this.cnpj = f.cnpj; this.codigoTipo = f.codigoTipo; this.cdr.detectChanges(); },
        error: () => {}
      });
    }
  }

  salvar(): void {
    this.error = '';
    this.loading = true;

    if (this.isEdit) {
      this.fundoService.update(this.codigo, { nome: this.nome, cnpj: this.cnpj, codigoTipo: this.codigoTipo }).subscribe({
        next: () => this.router.navigate(['/fundos']),
        error: (err: { error?: { error?: string } }) => { this.error = err?.error?.error ?? 'Erro ao salvar fundo.'; this.loading = false; this.cdr.detectChanges(); }
      });
    } else {
      this.fundoService.create({ codigo: this.codigo, nome: this.nome, cnpj: this.cnpj, codigoTipo: this.codigoTipo }).subscribe({
        next: () => this.router.navigate(['/fundos']),
        error: (err: { error?: { error?: string } }) => { this.error = err?.error?.error ?? 'Erro ao salvar fundo.'; this.loading = false; this.cdr.detectChanges(); }
      });
    }
  }

  cancelar(): void {
    this.router.navigate(['/fundos']);
  }
}
