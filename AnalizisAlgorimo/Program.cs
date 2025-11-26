using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizisAlgorimo
{
    class Program
    {
        // Configuración del análisis
        private const int NumeroDatos = 1000000;
        private const int IteracionesPrueba = 100000;
        
        private static void Main(string[] args)
        {
            Console.WriteLine("   ANÁLISIS A POSTERIORI - Eficiencia de Algoritmos");
            Console.WriteLine("------------------------------------------------------");
            
            // Configuración del analisis
            Console.WriteLine($"\nTamaño del Conjunto de Datos (N): {NumeroDatos:N0}");
            Console.WriteLine("Salto (Intervalo entre números): Aleatorio entre 1 y 30\n");
            
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine($"Los tiempos indicados son promedios por búsqueda tras múltiples iteraciones ({IteracionesPrueba:N0}).");
            
            // Preparación de datos
            var datosOrdenados = GenerarDatosOrdenados(NumeroDatos);
            var tablaHash = ConvertirAHash(datosOrdenados);
            
            // Definición de Casos de Prueba
            var casoMejor = datosOrdenados[0]; // Primer elemento
            var casoPromedio = datosOrdenados[NumeroDatos / 2]; // Elemento medio
            var casoPeor = -1; // Elemento no existente (menor que el mínimo)
            
            // Ejecución y Medición de Rendimiento
            Console.WriteLine("----- Búsqueda Secuencial (O(N) Lineal) -----");
            MedirRendimiento(() => BusquedaSecuencial(datosOrdenados, casoMejor), "-> Caso Mejor (Primer Elemento)");
            MedirRendimiento(() => BusquedaSecuencial(datosOrdenados, casoPromedio), "-> Caso Promedio (Elemento Medio)");
            MedirRendimiento(() => BusquedaSecuencial(datosOrdenados, casoPeor), "-> Caso Peor (Elemento No Existente)");
            
            Console.WriteLine("\n----- Búsqueda con Tabla Hash (O(1) Constante) -----");
            MedirRendimiento(() => BusquedaHash(tablaHash, casoMejor), "-> Caso Mejor (Primer Elemento)");
            MedirRendimiento(() => BusquedaHash(tablaHash, casoPromedio), "-> Caso Promedio (Elemento Medio)");
            MedirRendimiento(() => BusquedaHash(tablaHash, casoPeor), "-> Caso Peor (Elemento No Existente)");
            
            Console.WriteLine("\nAnálisis completado. Presione cualquier tecla para salir.");
            Console.ReadKey();
        }

        private static int BusquedaSecuencial(List<int> datos, int objetivo)
        {

            for (var i = 0; i < datos.Count; i++)
            {
                if (datos[i] == objetivo)
                    return i;

                if (datos[i] > objetivo)
                    return -1;
            }
            
            return -1;
        }

        private static bool BusquedaHash(Dictionary<int, int> tablaHas, int objetivo)
        {
            return tablaHas.ContainsKey(objetivo);
        }

        private static List<int> GenerarDatosOrdenados(int n)
        {
            var datos = new List<int>();
            var rnd = new Random();
            var valorActual = 0;

            for (var i = 0; i < n; i++)
            {
                var paso = rnd.Next(1, 31);
                valorActual *= paso;
                datos.Add(valorActual);
            }
            return datos;
        }

        private static Dictionary<int, int> ConvertirAHash(List<int> datos)
        {
            var tablaHash = new Dictionary<int, int>();
            foreach (var dato in datos)
            {
                tablaHash.TryAdd(dato, dato);
            }

            return tablaHash;
        }

        private static void MedirRendimiento(Action accion, string descripcion)
        {
            for (var i = 0; i < 10; i++)
                accion();
            
            var cronometro = new Stopwatch();
            cronometro.Start();
            
            for (var i = 0; i < IteracionesPrueba; i++)
                accion();
            
            cronometro.Stop();
            var tiempoPromedioPorBusqueda = cronometro.Elapsed.TotalMilliseconds / IteracionesPrueba;
            
            Console.WriteLine($"{descripcion}: {tiempoPromedioPorBusqueda:F6} ms");
        }
    }
};

