using VitalGest.Interfaces;

namespace VitalGest.Models
{
    // TASK 2 + 6: Paciente implementa IRegistrable.
    public class Paciente : IRegistrable
    {
        // TASK 4: campo privado, solo accesible mediante la propiedad Telefono.
        private string telefono;

        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }

        // TASK 4: dato sensible. Se puede leer libremente (get público),
        // pero solo se puede modificar desde dentro de esta clase (set private).
        // Para cambiarlo desde afuera hay que usar ActualizarTelefono().
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

        // TASK 6: implementación de IRegistrable.
        public void Registrar()
        {
            Console.WriteLine($"[Registro] Paciente '{Nombre}' registrado en el sistema.");
        }
    }
}