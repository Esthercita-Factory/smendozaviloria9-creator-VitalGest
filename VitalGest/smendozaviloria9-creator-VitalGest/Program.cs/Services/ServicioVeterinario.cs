using VitalGest.Models;

namespace VitalGest.Services
{
    // TASK 6: clase abstracta. No se puede hacer "new ServicioVeterinario()" directo.
    public abstract class ServicioVeterinario
    {
        public string NombreServicio { get; set; }

        protected ServicioVeterinario(string nombreServicio)
        {
            NombreServicio = nombreServicio;
        }

        public abstract void Atender(Mascota mascota);
    }
}