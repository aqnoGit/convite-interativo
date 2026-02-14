import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ConvidadoService } from '../../services/convidado.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-convite',
  imports: [FormsModule, CommonModule],
  templateUrl: './convite.component.html',
  styleUrl: './convite.component.css',
})
export class ConviteComponent {
  nomeConvidado: string = '';
  mensagem: string = '';
  showMensagem: boolean = false;

  constructor(private convidadoService: ConvidadoService) {}

  confirmarPresenca() {
    if (!this.nomeConvidado.trim()) {
      this.mostrarMensagem('Por favor, digite seu nome!', false);
      return;
    }

    const convidado = {
      nome: this.nomeConvidado,
      confirmado: true
    };

    this.convidadoService.addConvidado(convidado).subscribe({
      next: () => {
        this.mostrarMensagem('Presença confirmada com sucesso! 🎉', true);
        this.nomeConvidado = '';
      },
      error: () => {
        this.mostrarMensagem('Erro ao confirmar presença. Tente novamente.', false);
      }
    });
  }

  mostrarMensagem(msg: string, sucesso: boolean) {
    this.mensagem = msg;
    this.showMensagem = true;
    setTimeout(() => this.showMensagem = false, 3000);
  }
}