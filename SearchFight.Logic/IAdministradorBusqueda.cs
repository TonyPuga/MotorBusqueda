using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SearchFight.Logic.Models;

namespace SearchFight.Logic
{
    public interface IAdministradorBusqueda
    {
        Task<string> GetSearchReport(List<string> querys);
        Task<List<ResultadoBusqueda>> GetResultsAsync(IEnumerable<string> querys);
        IEnumerable<Ganador> GetWinners(List<ResultadoBusqueda> searchResults);
        string GetTotalWinner(List<ResultadoBusqueda> searchResults);
        IEnumerable<IGrouping<string, ResultadoBusqueda>> GetMainResults(List<ResultadoBusqueda> searchResults);
    }
}
