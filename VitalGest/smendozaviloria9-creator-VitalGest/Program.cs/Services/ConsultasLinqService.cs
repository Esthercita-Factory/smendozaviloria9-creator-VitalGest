using System;
using System.Collections.Generic;
using System.Linq;
using VitalGest.Models;

namespace VitalGest.Services
{

    public class ParClienteMascota
    {
        public Cliente Cliente { get; set; }
        public Mascota Mascota { get; set; }
    }

  
    public class ConsultasLinqService
    {

 
        public void CargarDatosDePruebaSiVacia(List<Cliente> clientes)
        {
            if (clientes.Count > 0) return;

            clientes.Add(new Cliente
            {
                Id = "1",
                Nombre = "Ana Torres",
                Telefono = "555-0101",
                Mascotas = new List<Mascota>
                {
                    new Mascota { Nombre = "Firulais", Especie = "Perro", Edad = 4, Sintoma = "Vomitos" }
                }
            });

            clientes.Add(new Cliente
            {
                Id = "2",
                Nombre = "Luis Perez",
                Telefono = "555-0102",
                Mascotas = new List<Mascota>
                {
                    new Mascota { Nombre = "Michi", Especie = "Gato", Edad = 2, Sintoma = "Cojera" }
                }
            });

            clientes.Add(new Cliente
            {
                Id = "3",
                Nombre = "Marta Gomez",
                Telefono = "555-0103",
                Mascotas = new List<Mascota>
                {
                    new Mascota { Nombre = "Rocky", Especie = "Perro", Edad = 6, Sintoma = "Falta de apetito" },
                    new Mascota { Nombre = "Kiwi", Especie = "Ave", Edad = 1, Sintoma = "Estornudos" }
                }
            });

            clientes.Add(new Cliente
            {
                Id = "4",
                Nombre = "Diana Leon",
                Telefono = "555-0104",
                Mascotas = new List<Mascota>
                {
                    new Mascota { Nombre = "Toby", Especie = "Perro", Edad = 3, Sintoma = "" } // sin sintoma definido
                }
            });

            clientes.Add(new Cliente
            {
                Id = "5",
                Nombre = "Pedro Vidal",
                Telefono = "555-0105",
                Mascotas = new List<Mascota>
                {
                    new Mascota { Nombre = "Nala", Especie = "Gato", Edad = 5, Sintoma = "Revision" },
                    new Mascota { Nombre = "Max", Especie = "Perro", Edad = 8, Sintoma = "Fractura" }
                }
            });

            Console.WriteLine("\n(Se cargaron 5 clientes de ejemplo porque la lista estaba vacia)");
        }

        public void EjecutarDemo(List<Cliente> clientes)
        {
            CargarDatosDePruebaSiVacia(clientes);

            List<ParClienteMascota> pares = clientes.SelectMany(
                c => c.Mascotas,
                (c, m) => new ParClienteMascota { Cliente = c, Mascota = m }
            ).ToList();

            Task2_WhereSelectOrderByGroupBy(clientes, pares);
            Task4_ConsultaEncadenada(pares);
            Task5_ProblemasPracticos(clientes, pares);
        }


        private void Task2_WhereSelectOrderByGroupBy(
            List<Cliente> clientes,
            List<ParClienteMascota> paresClienteMascota)
        {
            Console.WriteLine("\n=== TASK 2: Where, Select, OrderBy, GroupBy, First/Any/All/Count ===");


            var duenosDePerros_metodo = paresClienteMascota
                .Where(p => p.Mascota.Especie == "Perro");


            var duenosDePerros_consulta =
                from p in paresClienteMascota
                where p.Mascota.Especie == "Perro"
                select p;

            Console.WriteLine("\nDuenos de perros (Where):");
            foreach (var p in duenosDePerros_metodo)
                Console.WriteLine($"  - {p.Cliente.Nombre} -> {p.Mascota.Nombre}");

  
            var mascotasMayoresA3 = paresClienteMascota.Where(p => p.Mascota.Edad > 3);
            Console.WriteLine("\nMascotas con mas de 3 anos:");
            foreach (var p in mascotasMayoresA3)
                Console.WriteLine($"  - {p.Mascota.Nombre} ({p.Mascota.Edad} anos)");

   
            var soloNombres_metodo = clientes.Select(c => c.Nombre);


            var soloNombres_consulta =
                from c in clientes
                select c.Nombre;

            Console.WriteLine("\nSolo nombres de clientes (Select):");
            foreach (var nombre in soloNombres_metodo)
                Console.WriteLine($"  - {nombre}");


            var clientesPorNombreAsc = clientes.OrderBy(c => c.Nombre);              // A-Z
            var mascotasPorEdadDesc = paresClienteMascota.OrderByDescending(p => p.Mascota.Edad); // mayor a menor

            Console.WriteLine("\nMascotas ordenadas por edad (descendente):");
            foreach (var p in mascotasPorEdadDesc)
                Console.WriteLine($"  - {p.Mascota.Nombre}: {p.Mascota.Edad} anos");


            var porEspecie_metodo = paresClienteMascota.GroupBy(p => p.Mascota.Especie);

            var porEspecie_consulta =
                from p in paresClienteMascota
                group p by p.Mascota.Especie into grupoEspecie
                select grupoEspecie;

            Console.WriteLine("\nMascotas agrupadas por especie:");
            foreach (var grupo in porEspecie_metodo)
            {
                Console.WriteLine($"  Especie: {grupo.Key} ({grupo.Count()} mascotas)");
                foreach (var p in grupo)
                    Console.WriteLine($"    - {p.Mascota.Nombre} (dueno: {p.Cliente.Nombre})");
            }

           
            var primerGato = paresClienteMascota.First(p => p.Mascota.Especie == "Gato");
            Console.WriteLine($"\nFirst -> primera mascota gato: {primerGato.Mascota.Nombre}");

 
            var primeraTortuga = paresClienteMascota.FirstOrDefault(p => p.Mascota.Especie == "Tortuga");
            Console.WriteLine(primeraTortuga is null
                ? "FirstOrDefault -> no hay mascotas tipo tortuga"
                : $"FirstOrDefault -> primera tortuga: {primeraTortuga.Mascota.Nombre}");

            bool hayMascotaMayorA7 = paresClienteMascota.Any(p => p.Mascota.Edad > 7);
            Console.WriteLine($"Any -> hay alguna mascota mayor a 7 anos? {hayMascotaMayorA7}");

            bool todosTienenTelefono = clientes.All(c => !string.IsNullOrWhiteSpace(c.Telefono));
            Console.WriteLine($"All -> todos los clientes tienen telefono? {todosTienenTelefono}");


            int totalClientes = clientes.Count();
            int totalPerros = paresClienteMascota.Count(p => p.Mascota.Especie == "Perro");
            Console.WriteLine($"Count -> total clientes: {totalClientes}, total perros: {totalPerros}");
        }


