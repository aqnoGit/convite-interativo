import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Convidado {
  id?: number;
  nome: string;
  confirmado: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class ConvidadoService {
   private apiUrl = 'https://api-convite-aaaaf7hhh7f6ajfg.brazilsouth-01.azurewebsites.net/gerenciadorPresenca';
  // private apiUrl = 'http://localhost:5000/gerenciadorPresenca';

  constructor(private http: HttpClient) {}

  // Listar todos os convidados
  getConvidados(): Observable<Convidado[]> {
    return this.http.get<Convidado[]>(`${this.apiUrl}/consultar`);
  }

  // Adicionar novo convidado
  addConvidado(convidado: Convidado): Observable<Convidado> {
    return this.http.post<any>(`${this.apiUrl}/confirmar`, convidado);
  }
}
