using System;
using System.Collections.Generic;
using System.Text;
using JogoXadrez.tabuleiro;

namespace JogoXadrez.xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public bool Terminada { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool xeque { get; private set; }
        public Peca vulneravelEnPassant { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        
        public PartidaDeXadrez ()
        {
            this.Tab = new Tabuleiro(8, 8);
            this.Turno = 1;
            this.JogadorAtual = Cor.Branca;
            this.xeque = false;
            vulneravelEnPassant = null;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public Peca executarMovimento (Posicao origem, Posicao destino)
        {
            Peca p = Tab.retirarPeca(origem);
            p.incrementarMovimentos();
            Peca pecaRetirada = Tab.retirarPeca(destino);
            Tab.colocarPeca(p, destino);

            if (pecaRetirada != null)
            {
                capturadas.Add(pecaRetirada);
            }

            //#JOGADA ESPECIAL ROQUE PEQUENO
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao (origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao (origem.Linha, origem.Coluna + 1);
                Peca T = Tab.retirarPeca(origemT);
                T.incrementarMovimentos();
                Tab.colocarPeca(T, destinoT);
            }

            //#JOGADA ESPECIAL ROQUE GRANDE
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.retirarPeca(origemT);
                T.incrementarMovimentos();
                Tab.colocarPeca(T, destinoT);
            }

            //#JOGA ESPECIAL EN PASSANT
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaRetirada == null)
                {
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaRetirada = Tab.retirarPeca(posP);
                    capturadas.Add(pecaRetirada);
                }
            }

