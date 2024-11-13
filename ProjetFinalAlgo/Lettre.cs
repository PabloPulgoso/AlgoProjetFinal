﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        public Lettre[] CreerPossibilitesLettres(Lettre[] frequences, int tailleTab)
        {
            Lettre[] dispo = new Lettre[(tailleTab * tailleTab)*6];

            Random random = new Random();

            for (int i = 0; i<dispo.Length; i++)
            {

                dispo[i] = frequences[random.Next(100)];

            }

            return dispo;

        }
        


    }
}
