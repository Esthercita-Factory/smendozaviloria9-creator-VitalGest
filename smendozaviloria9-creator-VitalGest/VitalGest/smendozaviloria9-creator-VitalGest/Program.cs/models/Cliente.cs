using System.Collections.Generic;

namespace VitalGest.Models
{
    public class Cliente
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public List<Mascota> Mascotas { get; set; } = new List<Mascota>();
    }
}