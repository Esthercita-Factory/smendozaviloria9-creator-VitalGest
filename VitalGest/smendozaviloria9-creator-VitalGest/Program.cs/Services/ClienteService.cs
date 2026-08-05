
using VitalGest.Models;

namespace VitalGest.Services
{
    public class ClienteService
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

        private string GenerarSiguienteId()
        {
            return Guid.NewGuid().ToString();
        }

        public void RegistrarCliente(List<Cliente> lista)
        {
            Console.WriteLine("\n--- Registrar nuevo cliente ---");

            var cliente = new Cliente
            {
                Id = GenerarSiguienteId(),
                Nombre = LeerTexto("Nombre del cliente: "),
                Telefono = LeerTexto("Telefono del cliente: ")
            };

            bool quiereMascota = LeerConfirmacion("Desea registrar una mascota para este cliente? (s/n): ");

            while (quiereMascota)
            {
                var mascota = new Mascota
                {
                    Nombre = LeerTexto("  Nombre de la mascota: "),
                    Especie = LeerTexto("  Especie (perro, gato, ave, etc.): "),
                    Edad = LeerNumero("  Edad de la mascota: "),
                    Sintoma = LeerTexto("  Sintoma o motivo de consulta: ")
                };

                cliente.Mascotas.Add(mascota);

                quiereMascota = LeerConfirmacion("Desea agregar otra mascota? (s/n): ");
            }

            lista.Add(cliente);

            Console.WriteLine($"\nCliente '{cliente.Nombre}' registrado con exito. ID asignado: {cliente.Id}");
        }

        public void ListarClientes(List<Cliente> lista)
        {
            Console.WriteLine("\n--- Lista de clientes ---");

            if (lista.Count == 0)
            {
                Console.WriteLine("No hay clientes registrados todavia.");
                return;
            }

            foreach (var cliente in lista)
            {
                MostrarCliente(cliente);
            }
        }

        public void BuscarClientePorNombre(List<Cliente> lista, string nombre)
        {
            string nombreBuscado = Normalizar(nombre);

            var encontrados = lista
                .Where(c => Normalizar(c.Nombre).Contains(nombreBuscado))
                .ToList();

            if (encontrados.Count == 0)
            {
                Console.WriteLine($"\nNo se encontro ningun cliente con el nombre '{nombre}'.");
                return;
            }

            Console.WriteLine($"\n--- Resultado(s) de la busqueda: '{nombre}' ---");
            foreach (var cliente in encontrados)
            {
                MostrarCliente(cliente);
            }
        }

        public bool EliminarCliente(List<Cliente> lista, string nombre)
        {
            var cliente = SeleccionarClientePorNombre(lista, nombre, "eliminar");

            if (cliente == null)
                return false;

            lista.Remove(cliente);
            Console.WriteLine($"\nCliente '{cliente.Nombre}' (ID {cliente.Id}) eliminado correctamente, junto con sus mascotas.");
            return true;
        }

        public bool EliminarMascota(List<Cliente> lista, string nombreCliente)
        {
            var cliente = SeleccionarClientePorNombre(lista, nombreCliente, "modificar");

            if (cliente == null)
                return false;

            if (cliente.Mascotas.Count == 0)
            {
                Console.WriteLine($"\nEl cliente '{cliente.Nombre}' no tiene mascotas registradas.");
                return false;
            }

            Console.WriteLine($"\nMascotas de {cliente.Nombre}:");
            for (int i = 0; i < cliente.Mascotas.Count; i++)
            {
                var m = cliente.Mascotas[i];
                Console.WriteLine($"  {i + 1}. {m.Nombre} ({m.Especie}, {m.Edad} anos)");
            }

            int opcion = LeerNumero("Ingrese el numero de la mascota a eliminar: ");

            if (opcion < 1 || opcion > cliente.Mascotas.Count)
            {
                Console.WriteLine("Opcion invalida. No se elimino ninguna mascota.");
                return false;
            }

            var mascotaAEliminar = cliente.Mascotas[opcion - 1];
            cliente.Mascotas.RemoveAt(opcion - 1);

            Console.WriteLine($"\nMascota '{mascotaAEliminar.Nombre}' eliminada del cliente '{cliente.Nombre}'.");
            return true;
        }

        private Cliente SeleccionarClientePorNombre(List<Cliente> lista, string nombre, string accion)
        {
            string nombreBuscado = Normalizar(nombre);
            var coincidencias = lista.Where(c => Normalizar(c.Nombre) == nombreBuscado).ToList();

            if (coincidencias.Count == 0)
            {
                Console.WriteLine($"\nNo se encontro ningun cliente con el nombre '{nombre}' para {accion}.");
                return null;
            }

            if (coincidencias.Count == 1)
            {
                return coincidencias[0];
            }

            Console.WriteLine("\nSe encontraron varios clientes con ese nombre:");
            foreach (var c in coincidencias)
            {
                Console.WriteLine($"  ID {c.Id} - {c.Nombre}, tel: {c.Telefono}");
            }

            string idElegido = LeerTexto($"Ingrese el ID completo del cliente que desea {accion}: ");
            var cliente = coincidencias.FirstOrDefault(c => Normalizar(c.Id) == Normalizar(idElegido));

            if (cliente == null)
            {
                Console.WriteLine("El ID ingresado no corresponde a ninguno de los clientes listados.");
            }

            return cliente;
        }

        private void MostrarCliente(Cliente cliente)
        {
            Console.WriteLine("--------------------------------------");
            Console.WriteLine($"ID: {cliente.Id}");
            Console.WriteLine($"Nombre: {cliente.Nombre}");
            Console.WriteLine($"Telefono: {cliente.Telefono}");

            if (cliente.Mascotas.Count == 0)
            {
                Console.WriteLine("Mascotas: Ninguna registrada");
            }
            else
            {
                Console.WriteLine("Mascotas:");
                foreach (var mascota in cliente.Mascotas)
                {
                    Console.WriteLine($"   - {mascota.Nombre} ({mascota.Especie}, {mascota.Edad} anos) - Sintoma: {mascota.Sintoma}");
                }
            }

            Console.WriteLine("--------------------------------------");
        }
    }
}