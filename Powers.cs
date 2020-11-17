using System;
using System.Drawing;
using System.Resources;

namespace DJD_Bricks
{
    public enum TipoPower { MULTI = 0, AUMENTAR, BARREIRA };

    public class Powers : GameObject
    {

        public TipoPower tipo;

        //construtor da class powers
        public Powers(TipoPower tipo, float px, float py, float dir, float velo)
            : base(px, py, 15, 15, dir, velo)
        {
            this.image = Properties.Resources.Ball2 as Image;
            this.tipo = tipo;
            this.velocidade = velo;
            this.direcao = dir;

        }

        public override void Move()
        {
            this.pY = this.pY + this.velocidade * (float)Math.Sin(-this.direcao * Math.PI / 180.0);
        }

        public override void Render(Graphics g)
        {
            g.DrawImage(this.image, this.pX, this.pY, this.comprimento, this.altura);
        }

    }//fim da class
}//fim do namespace
