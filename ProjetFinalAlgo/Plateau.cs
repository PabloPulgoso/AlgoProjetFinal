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


        public Plateau(int taille)
        {

            this.tab = new De[taille, taille];
            this.taille = taille;

            List<Lettre> lettresDispo = new List<Lettre>(Lettre.CreerLettresDisponibles(taille));

            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    tab[i, j] = new De(lettresDispo);
                }
            }

        }

        public string toString()
        {
            string s = "";

            for (int i = 0;i < taille; i++)
            {
                for(int j = 0;j < taille; j++)
                {
                    s += $"{tab[i,j].FaceVisible.Id} ";
                }

                s += "\n";
            }
            return s;
        }

        public void Melanger()
        {
            for (int i= 0; i < taille; i++)
            {
                for (int j= 0; j < taille; j++)
                {
                    tab[i, j].LanceDe();
                }
            }
        }
    }
}
