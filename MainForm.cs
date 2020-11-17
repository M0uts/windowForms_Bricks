using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DJD_Bricks
{
    public partial class MainForm : Form
    {
        JogoBricks jogo = null;

        public MainForm()
        {
            InitializeComponent();
            this.TimerLoop.Interval = 20;
            jogo = new JogoBricks(this.pictureBox1.Width, pictureBox1.Height, 0);
            this.pictureBox1.Image = jogo.RenderFrame();
        }

        

        /// <summary>
        /// Evento que esconde o menu e inicia o jogo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BotaoPlayClick(object sender, EventArgs e)
        {
            jogo = new JogoBricks(this.pictureBox1.Width, pictureBox1.Height, 1);
            this.groupBox1.Visible = false;     //esconde o menu
            this.jogo.Start();                 //começa o jogo
            this.TimerLoop.Start();           //começa o loop de jogo
        }

        /// <summary>
        /// Evento que fecha a aplicação
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButaoExitClick(object sender, EventArgs e)
        {
            DialogResult certeza = new DialogResult();
            certeza = MessageBox.Show("Tem a certeza que quer fechar o jogo?", "Fechar Jogo", MessageBoxButtons.YesNo);
            if(certeza == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        //quando se fecha a aplicação com o X , termina o jogo
        private void FechoManual(object sender, FormClosingEventArgs e)
        {
            if(this.jogo != null)
            {
                this.jogo.Stop();
            }
        }

        private void MainFormTeclas(object sender, KeyPressEventArgs e)
        {
            if(this.jogo == null)
            {
                return;
            }
            if(e.KeyChar == 'p' || e.KeyChar == 'P' && this.jogo.Status != JogoBricks.GameStatus.ENDED)
            {
                this.jogo.Pausa();
                this.TimerLoop.Start();
            }
            if(this.pictureBox1.Image != null)
            {
                this.pictureBox1.Image.Dispose();
            }
            this.pictureBox1.Image = jogo.RenderFrame();
        }

        //Loop de jogo em timer
        private void TimerLoopTick(object sender, EventArgs e)
        {
            this.TimerLoop.Start();
            jogo.ProcessInputs();
            jogo.Update();

            //verificações gerais
            if(this.jogo.Status == JogoBricks.GameStatus.PAUSE)
            {
                this.TimerLoop.Stop();      //pausa
            }

            if(this.jogo.Status == JogoBricks.GameStatus.ENDED)
            {
                this.TimerLoop.Stop();      //pausa
                this.groupBox1.Visible = true;  //mostra o menu
            }

            if (this.jogo.Status == JogoBricks.GameStatus.WIN)
            {
                this.TimerLoop.Stop();      //pausa
                this.groupBox1.Visible = true;  //mostra o menu
            }

            if (this.pictureBox1.Image != null)
            {
                this.pictureBox1.Image.Dispose();
            }

            this.pictureBox1.Image = jogo.RenderFrame();
        }

        
    }//fim da class mainform
}//fim do namespace
