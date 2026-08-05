using System;
using System.Collections.Generic;
using VitalGest.Models;
using VitalGest.Services;

namespace VitalGest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hola VitalGest");

            List<Cliente> clientes = new List<Cliente>();
            ClienteService servicio = new ClienteService();

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
                            servicio.RegistrarCliente(clientes);
                            break;

                        case "2":
                            servicio.ListarClientes(clientes);
                            break;

                        case "3":
                            string nombreBuscar = servicio.LeerTexto("\nIngrese el nombre del cliente a buscar: ");
                            servicio.BuscarClientePorNombre(clientes, nombreBuscar);
                            break;

                        case "4":
                            string nombreEliminar = servicio.LeerTexto("\nIngrese el nombre del cliente a eliminar: ");
                            servicio.EliminarCliente(clientes, nombreEliminar);
                            break;

                        case "5":
                            string nombreParaMascota = servicio.LeerTexto("\nIngrese el nombre del cliente dueño de la mascota: ");
                            servicio.EliminarMascota(clientes, nombreParaMascota);
                            break;

                        case "6":
                            salir = true;
                            Console.WriteLine("\nGracias por usar el sistema de VitalGest. Hasta pronto.");
                            break;

                        default:
                            Console.WriteLine("\nOpcion invalida. Por favor selecciona una opcion del 1 al 6.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nOcurrio un error inesperado: {ex.Message}");
                    Console.WriteLine("Por favor intenta nuevamente.");
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
            Console.WriteLine("1. Registrar cliente (y mascota opcional)");
            Console.WriteLine("2. Listar clientes");
            Console.WriteLine("3. Buscar cliente por nombre");
            Console.WriteLine("4. Eliminar cliente (con sus mascotas)");
            Console.WriteLine("5. Eliminar una mascota de un cliente");
            Console.WriteLine("6. Salir");
            Console.WriteLine("========================================");
            Console.Write("Seleccione una opcion: ");
        }
    }
}