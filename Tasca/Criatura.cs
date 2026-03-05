using System;
using System.Collections.Generic;

namespace PeixeraVirtual;

public abstract class Criatura
    {
        public Especie Especie;
        public Sexe Sexe;
        public Posicio Posicio;
        public bool Viva;

        public Criatura(Especie especie, Sexe sexe, Posicio posicio)
        {
            Especie = especie;
            Sexe = sexe;
            Posicio = posicio;
            Viva = true;
        }

        public void Mor()
        {
            Viva = false;
        }

        public abstract void Mou(Peixera peixera);

        public virtual void CanviaDireccio()
        {
            // per defecte no fa res
        }
    }