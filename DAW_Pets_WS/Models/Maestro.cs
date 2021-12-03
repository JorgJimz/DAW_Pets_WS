using System;
using System.Collections.Generic;

#nullable disable

namespace DAW_Pets_WS.Models
{
    public partial class Maestro
    {
        public Maestro()
        {
            Solicitud = new HashSet<Solicitud>();
            Vacuna = new HashSet<Vacuna>();
        }

        public int Id { get; set; }
        public string Grupo { get; set; }
        public string Valor { get; set; }
        public string Atributo1 { get; set; }
        public string Atributo2 { get; set; }
        public string Atributo3 { get; set; }

        public virtual ICollection<Solicitud> Solicitud { get; set; }
        public virtual ICollection<Vacuna> Vacuna { get; set; }
    }
}
