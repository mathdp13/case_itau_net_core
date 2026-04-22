import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { FundoService } from '../../core/services/fundo.service';
import { AuthService } from '../../core/services/auth.service';
import { Fundo } from '../../core/models/fundo.model';

@Component({
  selector: 'app-fundos-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './fundos-list.component.html',
  styleUrl: './fundos-list.component.css'
})
export class FundosListComponent implements OnInit {
  fundos: Fundo[] = [];
  loading = false;
  error = '';

  movimentandoCodigo: string | null = null;
  valorPatrimonio = 0;

  confirmandoExclusao: string | null = null;
  erroModal = '';

  constructor(
    private fundoService: FundoService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.carregar();
  }

  carregar(): void {
    this.loading = true;
    this.fundoService.getAll().subscribe({
      next: data => { this.fundos = data; this.loading = false; },
      error: () => { this.error = 'Erro ao carregar fundos.'; this.loading = false; }
    });
  }

  novo(): void {
    this.router.navigate(['/fundos/novo']);
  }

  editar(codigo: string): void {
    this.router.navigate(['/fundos', codigo, 'editar']);
  }

  abrirExcluir(codigo: string): void {
    this.confirmandoExclusao = codigo;
    this.erroModal = '';
  }

  confirmarExcluir(): void {
    if (!this.confirmandoExclusao) return;
    this.fundoService.delete(this.confirmandoExclusao).subscribe({
      next: () => { this.confirmandoExclusao = null; this.carregar(); },
      error: () => { this.erroModal = 'Erro ao excluir fundo.'; }
    });
  }

  abrirMovimentar(codigo: string): void {
    this.movimentandoCodigo = codigo;
    this.valorPatrimonio = 0;
    this.erroModal = '';
  }

  confirmarMovimentar(): void {
    if (!this.movimentandoCodigo) return;
    this.fundoService.movimentarPatrimonio(this.movimentandoCodigo, { valor: this.valorPatrimonio }).subscribe({
      next: () => { this.movimentandoCodigo = null; this.carregar(); },
      error: () => { this.erroModal = 'Erro ao movimentar patrimônio.'; }
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
