using System;
using System.Drawing;
using System.Resources;

namespace DJD_Bricks
{
    public abstract class GameObject
    {
        public float pX;
        public float pY;
        public int comprimento;
        public int altura;
        public float direcao;
        public float velocidade;

        protected Image image = null;


        //construtor base
        protected GameObject(float px, float py, int comp, int alt, float dir, float veloc)
        {
            this.pX = px;
            this.pY = py;
            this.comprimento = comp;
            this.altura = alt;
            this.velocidade = veloc;
            this.direcao = dir;
        }

        //base do metodo movimento da cada game object
        public virtual void Move()
        {

        }

        //base do metodo de render de cada game object
        public virtual void Render(Graphics g)
        {

        }

        //detecta as colisões
        public virtual bool IntersetaCom(GameObject other)
        {
            if (other.pX - other.comprimento / 2 < this.pX + this.comprimento / 2 &&
             this.pX - this.comprimento / 2 < other.pX + other.comprimento / 2 &&
               other.pY - other.altura / 2 < this.pY + this.altura / 2 &&
                   this.pY - this.altura / 2 < other.pY + other.altura / 2)
            {
                return true;
            }
            else return false;
        }
    }//fim da class
}//fim do namespace
