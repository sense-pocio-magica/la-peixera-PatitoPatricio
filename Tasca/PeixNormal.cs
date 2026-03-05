using System;
using System.Collections.Generic;

namespace PeixeraVirtual;


public class PeixNormal : CriaturaAmbDireccio
    {
        public PeixNormal(Sexe sexe, Posicio posicio, Direccio direccio)
            : base(Especie.PeixNormal, sexe, posicio, direccio)
        {
        }
    }