using System;
using System.Collections.Generic;

namespace PeixeraVirtual;

public class Tortuga : CriaturaAmbDireccio
    {
        public Tortuga(Sexe sexe, Posicio posicio, Direccio direccio)
            : base(Especie.Tortuga, sexe, posicio, direccio)
        {
        }
    }