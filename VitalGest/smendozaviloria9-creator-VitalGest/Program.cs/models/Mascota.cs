using VitalGest.Interfaces;

namespace VitalGest.Models
{
    // TASK 5: Mascota hereda de Animal (es-un Animal) y agrega Raza + Dueno.
    // TASK 6: implementa IRegistrable.
    public class Mascota : Animal, IRegistrable
    {
        public string Raza { get; set; }
        public string Sintoma { get; set; }

        // Referencia al paciente dueño (se completa al agregarla con Paciente.AgregarMascota).
        public Paciente Dueno { get; set; }

        public Mascota(string nombre, int edad, string especie, string raza, string sintoma)
            : base(nombre, edad, especie)
        {
            Raza = raza;
            Sintoma = sintoma;
        }

        // TASK 5: mismo método que en Animal, comportamiento distinto según la especie.
        public override string EmitirSonido()
        {
            switch (Especie.Trim().ToLower())
            {
                case "perro":
                    return $"{Nombre} dice: ¡Guau!";
                case "gato":
                    return $"{Nombre} dice: ¡Miau!";
                case "ave":
                    return $"{Nombre} dice: ¡Pío!";
                default:
                    return $"{Nombre} hace un sonido de {Especie}.";
            }
        }

        // TASK 6: implementación de IRegistrable.
        public void Registrar()
        {
            Console.WriteLine($"[Registro] Mascota '{Nombre}' registrada en el sistema.");
        }
    }
}