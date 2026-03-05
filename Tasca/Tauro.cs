using System;
using System.Collections.Generic;

namespace PeixeraVirtual;


public class Tauro : CriaturaAmbDireccio
    {
        public int Edat;

        public Tauro(Sexe sexe, Posicio posicio, Direccio direccio)
            : base(Especie.Tauro, sexe, posicio, direccio)
        {
            Edat = 0;
        }

        public void EnvelleixUnaRonda()
        {
            Edat++;
            if (Edat >= 75)
            {
                Mor();
            }
        }
    }