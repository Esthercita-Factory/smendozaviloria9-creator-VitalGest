using VitalGest.Models;

namespace VitalGest.Services
{
    public class ConsultaGeneral : ServicioVeterinario
    {
        public ConsultaGeneral() : base("Consulta general") { }

        public override void Atender(Mascota mascota)
        {
            Console.WriteLine($"[{NombreServicio}] Revisando el estado general de {mascota.Nombre}. Síntoma reportado: {mascota.Sintoma}.");
        }
    }
}