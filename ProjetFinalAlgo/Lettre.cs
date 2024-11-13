using System;
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

        public Lettre(Lettre l)
        {
            this.poids = l.poids;
            this.id = l.id;
            this.quantite= l.quantite;
        }

        /// <summary>
        /// Fait un tableau rempli de lettres en fonction de la taille du plateau
        /// </summary>
        /// <param name="frequences">Tableau de lettres qui a le nombre de lettres ajusté à leur fréquence</param>
        /// <param name="tailleTab">taille d'un côté du plateau de jeu</param>
        /// <returns></returns>
        public static Lettre[] CreerPossibilitesLettres(Lettre[] frequences, int tailleTab)
        {
            Lettre[] dispo = new Lettre[(tailleTab * tailleTab)*6];

            Random random = new Random();

            for (int i = 0; i<dispo.Length; i++)
            {

                dispo[i] = frequences[random.Next(100)];

            }

            return dispo;

        }
        
        /// <summary>
        /// Créer un tableau contenant une lettre un certain nombre de fois en fonction de sa quantité
        /// </summary>
        /// <param name="lettres">Contient la lettre, sa quantité et son poids</param>
        /// <returns>Un tableau comprenant 100 lettres</returns>
        public static Lettre[] Ponderation(Lettre[] lettres )
        {
            //créer un tableau sur 100
            Lettre[] tableauLettres = new Lettre[100]; 

            //compteur pour parcourir le tableauLettres
            int compteur = 0;

            //boucle parcourant le tableau en paramètre
            for(int i=0; i<lettres.Length; i++)
            {
                //boucle en fonction du nombre de lettre dont nous avons besoin
                for(int j = 0; j < lettres[i].quantite; j++)
                {
                    
                    tableauLettres[compteur] = new Lettre(lettres[i]);
                    tableauLettres[compteur].quantite = 1;
                    compteur++;
                }
            }
            return tableauLettres;

        }

        /// <summary>
        /// Renvoie un tableau avec les lettres disponibles dans le jeu, ajusté à la taille du plateau.
        /// </summary>
        /// <param name="taille">Taille du plateau</param>
        /// <returns>Tableau de Lettre</returns>
        public static Lettre[] CreerLettresDisponibles(int taille)
        {
            Lettre[] l = Program.CreerLettres();
            Lettre[] centLettres = Lettre.Ponderation(l);
            Lettre[] t = Lettre.CreerPossibilitesLettres(centLettres, taille);

            return t;
        }
    }
}
