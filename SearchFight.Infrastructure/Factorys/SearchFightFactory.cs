using System;
using System.Linq;
using SearchFight.Logic;
using SearchFight.Services.Interfaces;

namespace SearchFight.Infrastructure.Factorys
{
    public class SearchFightFactory
    {
        public static IAdministradorBusqueda CreateSearchManager() => CreateSearchClients();

        private static AdministradorBusqueda CreateSearchClients()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                ?.Where(assembly => assembly.FullName.StartsWith("SearchFight"));

            var searchClients = loadedAssemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetInterface(typeof(ISearchClient).ToString()) != null)
                .Select(type => Activator.CreateInstance(type) as ISearchClient);

            return new AdministradorBusqueda(searchClients);
        }
    }
}
