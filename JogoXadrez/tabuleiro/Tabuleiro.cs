using System;
using System.Collections.Generic;
using System.Text;

namespace JogoXadrez.tabuleiro
{
    class Tabuleiro
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Peca[,] Pecas;

        public Tabuleiro(int linhas, int colunas)
        {
            this.Linhas = linhas;
            this.Colunas = colunas;
            this.Pecas = new Peca[linhas, colunas];
        }

        public Peca getPeca(int linha, int coluna)
        {
            return this.Pecas[linha, coluna];
        }

        public Peca getPeca(Posicao pos)
        {
            return this.Pecas[pos.Linha, pos.Coluna];
        }

        public void colocarPeca(Peca p, Posicao pos)
        {
            if (verificaExistencia(pos))
            {
                throw new TabuleiroException("Já existe uma peça nesta posição!");
            }
            this.Pecas[pos.Linha, pos.Coluna] = p;
            p.Posicao = pos;
        }

        public Peca retirarPeca (Posicao p)
        {
            if (getPeca(p) == null)
            {
                return null;
            }
            Peca aux = getPeca(p);
            aux.Posicao = null;
            Pecas[p.Linha, p.Coluna] = null;
            return aux;
        }

        public bool posicaoValida(Posicao pos)
        {
            if (pos.Linha < 0 || pos.Linha >= this.Linhas || pos.Coluna < 0 || pos.Coluna >= this.Colunas)
            {
                return false;
            }
            return true;
        }

        public void validaPosicao(Posicao pos)
        {
            if (!posicaoValida(pos))
            {
                throw new TabuleiroException("Posição inválida!");
            }
        }

        public bool verificaExistencia(Posicao pos)
        {
            validaPosicao(pos);
            return getPeca(pos) != null;
        }

    }
}
