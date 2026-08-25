namespace VitalGest.Exceptions
{
    // TASK 5: excepcion propia del dominio de la clinica. En vez de devolver null
    // cuando no se encuentra una mascota, se lanza esto para que quien llama el
    // metodo sepa exactamente que fallo y por que, sin adivinar.
    public class MascotaNoEncontradaException : Exception
    {
        public MascotaNoEncontradaException(string mensaje) : base(mensaje)
        {
        }
    }
}