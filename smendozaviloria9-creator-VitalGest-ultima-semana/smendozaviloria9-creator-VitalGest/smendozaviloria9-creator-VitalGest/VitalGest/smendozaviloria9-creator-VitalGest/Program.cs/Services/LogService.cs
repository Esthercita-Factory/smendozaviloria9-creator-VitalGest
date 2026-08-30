namespace VitalGest.Services
{

    public class LogService
    {
        private readonly string rutaArchivo = "errores.log";

        public void RegistrarError(string mensaje)
        {
            string linea = $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] {mensaje}";

            try
            {
                File.AppendAllText(rutaArchivo, linea + Environment.NewLine);
            }
            catch
            {
                Console.WriteLine("(No se pudo guardar el log en el archivo errores.log)");
            }
        }
    }
}