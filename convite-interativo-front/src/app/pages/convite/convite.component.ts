import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ConvidadoService } from '../../services/convidado.service';

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
      confirmado: true,
    };

    this.convidadoService.addConvidado(convidado).subscribe({
      next: () => {
        this.mostrarMensagem('Presença confirmada com sucesso! 🎉', true);

        // Limpar campo após 3 segundos
        setTimeout(() => {
          this.nomeConvidado = '';
          this.showMensagem = false;
        }, 3000);
      },
      error: () => {
        this.mostrarMensagem('Erro ao confirmar presença. Tente novamente.', false);
      },
    });
  }

  mostrarMensagem(msg: string, sucesso: boolean) {
    this.mensagem = msg;
    this.showMensagem = true;
  }
}
