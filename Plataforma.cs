using System;
using System.Drawing;

namespace DJD_Bricks
{
    public abstract class Plataforma : GameObject
    {
        protected int veloInicial = 24;

        //construtor
        protected Plataforma(int px, int py, int comp, int alt, float dir)
        : base(px, py, comp, alt, dir, 0)
        {

        }

        //metodo que move a plataforma
        public override void Move()
        {
            //mover na horizontal
            this.pX = this.pX + this.velocidade * (float)Math.Cos(-this.direcao * Math.PI / 180.0);
        }

        //metodo que desenha a plataforma
        public override void Render(Graphics g)
        {
            //g.FillRectangle(new SolidBrush(this.cor), this.pX - this.comprimento / 2, this.pY - this.altura / 2, this.comprimento, this.altura);
            g.DrawImage(this.image, this.pX - this.comprimento / 2, this.pY - this.altura/2, this.comprimento, this.altura/2);
        }
        
        //player input
        public virtual void PlayerInput(float cx)
        {

        }
    }//fim da class
}//fim do namespace
