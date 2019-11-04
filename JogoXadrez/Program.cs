using System;
using JogoXadrez.tabuleiro;
using JogoXadrez.xadrez;

namespace JogoXadrez
{
    class Program
    {
        static void Main(string[] args)
        {
            
            try
            {
                PartidaDeXadrez partida = new PartidaDeXadrez();
                Cor ultimoJogadorDaPartida = partida.JogadorAtual;
                while (!partida.Terminada)
                {
                    try
                    {
                        ultimoJogadorDaPartida = partida.JogadorAtual;
                        Console.Clear();
                        Tela.imprimirPartida(partida);
                        Console.WriteLine();
                        Console.Write("Origem: ");
                        Posicao origem = Tela.lerPosicaoXadrez().toPosicao();
                        partida.validaPosicaoDeOrigem(origem);

                        bool[,] posicoesPossiveis = partida.Tab.getPeca(origem).movimentosPossiveis();

                        Console.Clear();
                        Tela.imprimirTabuleiro(partida.Tab, posicoesPossiveis);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Posicao destino = Tela.lerPosicaoXadrez().toPosicao();
                        partida.validarPosicaoDeDestino(origem, destino);

                        partida.realizaMovimento(origem, destino);
                    }
                    catch (TabuleiroException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }
                Console.Clear();
                Tela.imprimirPartidaXequeMate(ultimoJogadorDaPartida, partida);

            }
            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
            
            /*
            PosicaoXadrez p = new PosicaoXadrez('c', 7);
            Console.WriteLine(p);
            Console.WriteLine(p.toPosicao());
            */
            
            Console.ReadLine();
        }
    }
}
