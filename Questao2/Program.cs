using System;
using System.Threading.Tasks;
using Questao2.Services;

namespace Questao2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
           
            var footballMatchService = new FootballMatchService();

            int psgGoals2013 = await footballMatchService.GetTotalGoalsAsync(2013, "Paris Saint-Germain");
            int chelseaGoals2014 = await footballMatchService.GetTotalGoalsAsync(2014, "Chelsea");

            Console.WriteLine($"Team Paris Saint-Germain scored {psgGoals2013} goals in 2013");
            Console.WriteLine($"Team Chelsea scored {chelseaGoals2014} goals in 2014");
        }
    }
}
