import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Fundo, CreateFundo, UpdateFundo, MovimentarPatrimonio, TipoFundo } from '../models/fundo.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class FundoService {
  private readonly apiUrl = `${environment.apiUrl}/fundos`;
  private readonly tiposUrl = `${environment.apiUrl}/tipos-fundo`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Fundo[]> {
    return this.http.get<Fundo[]>(this.apiUrl);
  }

  getByCodigo(codigo: string): Observable<Fundo> {
    return this.http.get<Fundo>(`${this.apiUrl}/${codigo}`);
  }

  create(dto: CreateFundo): Observable<Fundo> {
    return this.http.post<Fundo>(this.apiUrl, dto);
  }

  update(codigo: string, dto: UpdateFundo): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${codigo}`, dto);
  }

  delete(codigo: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${codigo}`);
  }

  movimentarPatrimonio(codigo: string, dto: MovimentarPatrimonio): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${codigo}/patrimonio`, dto);
  }

  getTipos(): Observable<TipoFundo[]> {
    return this.http.get<TipoFundo[]>(this.tiposUrl);
  }
}
