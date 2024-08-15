using System;

namespace SPG_Fachtheorie.Aufgabe2.Model
{
    public abstract class Entity<Tkey> where Tkey : struct
    {
        public Tkey Id { get; private set; }
        public Guid Guid { get; set; }
    }
}