        private void Task4_ConsultaEncadenada(List<ParClienteMascota> paresClienteMascota)
        {
            Console.WriteLine("\n=== TASK 4: Consulta encadenada ===");

            // Sintaxis de metodo
            var duenosDePerroOrdenados = paresClienteMascota
                .Where(p => p.Mascota.Especie == "Perro")   // 1. filtrar por especie de mascota
                .OrderBy(p => p.Mascota.Edad)                // 2. ordenar por edad de la mascota
                .Select(p => new                              // 3. proyectar solo lo necesario
                {
                    p.Cliente.Nombre,
                    p.Cliente.Telefono
                });


            var duenosDePerroOrdenados_consulta =
                from p in paresClienteMascota
                where p.Mascota.Especie == "Perro"
                orderby p.Mascota.Edad
                select new { p.Cliente.Nombre, p.Cliente.Telefono };

            Console.WriteLine("Duenos de perro, ordenados por edad de la mascota (nombre y telefono):");
            foreach (var item in duenosDePerroOrdenados)
                Console.WriteLine($"  - {item.Nombre} | Tel: {item.Telefono}");
        }

        private void Task5_ProblemasPracticos(List<Cliente> clientes, List<ParClienteMascota> paresClienteMascota)
        {
            Console.WriteLine("\n=== TASK 5: Problemas practicos ===");


            var masJoven = paresClienteMascota.OrderBy(p => p.Mascota.Edad).First();
            var mayorEdad = paresClienteMascota.OrderByDescending(p => p.Mascota.Edad).First();

            Console.WriteLine($"Mascota mas joven: {masJoven.Mascota.Nombre} ({masJoven.Mascota.Edad} anos, dueno: {masJoven.Cliente.Nombre})");
            Console.WriteLine($"Mascota de mayor edad: {mayorEdad.Mascota.Nombre} ({mayorEdad.Mascota.Edad} anos, dueno: {mayorEdad.Cliente.Nombre})");

            var conteoPorEspecie = paresClienteMascota
                .GroupBy(p => p.Mascota.Especie)
                .Select(g => new { Especie = g.Key, Cantidad = g.Count() });

            Console.WriteLine("Cantidad de mascotas por especie:");
            foreach (var item in conteoPorEspecie)
                Console.WriteLine($"  - {item.Especie}: {item.Cantidad}");
            
            bool haySinSintoma = paresClienteMascota.Any(p => string.IsNullOrWhiteSpace(p.Mascota.Sintoma));
            Console.WriteLine($"Existe alguna mascota sin sintoma definido? {haySinSintoma}");

            if (haySinSintoma)
            {
                var mascotasSinSintoma = paresClienteMascota.Where(p => string.IsNullOrWhiteSpace(p.Mascota.Sintoma));
                foreach (var p in mascotasSinSintoma)
                    Console.WriteLine($"  - {p.Mascota.Nombre} (dueno: {p.Cliente.Nombre})");
            }

            var nombresMayusculasOrdenados = clientes
                .Select(c => c.Nombre.ToUpper())
                .OrderBy(nombre => nombre);

            Console.WriteLine("Nombres de clientes en mayusculas, orden alfabetico:");
            foreach (var nombre in nombresMayusculasOrdenados)
                Console.WriteLine($"  - {nombre}");
        }
    }
}