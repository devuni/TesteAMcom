using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Questao2.Models;

namespace Questao2.Services
{
    public class FootballMatchService
    {
        private readonly HttpClient _httpClient;

        public FootballMatchService()
        {
            // É recomendável manter apenas uma instância de HttpClient
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Retorna o total de gols marcados por um time em um determinado ano.
        /// Soma gols como 'team1' e como 'team2'.
        /// </summary>
        public async Task<int> GetTotalGoalsAsync(int year, string team)
        {
            int totalGoals = 0;

            // Somar gols quando o time aparece como "team1"
            totalGoals += await GetGoalsForPositionAsync(year, team, isTeam1: true);

            // Somar gols quando o time aparece como "team2"
            totalGoals += await GetGoalsForPositionAsync(year, team, isTeam1: false);

            return totalGoals;
        }

        /// <summary>
        /// Faz a busca paginada na API para retornar os gols (team1goals ou team2goals).
        /// </summary>
        private async Task<int> GetGoalsForPositionAsync(int year, string team, bool isTeam1)
        {
            int goalsSum = 0;
            int currentPage = 1;
            int totalPages = 1;

            while (currentPage <= totalPages)
            {
                // Monta parâmetros de consulta
                var queryParams = new Dictionary<string, string>
                {
                    { "year", year.ToString() },
                    { isTeam1 ? "team1" : "team2", team },
                    { "page", currentPage.ToString() }
                };

                // Constrói a URL
                var url = "https://jsonmock.hackerrank.com/api/football_matches"
                          + BuildQueryString(queryParams);

                // Faz requisição
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Lê conteúdo e desserializa
                var responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(
                    responseBody, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (apiResponse == null)
                    break;

                totalPages = apiResponse.TotalPages;

                // Soma os gols do time (dependendo de isTeam1)
                foreach (var match in apiResponse.Data)
                {
                    if (isTeam1)
                    {
                        if (int.TryParse(match.Team1Goals, out int team1Goals))
                            goalsSum += team1Goals;
                    }
                    else
                    {
                        if (int.TryParse(match.Team2Goals, out int team2Goals))
                            goalsSum += team2Goals;
                    }
                }

                currentPage++;
            }

            return goalsSum;
        }

        /// <summary>
        /// Constrói a query string a partir de um dicionário de parâmetros.
        /// </summary>
        private string BuildQueryString(Dictionary<string, string> queryParams)
        {
            var queryParts = new List<string>();
            foreach (var param in queryParams)
            {
                // Faz o encode para lidar com espaços e caracteres especiais
                queryParts.Add(
                    $"{Uri.EscapeDataString(param.Key)}={Uri.EscapeDataString(param.Value)}"
                );
            }

            return "?" + string.Join("&", queryParts);
        }
    }
}
