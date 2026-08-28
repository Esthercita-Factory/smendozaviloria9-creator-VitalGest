namespace VitalGest.Services
{
    public class ProcesosClinicosService
    {
        public async Task CargarHistorialClinicoAsync(string nombreMascota)
        {
            Console.WriteLine($"Cargando historial clinico de {nombreMascota}...");
            await Task.Delay(2000);
            Console.WriteLine($"Historial clinico de {nombreMascota} listo.");
        }

        public async Task AgendarCitaAsync(string nombrePaciente)
        {
            Console.WriteLine($"Agendando cita para {nombrePaciente}...");
            await Task.Delay(3000);
            Console.WriteLine($"Cita agendada para {nombrePaciente}.");
        }

        public async Task EnviarNotificacionAsync(string nombrePaciente)
        {
            Console.WriteLine($"Enviando notificacion a {nombrePaciente}...");
            await Task.Delay(1000);
            Console.WriteLine($"Notificacion enviada a {nombrePaciente}.");
        }

        public async Task EjecutarProcesosCompletosAsync(string nombrePaciente, string nombreMascota)
        {
            Console.WriteLine("\n--- Ejecutando procesos de la clinica en paralelo ---");

            var tareaHistorial = Task.Run(() => CargarHistorialClinicoAsync(nombreMascota));
            var tareaCita = Task.Run(() => AgendarCitaAsync(nombrePaciente));
            var tareaNotificacion = Task.Run(() => EnviarNotificacionAsync(nombrePaciente));

            await Task.WhenAll(tareaHistorial, tareaCita, tareaNotificacion);

            Console.WriteLine("\nTodos los procesos terminaron.");
        }

        public async Task CompararWhenAllYWhenAnyAsync(List<string> nombresMascotas)
        {
            Console.WriteLine("\n--- Registrando varias mascotas en paralelo ---");

            var random = new Random();
            var tareas = new List<Task<string>>();

            foreach (var nombre in nombresMascotas)
            {
                tareas.Add(RegistrarConDelayAsync(nombre, random.Next(1000, 4000)));
            }

            var primeraEnTerminar = await Task.WhenAny(tareas);
            Console.WriteLine($"\n(WhenAny) La primera en terminar fue: {primeraEnTerminar.Result}");

            var resultados = await Task.WhenAll(tareas);
            Console.WriteLine("\n(WhenAll) Todas las mascotas terminaron de registrarse:");
            foreach (var resultado in resultados)
            {
                Console.WriteLine($"  - {resultado}");
            }
        }

        private async Task<string> RegistrarConDelayAsync(string nombreMascota, int milisegundos)
        {
            await Task.Delay(milisegundos);
            return $"{nombreMascota} (tardo {milisegundos} ms)";
        }
    }
}