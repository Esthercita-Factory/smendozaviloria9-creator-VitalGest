namespace VitalGest.Models
{
    public class Animal
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }

        public string Especie { get; protected set; }

        public Animal(string nombre, int edad, string especie)
        {
            Nombre = nombre;
            Edad = edad;
            Especie = especie;
        }

        public virtual string EmitirSonido()
        {
            return $"{Nombre} hace un sonido.";
        }
    }
}