using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchFight.Common.Exceptions;
using SearchFight.Common.Extensions;
using SearchFight.Logic.Models;
using SearchFight.Services.Interfaces;



namespace SearchFight.Logic
{
    public class AdministradorBusqueda : IAdministradorBusqueda
    {
        private readonly IEnumerable<ISearchClient> _searchClients;
        private readonly StringBuilder _stringBuilder;

        public AdministradorBusqueda(IEnumerable<ISearchClient> searchClients)
        {
            _searchClients = searchClients;
            _stringBuilder = new StringBuilder();
        }

        public async Task<string> GetSearchReport(List<string> querys)
        {
            if (querys == null)
                throw new ArgumentNullException(nameof(querys));

            try
            {
                var resultadoBusqueda = await GetResultsAsync(querys.Distinct());

                var ganadores = GetWinners(resultadoBusqueda);
                var totalGanador = GetTotalWinner(resultadoBusqueda);
                var Resultados = GetMainResults(resultadoBusqueda);


                var clientResultsString = Resultados
                    .Select(resultsGroup =>
                        $"{resultsGroup.Key}: {string.Join(" ", resultsGroup.Select(client => $"{client.SearchClient}: {client.TotalResults}"))}")
                    .ToList();

                var winnerString = ganadores.Select(client => $"{client.ClientName} ganador: {client.WinnerQuery}")
                    .ToList();

                var totallWinnerString = $"Total ganador: {totalGanador}";


                clientResultsString.ForEach(queryResults => _stringBuilder.AppendLine(queryResults));
                winnerString.ForEach(winners => _stringBuilder.AppendLine(winners));

                _stringBuilder.AppendLine(totallWinnerString);

                return _stringBuilder.ToString();
            }
            catch (SearchFightLogicException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new SearchFightLogicException("Error al procesar los resultados", e);
            }
        }

        public IEnumerable<Ganador> GetWinners(List<ResultadoBusqueda> searchResults)
        {
            if (searchResults == null)
                throw new ArgumentNullException(nameof(searchResults));

            var winners = searchResults
                .OrderBy(result => result.SearchClient)
                .GroupBy(result => result.SearchClient, result => result,
                    (client, result) => new Ganador
                    {
                        ClientName = client,
                        WinnerQuery = result.MaxValue(r => r.TotalResults).Query
                    });

            return winners;
        }

        public string GetTotalWinner(List<ResultadoBusqueda> searchResults)
        {
            if (searchResults == null)
                throw new ArgumentNullException(nameof(searchResults));

            var totalWinner = searchResults
                .OrderBy(result => result.SearchClient)
                .GroupBy(result => result.Query, result => result,
                    (query, result) => new { Query = query, Total = result.Sum(r => r.TotalResults) })
                .MaxValue(r => r.Total).Query;

            return totalWinner;
        }

        public IEnumerable<IGrouping<string, ResultadoBusqueda>> GetMainResults(List<ResultadoBusqueda> searchResults)
        {
            if (searchResults == null)
                throw new ArgumentNullException(nameof(searchResults));

            var results = searchResults
                .OrderBy(result => result.SearchClient)
                .ToLookup(result => result.Query, result => result);

            return results;
        }

        public async Task<List<ResultadoBusqueda>> GetResultsAsync(IEnumerable<string> querys)
        {
            var results = new List<ResultadoBusqueda>();

            foreach (var keyword in querys)
            {
                foreach (var searchClient in _searchClients)
                {
                    results.Add(new ResultadoBusqueda
                    {
                        SearchClient = searchClient.ClientName,
                        Query = keyword,
                        TotalResults = await searchClient.GetResultsCountAsync(keyword)
                    });
                }
            }

            return results;
        }
    }
}
