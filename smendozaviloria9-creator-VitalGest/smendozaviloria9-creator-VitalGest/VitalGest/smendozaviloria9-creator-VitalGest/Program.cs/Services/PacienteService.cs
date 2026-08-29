using VitalGest.Models;
using VitalGest.Exceptions;

namespace VitalGest.Services
{
    public class PacienteService
    {
        public string LeerTexto(string mensaje)
        {
            string valor;
            do
            {
                Console.Write(mensaje);
                valor = Console.ReadLine();
                valor = valor?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(valor))
                {
                    Console.WriteLine("Este campo no puede estar vacio. Intenta nuevamente.");
                }
            } while (string.IsNullOrWhiteSpace(valor));

            return valor;
        }

        public int LeerNumero(string mensaje)
        {
            int valor;
            bool esValido;

            do
            {
                Console.Write(mensaje);
                string entrada = Console.ReadLine()?.Trim();

                try
                {
                    valor = int.Parse(entrada);

                    if (valor < 0)
                    {
                        Console.WriteLine("El valor no puede ser negativo. Intenta nuevamente.");
                        esValido = false;
                    }
                    else
                    {
                        esValido = true;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Debes ingresar solo numeros, sin letras ni simbolos. Intenta nuevamente.");
                    valor = 0;
                    esValido = false;
                }
                catch (OverflowException)
                {
                    Console.WriteLine("El numero ingresado es demasiado grande. Intenta nuevamente.");
                    valor = 0;
                    esValido = false;
                }
            } while (!esValido);

            return valor;
        }

        public bool LeerConfirmacion(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string respuesta = Console.ReadLine()?.Trim().ToLower();

                if (respuesta == "s" || respuesta == "si")
                    return true;

                if (respuesta == "n" || respuesta == "no")
                    return false;

                Console.WriteLine("Por favor responde 's' (si) o 'n' (no).");
            }
        }

        private string Normalizar(string texto)
        {
            return texto.Trim().ToLower();
        }

        public void RegistrarPaciente(List<Paciente> lista)
        {
            Console.WriteLine("\n--- Registrar nuevo paciente ---");

            string nombre = LeerTexto("Nombre del paciente: ");
            string direccion = LeerTexto("Direccion del paciente: ");
            string telefono = LeerTexto("Telefono del paciente: ");

            var paciente = new Paciente(nombre, direccion, telefono);

            bool quiereMascota = LeerConfirmacion("Desea registrar una mascota para este paciente? (s/n): ");

            while (quiereMascota)
            {
                string nombreMascota = LeerTexto("  Nombre de la mascota: ");
                string especie = LeerTexto("  Especie (perro, gato, ave, etc.): ");
                int edad = LeerNumero("  Edad de la mascota: ");
                string raza = LeerTexto("  Raza de la mascota: ");
                string sintoma = LeerTexto("  Sintoma o motivo de consulta: ");

                var mascota = new Mascota(nombreMascota, edad, especie, raza, sintoma);
                paciente.AgregarMascota(mascota);

                quiereMascota = LeerConfirmacion("Desea agregar otra mascota? (s/n): ");
            }

            lista.Add(paciente);

            Console.WriteLine($"\nPaciente '{paciente.Nombre}' registrado con exito. ID asignado: {paciente.Id}");
        }

        public async Task RegistrarPacienteAsync(List<Paciente> lista)
        {
            Console.WriteLine("\n--- Registrar nuevo paciente (async) ---");

            string nombre = LeerTexto("Nombre del paciente: ");
            string direccion = LeerTexto("Direccion del paciente: ");
            string telefono = LeerTexto("Telefono del paciente: ");

            var paciente = new Paciente(nombre, direccion, telefono);

            bool quiereMascota = LeerConfirmacion("Desea registrar una mascota para este paciente? (s/n): ");

            while (quiereMascota)
            {
                string nombreMascota = LeerTexto("  Nombre de la mascota: ");
                string especie = LeerTexto("  Especie (perro, gato, ave, etc.): ");
                int edad = LeerNumero("  Edad de la mascota: ");
                string raza = LeerTexto("  Raza de la mascota: ");
                string sintoma = LeerTexto("  Sintoma o motivo de consulta: ");

                var mascota = new Mascota(nombreMascota, edad, especie, raza, sintoma);
                paciente.AgregarMascota(mascota);

                quiereMascota = LeerConfirmacion("Desea agregar otra mascota? (s/n): ");
            }

            Console.WriteLine("\nGuardando paciente, un momento...");
            await Task.Delay(2000);

            lista.Add(paciente);

            Console.WriteLine($"Paciente '{paciente.Nombre}' registrado con exito. ID asignado: {paciente.Id}");
        }

        public void ListarPacientes(List<Paciente> lista)
        {
            Console.WriteLine("\n--- Lista de pacientes ---");

            if (lista.Count == 0)
            {
                Console.WriteLine("No hay pacientes registrados todavia.");
                return;
            }

            foreach (var paciente in lista)
            {
                MostrarPaciente(paciente);
            }
        }

