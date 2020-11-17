using System;
using System.Drawing;
using System.Resources;

namespace DJD_Bricks
{
    public class Bola : GameObject
    {
        //construtor
        public Bola(int px, int py, int comp, int alt, float dir, float veloc)
            : base(px, py, comp, alt, dir, veloc)
        {
            this.image = Properties.Resources.Ball as Image;
        }

        //metodo de movimento do objeto
        public override void Move()
        {
            this.pX = this.pX + this.velocidade * (float)Math.Cos(this.direcao * Math.PI / 180.0);
            this.pY = this.pY + this.velocidade * (float)Math.Sin(-this.direcao * Math.PI / 180.0);     // o y é negativo porque "cresce" para baixo
        }

        //metodo que desenha o objecto
        public override void Render(Graphics g)
        {
            //g.FillEllipse(new SolidBrush(this.cor), this.pX - this.comprimento / 2, this.pY - this.altura / 2, this.comprimento, this.altura);
            g.DrawImage(this.image, this.pX, this.pY, this.comprimento, this.altura);
        }

    }//fim da class
}//fim do namespace
