using System;
using System.Collections.Generic;
using System.Text;
using JogoXadrez.tabuleiro;

namespace JogoXadrez
{
    class Tela
    {
        public static void imprimirTabuleiro(Tabuleiro tab)
        {
            
            for(int i = 0; i < tab.Linhas; i++)
            {

                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (tab.getPeca(i, j) == null)
                    {
                        Console.Write("- ");
                    }
                    else
                    {
                        Console.Write(tab.getPeca(i, j)+" ");
                    }

                }
                Console.WriteLine();
            }

        }
    }
}
