using System;
using System.Drawing;
using System.Resources;

namespace DJD_Bricks
{
    public enum TipoBlock { NORMAL = 0, POWER };

    public class Tijolos : GameObject
    {
        public TipoBlock tipo;

        //construtor da class Tijolos
        public Tijolos(TipoBlock tipo, int px, int py, int comp, int alt, float dir, float velo)
        : base(px, py, comp, alt, dir, velo)
        {
            if(tipo == TipoBlock.NORMAL)
            {
                this.image = Properties.Resources.Block as Image;
            }
            else
            {
                this.image = Properties.Resources.Block2 as Image;
            }

            this.tipo = tipo;
            this.velocidade = velo;
            this.direcao = dir;

        }

        public override void Render(Graphics g)
        {
            g.DrawImage(this.image, this.pX, this.pY, this.comprimento, this.altura);
        }

    }//fim da class
}//fim do namespace
