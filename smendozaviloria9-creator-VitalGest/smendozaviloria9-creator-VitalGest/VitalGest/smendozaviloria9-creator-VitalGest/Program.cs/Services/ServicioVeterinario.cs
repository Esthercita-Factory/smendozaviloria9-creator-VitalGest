using VitalGest.Models;
using VitalGest.Interfaces;

namespace VitalGest.Services
{

    public abstract class ServicioVeterinario : IAtendible
    {
        public string NombreServicio { get; set; }

        protected ServicioVeterinario(string nombreServicio)
        {
            NombreServicio = nombreServicio;
        }

        public abstract void Atender(Mascota mascota);
    }
}