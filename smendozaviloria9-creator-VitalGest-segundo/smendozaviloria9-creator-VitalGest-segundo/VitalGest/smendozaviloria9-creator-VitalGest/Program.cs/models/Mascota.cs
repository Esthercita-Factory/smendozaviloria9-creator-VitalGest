using VitalGest.Interfaces;

namespace VitalGest.Models
{
    
    public class Mascota : Animal, IRegistrable
    {
        public string Raza { get; set; }
        public string Sintoma { get; set; }

        public Paciente Dueno { get; set; }

        public Mascota(string nombre, int edad, string especie, string raza, string sintoma)
            : base(nombre, edad, especie)
        {
            Raza = raza;
            Sintoma = sintoma;
        }

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

        public void Registrar()
        {
            Console.WriteLine($"[Registro] Mascota '{Nombre}' registrada en el sistema.");
        }
    }
}