        public void BuscarPacientePorNombre(List<Paciente> lista, string nombre)
        {
            string nombreBuscado = Normalizar(nombre);

            var encontrados = lista
                .Where(p => Normalizar(p.Nombre).Contains(nombreBuscado))
                .ToList();

            if (encontrados.Count == 0)
            {
                Console.WriteLine($"\nNo se encontro ningun paciente con el nombre '{nombre}'.");
                return;
            }

            Console.WriteLine($"\n--- Resultado(s) de la busqueda: '{nombre}' ---");
            foreach (var paciente in encontrados)
            {
                MostrarPaciente(paciente);
            }
        }

        public bool EliminarPaciente(List<Paciente> lista, string nombre)
        {
            var paciente = SeleccionarPacientePorNombre(lista, nombre, "eliminar");

            if (paciente == null)
                return false;

            lista.Remove(paciente);
            Console.WriteLine($"\nPaciente '{paciente.Nombre}' (ID {paciente.Id}) eliminado correctamente, junto con sus mascotas.");
            return true;
        }

        public bool EliminarMascota(List<Paciente> lista, string nombrePaciente)
        {
            var paciente = SeleccionarPacientePorNombre(lista, nombrePaciente, "modificar");

            if (paciente == null)
                return false;

            if (paciente.Mascotas.Count == 0)
            {
                Console.WriteLine($"\nEl paciente '{paciente.Nombre}' no tiene mascotas registradas.");
                return false;
            }

            Console.WriteLine($"\nMascotas de {paciente.Nombre}:");
            for (int i = 0; i < paciente.Mascotas.Count; i++)
            {
                var m = paciente.Mascotas[i];
                Console.WriteLine($"  {i + 1}. {m.Nombre} ({m.Especie}, {m.Edad} anos)");
            }

            int opcion = LeerNumero("Ingrese el numero de la mascota a eliminar: ");

            if (opcion < 1 || opcion > paciente.Mascotas.Count)
            {
                Console.WriteLine("Opcion invalida. No se elimino ninguna mascota.");
                return false;
            }

            var mascotaAEliminar = paciente.Mascotas[opcion - 1];
            paciente.Mascotas.RemoveAt(opcion - 1);

            Console.WriteLine($"\nMascota '{mascotaAEliminar.Nombre}' eliminada del paciente '{paciente.Nombre}'.");
            return true;
        }

        private Paciente SeleccionarPacientePorNombre(List<Paciente> lista, string nombre, string accion)
        {
            string nombreBuscado = Normalizar(nombre);
            var coincidencias = lista.Where(p => Normalizar(p.Nombre) == nombreBuscado).ToList();

            if (coincidencias.Count == 0)
            {
                Console.WriteLine($"\nNo se encontro ningun paciente con el nombre '{nombre}' para {accion}.");
                return null;
            }

            if (coincidencias.Count == 1)
            {
                return coincidencias[0];
            }

            Console.WriteLine("\nSe encontraron varios pacientes con ese nombre:");
            foreach (var p in coincidencias)
            {
                Console.WriteLine($"  ID {p.Id} - {p.Nombre}, tel: {p.Telefono}");
            }

            string idElegido = LeerTexto($"Ingrese el ID completo del paciente que desea {accion}: ");
            var paciente = coincidencias.FirstOrDefault(p => Normalizar(p.Id) == Normalizar(idElegido));

            if (paciente == null)
            {
                Console.WriteLine("El ID ingresado no corresponde a ninguno de los pacientes listados.");
            }

            return paciente;
        }

        private void MostrarPaciente(Paciente paciente)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"ID: {paciente.Id}");
            Console.WriteLine($"Nombre: {paciente.Nombre}");
            Console.WriteLine($"Direccion: {paciente.Direccion}");
            Console.WriteLine($"Telefono: {paciente.Telefono}");

            if (paciente.Mascotas.Count == 0)
            {
                Console.WriteLine("Mascotas: Ninguna registrada");
            }
            else
            {
                Console.WriteLine("Mascotas:");
                foreach (var mascota in paciente.Mascotas)
                {
                    Console.WriteLine($"   - {mascota.Nombre} ({mascota.Especie}, {mascota.Raza}, {mascota.Edad} anos) - Sintoma: {mascota.Sintoma}");
                }
            }

            Console.WriteLine("--------------------------------------");
        }

        public Mascota BuscarMascotaPorNombreExacto(List<Paciente> lista, string nombreMascota)
        {
            foreach (var paciente in lista)
            {
                foreach (var mascota in paciente.Mascotas)
                {
                    if (Normalizar(mascota.Nombre) == Normalizar(nombreMascota))
                    {
                        return mascota;
                    }
                }
            }

            throw new MascotaNoEncontradaException(
                $"No existe ninguna mascota registrada con el nombre '{nombreMascota}'.");
        }

        public int CalcularDosisPorPeso(int miligramosTotales, int pesoKg)
        {
            int dosis = miligramosTotales / pesoKg;
            return dosis;
        }
    }
}