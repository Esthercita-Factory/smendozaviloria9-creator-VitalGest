using VitalGest.Models;
using VitalGest.Services;
using VitalGest.Exceptions;

namespace VitalGest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hola VitalGest");

            List<Paciente> pacientes = new List<Paciente>();
            PacienteService servicio = new PacienteService();
            ProcesosClinicosService procesos = new ProcesosClinicosService();

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
                            await servicio.RegistrarPacienteAsync(pacientes);
                            break;

                        case "11":
                            await EjecutarProcesosParalelos(pacientes, servicio, procesos);
                            break;

                        case "12":
                            await RegistrarVariasMascotas(servicio, procesos);
                            break;

                        case "13":
                            salir = true;
                            Console.WriteLine("\nGracias por usar el sistema de VitalGest. Hasta pronto.");
                            break;

                        default:
                            Console.WriteLine("\nOpcion invalida. Por favor selecciona una opcion del 1 al 13.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nOcurrio un error inesperado: {ex.Message}");
                    Console.WriteLine("Por favor intenta nuevamente.");

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
            Console.WriteLine("1. Registrar paciente y mascota ");
            Console.WriteLine("2. Listar pacientes");
            Console.WriteLine("3. Buscar paciente por nombre");
            Console.WriteLine("4. Eliminar paciente (con sus mascotas)");
            Console.WriteLine("5. Eliminar una mascota de un paciente");
            Console.WriteLine("6. Escuchar sonidos de todas las mascotas ");
            Console.WriteLine("7. Atender una mascota ");
            Console.WriteLine("8. Enviar recordatorio de cita a un paciente");
            Console.WriteLine("9. Calcular dosis de medicamento por peso");
            Console.WriteLine("10. Registrar paciente ");
            Console.WriteLine("11. procesos de la clinica");
            Console.WriteLine("12. Registrar varias mascotas a la vez ");
            Console.WriteLine("13. Salir");
            Console.WriteLine("========================================");
            Console.Write("Seleccione una opcion: ");
        }

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

        static void CalcularDosisMedicamento(List<Paciente> pacientes, PacienteService servicio, LogService log)
        {
            Console.WriteLine("\n--- Calcular dosis de medicamento ---");

            string nombreMascota = servicio.LeerTexto("Nombre de la mascota: ");

            try
            {
                Mascota mascota = servicio.BuscarMascotaPorNombreExacto(pacientes, nombreMascota);

                int miligramos = servicio.LeerNumero("Miligramos totales del medicamento: ");
                int peso = servicio.LeerNumero("Peso de la mascota en kg: ");

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

        static async Task EjecutarProcesosParalelos(List<Paciente> pacientes, PacienteService servicio, ProcesosClinicosService procesos)
        {
            if (pacientes.Count == 0)
            {
                Console.WriteLine("No hay pacientes registrados todavia.");
                return;
            }

            string nombrePaciente = servicio.LeerTexto("Nombre del paciente: ");
            string nombreMascota = servicio.LeerTexto("Nombre de la mascota: ");

            await procesos.EjecutarProcesosCompletosAsync(nombrePaciente, nombreMascota);
        }

        static async Task RegistrarVariasMascotas(PacienteService servicio, ProcesosClinicosService procesos)
        {
            Console.WriteLine("\nIngrese los nombres de las mascotas separados por coma (ej: Firulais, Michi, Rocky):");
            string entrada = servicio.LeerTexto("Mascotas: ");

            var nombres = entrada.Split(',').Select(n => n.Trim()).Where(n => n.Length > 0).ToList();

            if (nombres.Count == 0)
            {
                Console.WriteLine("No se ingreso ninguna mascota.");
                return;
            }

            await procesos.CompararWhenAllYWhenAnyAsync(nombres);
        }
    }
}