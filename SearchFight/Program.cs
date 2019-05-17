using System;
using System.Linq;
using System.Threading.Tasks;
using SearchFight.Common.Exceptions;
using SearchFight.Infrastructure.Factorys;

namespace SearchFight
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("ngrese la(s) cadena(s) a buscar:");
                    args = Console.ReadLine()?.Split(' ');
                }

                Console.WriteLine("Cargando resultados ...");

                var searchManager = SearchFightFactory.CreateSearchManager();
                var result = await searchManager.GetSearchReport(args?.ToList());

                Console.Clear();
                Console.WriteLine(result);
            }
            catch (SearchFightException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar el reporte: {ex.Message}");
            }
            Console.ReadKey();
        }
    }
}
