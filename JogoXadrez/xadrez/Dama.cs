using System;
using System.Collections.Generic;
using System.Text;
using JogoXadrez.tabuleiro;
namespace JogoXadrez.xadrez
{
    class Dama : Peca
    {

        public Dama(Tabuleiro tab, Cor cor) : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return "D";
        }

        private bool podeMover(Posicao pos)
        {
            Peca p = Tab.getPeca(pos);
            return p == null || p.Cor != Cor;
        }

        public override bool[,] movimentosPossiveis()
        {
        
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            // ESQUERDA
            pos.definirValores(Posicao.Linha, Posicao.Coluna - 1);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.getPeca(pos) != null && Tab.getPeca(pos).Cor != Cor)
                {
                    break;
                }
                pos.definirValores(pos.Linha, pos.Coluna - 1);
            }
            //DIREITA
            pos.definirValores(Posicao.Linha, Posicao.Coluna + 1);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.getPeca(pos) != null && Tab.getPeca(pos).Cor != Cor)
                {
                    break;
                }
                pos.definirValores(pos.Linha, pos.Coluna + 1);
            }
            //ACIMA
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.getPeca(pos) != null && Tab.getPeca(pos).Cor != Cor)
                {
                    break;
                }
                pos.definirValores(pos.Linha - 1, pos.Coluna);
            }
            //ABAIXO
            pos.definirValores(Posicao.Linha + 1, Posicao.Coluna);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.getPeca(pos) != null && Tab.getPeca(pos).Cor != Cor)
                {
                    break;
                }
                pos.definirValores(pos.Linha + 1, pos.Coluna);
            }
            //NO
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.getPeca(pos) != null && Tab.getPeca(pos).Cor != Cor)
                {
                    break;
                }
                pos.definirValores(pos.Linha - 1, pos.Coluna - 1);
            }
            //NE
            pos.definirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.getPeca(pos) != null && Tab.getPeca(pos).Cor != Cor)
                {
                    break;
                }
                pos.definirValores(pos.Linha - 1, pos.Coluna + 1);
            }
            //SE
            pos.definirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.getPeca(pos) != null && Tab.getPeca(pos).Cor != Cor)
                {
                    break;
                }
                pos.definirValores(pos.Linha + 1, pos.Coluna + 1);
            }
            //SO
            pos.definirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            while (Tab.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.getPeca(pos) != null && Tab.getPeca(pos).Cor != Cor)
                {
                    break;
                }
                pos.definirValores(pos.Linha + 1, pos.Coluna - 1);
            }

            return mat;
        }


    }
}
