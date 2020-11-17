using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Resources;

namespace DJD_Bricks
{
    public class Jogador : Plataforma
    {
        //definição das teclas
        private Keys keyLeft = Keys.Left;
        private Keys keyRight = Keys.Right;

        //construtor da class jogador
        public Jogador(int px, int py, int comp, int alt, float dir) 
		: base(px, py, comp, alt, dir)
        {
            this.image = Properties.Resources.Paddle as Image;
        }

        //deteta se as teclas estão a ser precionadas com o programa aberto
        [DllImport("user32.dll")]
        public extern static Int16 GetKeyState(Int16 nVirtKey);

        private static bool IsKeyDown(Keys key)
        {
            return (GetKeyState(Convert.ToInt16(key)) & 0X80) == 0X80;
        }

        //processamento do input do jogador
        public override void PlayerInput(float cx)
        {
            if (IsKeyDown(this.keyRight))
            {
                this.direcao = 0f;
                this.velocidade = this.veloInicial;
                return;
            }
            if (IsKeyDown(this.keyLeft))
            {
                this.direcao = 180f;
                this.velocidade = this.veloInicial;
                return;
            }
            this.velocidade = 0;
        }
    }//fim da class
}//fim do namespace
