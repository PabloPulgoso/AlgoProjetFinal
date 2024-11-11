using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Plateau
    {
        private int taille;
        private De[,] tab; 

        public int Taille
        {
            get { return taille; }
        }


        public Plateau(De[,] tab)
        {
            if ( tab.GetLength(0)!= tab.GetLength(1))
            {
                throw new ArgumentException("Le plateu n'est pas carré.");
            }
            this.taille  = tab.GetLength(0);
            this.tab = tab;
        }


    }
}
