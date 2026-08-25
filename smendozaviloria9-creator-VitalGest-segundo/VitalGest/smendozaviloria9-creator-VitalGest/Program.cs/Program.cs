using VitalGest.Models;
using VitalGest.Services;
using VitalGest.Exceptions;

namespace VitalGest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hola VitalGest");

            List<Paciente> pacientes = new List<Paciente>();
            PacienteService servicio = new PacienteService();

            // TASK 6: un solo LogService para toda la ejecucion, asi todos los
            // errores quedan en el mismo archivo.
            LogService log = new LogService();

            bool salir = false;

            while (!salir)
            {
                try
                {
                    MostrarMenu();
                    string opcion = Console.ReadLine()?.Trim();

                    switch (opcion)
                    {
                        case "1":
                            servicio.RegistrarPaciente(pacientes);
                            break;

                        case "2":
                            servicio.ListarPacientes(pacientes);
                            break;

                        case "3":
                            string nombreBuscar = servicio.LeerTexto("\nIngrese el nombre del paciente a buscar: ");
                            servicio.BuscarPacientePorNombre(pacientes, nombreBuscar);
                            break;

                        case "4":
                            string nombreEliminar = servicio.LeerTexto("\nIngrese el nombre del paciente a eliminar: ");
                            servicio.EliminarPaciente(pacientes, nombreEliminar);
                            break;

                        case "5":
                            string nombreParaMascota = servicio.LeerTexto("\nIngrese el nombre del paciente dueño de la mascota: ");
                            servicio.EliminarMascota(pacientes, nombreParaMascota);
                            break;

                        case "6":
                            EscucharMascotas(pacientes);
                            break;

                        case "7":
                            AtenderMascota(pacientes, servicio);
                            break;

                        case "8":
                            EnviarRecordatorio(pacientes, servicio);
                            break;

                        case "9":
                            CalcularDosisMedicamento(pacientes, servicio, log);
                            break;

                        case "10":
                            salir = true;
                            Console.WriteLine("\nGracias por usar el sistema de VitalGest. Hasta pronto.");
                            break;

                        default:
                            Console.WriteLine("\nOpcion invalida. Por favor selecciona una opcion del 1 al 10.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nOcurrio un error inesperado: {ex.Message}");
                    Console.WriteLine("Por favor intenta nuevamente.");

                    // TASK 6: aparte de mostrarlo en consola, queda guardado en errores.log
                    log.RegistrarError(ex.Message);
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresiona ENTER para continuar...");
                    Console.ReadLine();
                }
            }
        }

        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("      VITALGEST  -  MENU PRINCIPAL");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Registrar paciente (y mascota opcional)");
            Console.WriteLine("2. Listar pacientes");
            Console.WriteLine("3. Buscar paciente por nombre");
            Console.WriteLine("4. Eliminar paciente (con sus mascotas)");
            Console.WriteLine("5. Eliminar una mascota de un paciente");
            Console.WriteLine("6. Escuchar sonidos de todas las mascotas (polimorfismo)");
            Console.WriteLine("7. Atender una mascota (servicio veterinario)");
            Console.WriteLine("8. Enviar recordatorio de cita a un paciente");
            Console.WriteLine("9. Calcular dosis de medicamento por peso");
            Console.WriteLine("10. Salir");
            Console.WriteLine("========================================");
            Console.Write("Seleccione una opcion: ");
        }

        // TASK 5: recorre todas las mascotas de todos los pacientes y llama al mismo
        // método EmitirSonido() en cada una: cada Mascota responde distinto según su Especie.
        static void EscucharMascotas(List<Paciente> pacientes)
        {
            Console.WriteLine("\n--- Sonidos de las mascotas ---");

            var todasLasMascotas = pacientes.SelectMany(p => p.Mascotas).ToList();

            if (todasLasMascotas.Count == 0)
            {
                Console.WriteLine("No hay mascotas registradas todavia.");
                return;
            }

            foreach (Animal mascota in todasLasMascotas)
            {
                Console.WriteLine(mascota.EmitirSonido());
            }
        }

        // TASK 6: usa la clase abstracta ServicioVeterinario a través de sus subclases concretas.
        static void AtenderMascota(List<Paciente> pacientes, PacienteService servicio)
        {
            Console.WriteLine("\n--- Atender una mascota ---");

            var todasLasMascotas = pacientes.SelectMany(p => p.Mascotas).ToList();

            if (todasLasMascotas.Count == 0)
            {
                Console.WriteLine("No hay mascotas registradas todavia.");
                return;
            }

            for (int i = 0; i < todasLasMascotas.Count; i++)
            {
                var m = todasLasMascotas[i];
                Console.WriteLine($"  {i + 1}. {m.Nombre} ({m.Especie}) - dueño: {m.Dueno?.Nombre}");
            }

            int opcion = servicio.LeerNumero("Seleccione el numero de la mascota a atender: ");
            if (opcion < 1 || opcion > todasLasMascotas.Count)
            {
                Console.WriteLine("Opcion invalida.");
                return;
            }

            Mascota mascotaElegida = todasLasMascotas[opcion - 1];

            Console.WriteLine("Tipo de servicio: 1. Consulta general   2. Vacunacion");
            string tipo = servicio.LeerTexto("Seleccione (1/2): ");

            ServicioVeterinario servicioVeterinario = tipo == "2"
                ? new Vacunacion()
                : new ConsultaGeneral();

            servicioVeterinario.Atender(mascotaElegida);
            mascotaElegida.Registrar();
        }

        // TASK 3: usa el metodo EnviarNotificacion() de la interfaz INotificable
        // que ahora implementa Paciente (ademas de IRegistrable).
        static void EnviarRecordatorio(List<Paciente> pacientes, PacienteService servicio)
        {
            Console.WriteLine("\n--- Enviar recordatorio de cita ---");

            if (pacientes.Count == 0)
            {
                Console.WriteLine("No hay pacientes registrados todavia.");
                return;
            }

            string nombre = servicio.LeerTexto("Nombre del paciente: ");
            var paciente = pacientes.FirstOrDefault(p => p.Nombre.Trim().ToLower() == nombre.Trim().ToLower());

            if (paciente == null)
            {
                Console.WriteLine($"No se encontro ningun paciente con el nombre '{nombre}'.");
                return;
            }

            paciente.EnviarNotificacion();
        }

        // TASK 4 y 5: aca se junta el error forzado (division entre cero) para
        // practicar debugging, el manejo con try-catch-finally, la excepcion
        // personalizada MascotaNoEncontradaException y el logging basico.
        static void CalcularDosisMedicamento(List<Paciente> pacientes, PacienteService servicio, LogService log)
        {
            Console.WriteLine("\n--- Calcular dosis de medicamento ---");

            string nombreMascota = servicio.LeerTexto("Nombre de la mascota: ");

            try
            {
                Mascota mascota = servicio.BuscarMascotaPorNombreExacto(pacientes, nombreMascota);

                int miligramos = servicio.LeerNumero("Miligramos totales del medicamento: ");
                int peso = servicio.LeerNumero("Peso de la mascota en kg: ");

                // Punto util para poner un breakpoint: inspeccionar miligramos y
                // peso justo antes de llamar a CalcularDosisPorPeso.
                int dosis = servicio.CalcularDosisPorPeso(miligramos, peso);

                Console.WriteLine($"Dosis recomendada para {mascota.Nombre}: {dosis} mg por kg.");
            }
            catch (MascotaNoEncontradaException ex)
            {
                Console.WriteLine($"\n{ex.Message}");
                log.RegistrarError(ex.Message);
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("\nEl peso no puede ser 0, no se puede calcular la dosis.");
                log.RegistrarError($"Division entre cero al calcular dosis: {ex.Message}");
            }
            finally
            {

                Console.WriteLine("Calculo de dosis finalizado.");
            }
        }
    }
}