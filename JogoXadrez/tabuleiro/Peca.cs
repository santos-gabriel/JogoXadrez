using System;
using System.Collections.Generic;
using System.Text;

namespace JogoXadrez.tabuleiro
{
    abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; set; }    
        public int QtdeMovimentos { get; protected set; }
        public Tabuleiro Tab { get; set; }
        
        public Peca(Tabuleiro tab, Cor cor)
        {
            this.Posicao = null;
            this.Tab = tab;
            this.Cor = cor;
            this.QtdeMovimentos = 0;
        }

        public void incrementarMovimentos()
        {
            QtdeMovimentos++;
        }

        public bool existeMovimentosPossiveis()
        {
            bool [,] mat = movimentosPossiveis();
            for (int i = 0; i < Tab.Linhas; i++)
            {
                for (int j = 0; j < Tab.Colunas; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool podeMoverPara(Posicao pos)
        {
            return movimentosPossiveis()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] movimentosPossiveis();

    }
}
