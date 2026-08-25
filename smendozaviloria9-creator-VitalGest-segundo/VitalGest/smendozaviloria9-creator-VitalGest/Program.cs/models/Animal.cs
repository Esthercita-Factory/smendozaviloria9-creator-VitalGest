namespace VitalGest.Models
{
    // TASK 5: clase base. Mascota va a heredar de aquí.
    public class Animal
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }

        // protected: se puede leer desde afuera (get público),
        // pero solo Animal y sus clases hijas pueden asignarlo directamente.
        public string Especie { get; protected set; }

        public Animal(string nombre, int edad, string especie)
        {
            Nombre = nombre;
            Edad = edad;
            Especie = especie;
        }

        // virtual: las clases hijas pueden sobrescribir este método (polimorfismo).
        public virtual string EmitirSonido()
        {
            return $"{Nombre} hace un sonido.";
        }
    }
}