using VitalGest.Models;

namespace VitalGest.Services
{
    public class Vacunacion : ServicioVeterinario
    {
        public Vacunacion() : base("Vacunación") { }

        public override void Atender(Mascota mascota)
        {
            Console.WriteLine($"[{NombreServicio}] Aplicando vacuna a {mascota.Nombre} ({mascota.Especie}, {mascota.Raza}). ¡Listo!");
        }
    }
}