using System;
using System.Collections.Generic;
using System.Text;

namespace JogoXadrez.tabuleiro
{
    class TabuleiroException : Exception
    {
        public TabuleiroException (string menssagem) : base(menssagem)
        {
        }
    }
}
