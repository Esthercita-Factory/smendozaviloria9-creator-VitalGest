using VitalGest.Interfaces;

namespace VitalGest.Models
{
 
    public class Paciente : IRegistrable, INotificable
    {
        private string telefono;

        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }


        public string Telefono
        {
            get { return telefono; }
            private set { telefono = value; }
        }

        public List<Mascota> Mascotas { get; set; } = new List<Mascota>();

        public Paciente(string nombre, string direccion, string telefono)
        {
            Id = Guid.NewGuid().ToString();
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
        }

        public void ActualizarTelefono(string nuevoTelefono)
        {
            Telefono = nuevoTelefono;
        }

        public void AgregarMascota(Mascota mascota)
        {
            mascota.Dueno = this;
            Mascotas.Add(mascota);
        }

        public void MostrarInformacion()
        {
            Console.WriteLine($"ID: {Id} | Nombre: {Nombre} | Dirección: {Direccion} | Teléfono: {Telefono}");
        }

       
        public void Registrar()
        {
            Console.WriteLine($"[Registro] Paciente '{Nombre}' registrado en el sistema.");
        }
        public void EnviarNotificacion()
        {
            Console.WriteLine($"[Notificacion] Recordatorio de cita enviado a {Nombre} (tel: {Telefono}).");
        }
    }
}