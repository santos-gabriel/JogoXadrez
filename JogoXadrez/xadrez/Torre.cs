using System;
using System.Collections.Generic;
using System.Text;
using JogoXadrez.tabuleiro;

namespace JogoXadrez.xadrez
{
    class Torre : Peca
    {
        public Torre (Tabuleiro tab, Cor cor) : base (tab, cor)
        {
        }

        public override string ToString()
        {
            return "T";
        }

    }
}
