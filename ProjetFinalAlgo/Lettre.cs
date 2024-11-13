﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetFinal
{
    internal class Lettre
    {
        private int quantite;
        private int poids;
        private char id;

        public int Quantite
        {
            get { return quantite; }
            set { quantite = value; }
        }

        public int Poids
        {
            get { return poids; }
        }

        public char Id
        {
            get { return id; }
        }

        public Lettre(char id, int quantite, int poids)
        {
            this.id = id;
            this.quantite = quantite;
            this.poids = poids;

        }

        public Lettre[] Ponderation(Lettre[] lettres )
        {
            Lettre[] tableauLettres = new Lettre[100];
            int compteur = 0;
            for(int i=0; i<tableauLettres.Length; i++)
            {
                for(int j = 0; j < lettres[i].quantite; j++)
                {
                    tableauLettres[compteur] = lettres[i];
                    tableauLettres[compteur].quantite = 1;
                    compteur++;
                }
            }
            return tableauLettres;

        }


    }
}
