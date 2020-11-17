using System;
using System.Drawing;
using System.Threading;
using System.Resources;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DJD_Bricks
{
    class JogoBricks
    {
        public enum GameStatus {NOTREADY=0, READY, PAUSE, ONGOING, ENDED, WIN};  //maquina de estados

        private static Random random = new Random();
        //private static ResourceManager resources = new ResourceManager(typeof(JogoBricks));

        private int comprimento;
        private int altura;
        private Image fundo;

        private List<Bola> bolas;
        //private Bola bola1 = null;
        //private Bola bola2 = null;
        //private Bola bola3 = null;
        private Jogador jogador;
        private Jogador barreira;
        private List<Tijolos> listaTijolos;
        private List<Powers> listaPowers;
        private int score;
        private GameStatus status = GameStatus.NOTREADY;
        
        //construtor da class jogo
        public JogoBricks(int comp, int alt, int score)
        {
            this.comprimento = comp;
            this.altura = alt;

            this.fundo = Properties.Resources.fundo as Image;
            //this.fundo = JogoBricks.resources.GetObject("fundo") as Image; 
            //nao consegui por a funcionar como nos laboratorios, da sempre erro e diz que falta qql coisa

            //dispor bolas
            this.bolas = new List<Bola>();
            this.bolas.Add(new Bola(comp / 2, alt / 2, 15, 15, 270, 40));
            
            //dispor jogador
            jogador = new Jogador(comp/2, alt-5, 120, 45, 0);
            
            //dispor os bricks
            this.listaTijolos = new List<Tijolos>();
           
            for(int i = 0; i < 7; i++)
            {
                for(int j = 0; j < 7; j++)
                {
                    if(i == 1 && j == 1 || i == 1 && j == 5 ||
                        i == 3 && j == 1 || i == 3 && j == 5 || i == 3 && j == 3 ||
                        i == 5 && j == 1 || i == 5 && j == 5)
                    {
                        this.listaTijolos.Add(new Tijolos(TipoBlock.POWER, 100 + j*50, 75 + i*25, 50, 25, 0, 0));
                    }
                    else
                    {
                        this.listaTijolos.Add(new Tijolos(TipoBlock.NORMAL, 100 + j*50, 75 + i*25, 50, 25, 0, 0));
                    }
                }
            }
            

            //cria a lista para os powers
            this.listaPowers = new List<Powers>();

            this.score = 0;
            this.status = GameStatus.READY;
        }

        //metodo que devolve o status
        public GameStatus Status
        {
            get
            {
                return this.status;
            }
        }

        //metodo que inicia o jogo/maquina de estado
        public void Start()
        {
            this.status = GameStatus.ONGOING;
        }

        //metodo que para o jogo
        public void Stop()
        {
            this.status = GameStatus.ENDED;
        }

        //metodo que pausa o jogo
        public void Pausa()
        {
            if(this.status == GameStatus.PAUSE)
            {
                this.status = GameStatus.ONGOING;
            }
            else
            {
                this.status = GameStatus.PAUSE;
            }
        }

        //precessamento do input
        public void ProcessInputs()
        {
            for (int b = 0; b < bolas.Count; b++)
            {
                this.jogador.PlayerInput(this.bolas[b].pX);
            }
        }

        //metodo da update de todo o jogo
        public GameStatus Update()
        {
            if (this.status != GameStatus.ONGOING)
            {
                return this.Status;
            }

            //move as bolas e o jogador
            for (int b = 0; b < bolas.Count; b++)
            {
                this.bolas[b].Move();
            }
            this.jogador.Move();

            //restrições para o jogador nao sair do ecran
            if (this.jogador.pX < this.jogador.comprimento / 2) this.jogador.pX = this.jogador.comprimento / 2;
            if (this.jogador.pX > this.comprimento - this.jogador.comprimento / 2) this.jogador.pX = this.comprimento - this.jogador.comprimento / 2;


            //verifica se a bola vai sair da janela de jogo sem contar com a "baliza" e coloca-a de volta em jogo
            for (int b = 0; b < bolas.Count; b++)
            {
                if (this.bolas[b].pY < 0)
                {
                    this.bolas[b].direcao = -this.bolas[b].direcao;
                    this.bolas[b].Move();
                }
                if (this.bolas[b].pX < 0 || this.bolas[b].pX > this.comprimento)
                {
                    this.bolas[b].direcao = (540 - bolas[b].direcao) % 360;
                    this.bolas[b].direcao += new Random().Next(25) - 10;        //factor que impede que a bola fique presa infinitamente na horizontal
                    this.bolas[b].Move();
                }
            }

            //verifica quando a bola bate no jogador/barreira
            for (int b = 0; b < bolas.Count; b++)
            {
                if (this.bolas[b].IntersetaCom(this.jogador) == true)
                {
                    this.bolas[b].direcao = -this.bolas[b].direcao;
                    this.bolas[b].direcao += new Random().Next(30) - 10;
                    this.bolas[b].velocidade *= 1.02f;  // acelera a bola
                    //a ponderar o handicap
                    this.bolas[b].Move();   //volta a colocar a bola em jogo
                }
                if(barreira != null)
                {
                    if (this.bolas[b].IntersetaCom(this.barreira))
                    {
                        this.bolas[b].direcao = -this.bolas[b].direcao;
                        this.bolas[b].direcao += new Random().Next(30) - 10;
                        this.bolas[b].velocidade *= 1.02f;  // acelera a bola
                                                            //a ponderar o handicap
                        this.bolas[b].Move();   //volta a colocar a bola em jog
                    }
                }
            }

            //verifica quando a bola bate nos tijolos
            for (int b = 0; b < bolas.Count; b++)
            {
                for (int i = 0; i < listaTijolos.Count; i++)
                {
                    if (this.bolas[b].IntersetaCom(listaTijolos[i]) == true)
                    {
                        this.score = this.score + 100;
                        this.bolas[b].direcao = -this.bolas[b].direcao;
                        if (listaTijolos[i].tipo == TipoBlock.POWER)
                        {
                            int aux = random.Next(1, 4);
                            switch (aux)
                            {
                                case 1:
                                    this.listaPowers.Add(new Powers(TipoPower.AUMENTAR, listaTijolos[i].pX, listaTijolos[i].pY, 270, 20));
                                    break;
                                case 2:
                                    this.listaPowers.Add(new Powers(TipoPower.MULTI, listaTijolos[i].pX, listaTijolos[i].pY, 270, 20));
                                    break;
                                case 3:
                                    this.listaPowers.Add(new Powers(TipoPower.BARREIRA, listaTijolos[i].pX, listaTijolos[i].pY, 270, 20));
                                    break;
                            }
                        }
                        this.listaTijolos.Remove(listaTijolos[i]);
                        this.bolas[b].Move();   //volta a colocar a bola em jogo
                    }
                }
            }
            //coloca os powers em movimento quando aparecem
            for(int i = 0; i < listaPowers.Count; i++)
            {
                listaPowers[i].Move();
            }


            //verifica quando o jogador bate nos powers
            //foreach (Powers powers in this.listaPowers)
            for (int i = 0; i < listaPowers.Count; i++)
            {
                if (this.jogador.IntersetaCom(listaPowers[i]) == true)
                {
                    if(listaPowers[i].tipo == TipoPower.AUMENTAR)
                    {
                        this.score = this.score + 50;
                        this.jogador.comprimento = this.jogador.comprimento + 10;
                    }
                    if(listaPowers[i].tipo == TipoPower.MULTI)
                    {
                        this.score = this.score + 50;
                        this.bolas.Add(new Bola(comprimento / 2, altura / 2, 15, 15, 180, 20));
                        this.bolas.Add(new Bola(comprimento / 2, altura / 2, 15, 15, 0, 20));
                        this.bolas.Add(new Bola(comprimento / 2, altura / 2, 15, 15, 90, 20));
                        //this.bola1 = new Bola(this.comprimento/2, this.altura - this.altura/5, 15, 15, 180, 20);
                        //this.bola2 = new Bola(this.comprimento/2, this.altura - this.altura/5, 15, 15, 0, 20);
                        //this.bola3 = new Bola(this.comprimento/2, this.altura - this.altura/5, 15, 15, 90, 20);
                    }
                    if(listaPowers[i].tipo == TipoPower.BARREIRA)
                    {
                        this.score = this.score + 50;
                        this.barreira = new Jogador(this.comprimento/2, this.altura/2, 150, 45, 0);
                    }

                    this.listaPowers.Remove(listaPowers[i]);
                }
            }

            //verifica se as bolas entram na "baliza"
            for (int b = 0; b < bolas.Count; b++)
            {
                if (this.bolas[b].pY > altura)
                {
                    this.bolas.Remove(bolas[b]);
                }
            }

            //se nao houver bolas em jogo termina o jogo
            if (bolas.Count == 0)
            {
                this.status = GameStatus.ENDED;
                return this.status;
            }
            //se não houver tijolos em jogo, o jogador ganha
            if(listaTijolos.Count == 0)
            {
                this.status = GameStatus.WIN;
                return this.status;
            }

            return this.Status;     //retorna o estado do jogo depois de todas as verificações
        }//class update

        //metodo que incorpora todos os componentes do jogo numa só imagem
        public Image RenderFrame()
        {
            if(this.status == GameStatus.NOTREADY)
            {
                return null;
            }

            //pinceis a ser usados
            //SolidBrush brush1 = new SolidBrush(Color.Black);
            SolidBrush brush2 = new SolidBrush(Color.White);
            //SolidBrush brush3 = new SolidBrush(Color.Red);

            //cria imagem onde serão colocados os componentes a mostrar
            //coloca logo o fundo com a imagem
            Image img = new Bitmap(this.fundo, this.comprimento, this.altura);
            Graphics g = Graphics.FromImage(img);

            //fundo - desnecessário com o novo fundo/imagem
            //g.FillRectangle(brush1, 0, 0, this.comprimento, this.altura);
            
            //escreve o score
            Font font = new Font("Courrier New", 10, FontStyle.Bold, GraphicsUnit.Point);
            g.DrawString("Score :  " + this.score, font, brush2, this.comprimento / 10, 20);

            //renderiza bolas e jogador
            for (int b = 0; b < bolas.Count; b++)
            {
                this.bolas[b].Render(g);
            }
            this.jogador.Render(g);

            //renderiza ajudas
            if (this.barreira != null) this.barreira.Render(g);

            //renderiza powers
            for(int i = 0; i < this.listaPowers.Count; i++)
            {
                this.listaPowers[i].Render(g);
            }

            //renderiza tijolos
            for (int i = 0; i < this.listaTijolos.Count; i++)
            {
                this.listaTijolos[i].Render(g);
            }

            //escreve estado do jogo
            switch (this.status)
            {
                case GameStatus.READY:
                    g.DrawString("READY", font, Brushes.BlueViolet, this.comprimento / 2, 40f);
                    break;
                case GameStatus.PAUSE:
                    g.DrawString("PAUSE", font, Brushes.Yellow, this.comprimento / 2, 40f);
                    break;
                case GameStatus.ENDED:
                    g.DrawString("GAME OVER", font, Brushes.Red, this.comprimento / 2 , 40f);
                    break;
                case GameStatus.WIN:
                    g.DrawString("YOU WIN", font, Brushes.Green, this.comprimento / 2, 40f);
                    break;
            }

            //dissolver a memoria fora de utilização
            //brush1.Dispose();
            brush2.Dispose();
            g.Dispose();

            //e finalmente devolver a imagem para ser projetada
            return img;


        }//fim de render

    }//fim da class
}//fim do namespace