            return pecaRetirada;
        }

        public HashSet<Peca> pecasCapturadas (Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in capturadas)
            {
                if (p.Cor == cor)
                {
                    aux.Add(p);
                }
            }
            return aux;
        }

        private Cor adversaria (Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }

        private Peca rei (Cor cor)
        {
            foreach (Peca p in pecasEmJogo(cor))
            {
                if (p is Rei)
                {
                    return p;
                }
            }
            return null;
        }

        public bool estaEmXeque (Cor cor)
        {
            Peca r = rei(cor);
            if(r == null)
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro! ");
            }

            foreach(Peca p in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = p.movimentosPossiveis();
                if(mat[r.Posicao.Linha, r.Posicao.Coluna])
                {
                    return true;
                }
            }

            return false;

        }

        public HashSet<Peca> pecasEmJogo (Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in pecas)
            {
                if (p.Cor == cor)
                {
                    aux.Add(p);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;

        }

        public void realizaMovimento(Posicao origem, Posicao destino)
        {
            Peca pecaRetirada = executarMovimento(origem, destino);

            if (estaEmXeque(JogadorAtual))
            {
                desfazMovimento(origem, destino, pecaRetirada);
                throw new TabuleiroException("Você não pode se colocar em xeque! ");
            }

            Peca p = Tab.getPeca(destino);

            //#JOGADA ESPECIAL PROMOÇÃO
            if (p is Peao)
            {
                if ((p.Cor == Cor.Branca && destino.Linha == 0) || (p.Cor == Cor.Preta && destino.Linha == 7))
                {
                    p = Tab.retirarPeca(destino);
                    pecas.Remove(p);
                    Peca dama = new Dama(Tab, p.Cor);
                    Tab.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }


            if (estaEmXeque(adversaria(JogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }
            if (testeXequeMate(adversaria(JogadorAtual))){
                Terminada = true;
            }
            else
            {
                Turno++;
                mudaJogador();
            }

            
            //#JOGADA ESPECIAL EN PASSANT
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                vulneravelEnPassant = p;
            }
            else {
                vulneravelEnPassant = null;
            }
            
        }


        public bool testeXequeMate(Cor cor)
        {
            if (!xeque)
            {
                return false;
            }
            foreach (Peca p in pecasEmJogo(cor))
            {
                bool[,] mat = p.movimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for (int j = 0; j < Tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = p.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executarMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca capturada)
        {
            Peca p = Tab.retirarPeca(destino);
            p.decrementarMovimentos();

            if(capturada != null)
            {
                Tab.colocarPeca(capturada, destino);
                capturadas.Remove(capturada);
            }
            Tab.colocarPeca(p, origem);

            //#JOGADA ESPECIAL ROQUE PEQUENO
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.retirarPeca(destinoT);
                T.decrementarMovimentos();
                Tab.colocarPeca(T, origemT);
            }

            //#JOGADA ESPECIAL ROQUE GRANDE
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.retirarPeca(destinoT);
                T.decrementarMovimentos();
                Tab.colocarPeca(T, origemT);
            }

            //#JOGADA ESPECIAL EN PASSANT
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && capturada == vulneravelEnPassant)
                {
                    Peca peao = Tab.retirarPeca(destino);
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    Tab.colocarPeca(peao, posP);
                }
            }

        }

        public void mudaJogador()
        {
            if(JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            }
            else
            {
                JogadorAtual = Cor.Branca;
            }
        }

        public void validaPosicaoDeOrigem (Posicao pos)
        {
            if (Tab.getPeca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição escolhida! ");
            }
            if (JogadorAtual != Tab.getPeca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua! ");
            }
            if (!Tab.getPeca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça escolhida! ");
            }
            
        }

        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.getPeca(origem).podeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        public void colocarPecas()
        {
            colocarNovaPeca('a', 1, new Torre(Tab, Cor.Branca));
            colocarNovaPeca('b', 1, new Cavalo(Tab, Cor.Branca));
            colocarNovaPeca('c', 1, new Bispo(Tab, Cor.Branca));
            colocarNovaPeca('d', 1, new Dama(Tab, Cor.Branca));
            colocarNovaPeca('e', 1, new Rei(Tab, Cor.Branca, this));
            colocarNovaPeca('f', 1, new Bispo(Tab, Cor.Branca));
            colocarNovaPeca('g', 1, new Cavalo(Tab, Cor.Branca));
            colocarNovaPeca('h', 1, new Torre(Tab, Cor.Branca));
            colocarNovaPeca('a', 2, new Peao(Tab, Cor.Branca, this));
            colocarNovaPeca('b', 2, new Peao(Tab, Cor.Branca, this));
            colocarNovaPeca('c', 2, new Peao(Tab, Cor.Branca, this));
            colocarNovaPeca('d', 2, new Peao(Tab, Cor.Branca, this));
            colocarNovaPeca('e', 2, new Peao(Tab, Cor.Branca, this));
            colocarNovaPeca('f', 2, new Peao(Tab, Cor.Branca, this));
            colocarNovaPeca('g', 2, new Peao(Tab, Cor.Branca, this));
            colocarNovaPeca('h', 2, new Peao(Tab, Cor.Branca, this));

            colocarNovaPeca('a', 8, new Torre(Tab, Cor.Preta));
            colocarNovaPeca('b', 8, new Cavalo(Tab, Cor.Preta));
            colocarNovaPeca('c', 8, new Bispo(Tab, Cor.Preta));
            colocarNovaPeca('d', 8, new Dama(Tab, Cor.Preta));
            colocarNovaPeca('e', 8, new Rei(Tab, Cor.Preta, this));
            colocarNovaPeca('f', 8, new Bispo(Tab, Cor.Preta));
            colocarNovaPeca('g', 8, new Cavalo(Tab, Cor.Preta));
            colocarNovaPeca('h', 8, new Torre(Tab, Cor.Preta));
            colocarNovaPeca('a', 7, new Peao(Tab, Cor.Preta, this));
            colocarNovaPeca('b', 7, new Peao(Tab, Cor.Preta, this));
            colocarNovaPeca('c', 7, new Peao(Tab, Cor.Preta, this));
            colocarNovaPeca('d', 7, new Peao(Tab, Cor.Preta, this));
            colocarNovaPeca('e', 7, new Peao(Tab, Cor.Preta, this));
            colocarNovaPeca('f', 7, new Peao(Tab, Cor.Preta, this));
            colocarNovaPeca('g', 7, new Peao(Tab, Cor.Preta, this));
            colocarNovaPeca('h', 7, new Peao(Tab, Cor.Preta, this));
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

    }
}
