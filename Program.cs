using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

class Program
{
    static async Task Main(string[] args)
    {
        decimal balance = 50.0m  ;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                string url = "http://prac3-web.test/api/teams";

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var teams = JsonConvert.DeserializeObject<List<Team>>(jsonResponse);

                    while (true)
                    {
                        Console.WriteLine($"\nCurrent Balance: {balance:C}");
                        Console.WriteLine("What would you like to do?");
                        Console.WriteLine("1. Place a bet on a team");
                        Console.WriteLine("2. View upcoming matches for a team");
                        Console.WriteLine("3. Exit");

                        string mainChoice = Console.ReadLine();

                        if (mainChoice == "3")
                        {
                            break;
                        }
                        else if (mainChoice == "1")
                        {
                            Console.WriteLine("Available teams:");
                            for (int i = 0; i < teams.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {teams[i].TeamName}");
                            }

                            Console.WriteLine("\nEnter the number of the team you want to bet on:");
                            string input = Console.ReadLine();

                            if (int.TryParse(input, out int choice) && choice > 0 && choice <= teams.Count)
                            {
                                var selectedTeam = teams[choice - 1];
                                Console.WriteLine($"You selected: {selectedTeam.TeamName}");
                                Console.WriteLine("Enter the amount you want to bet:");
                                string betInput = Console.ReadLine();
                                if (decimal.TryParse(betInput, out decimal betAmount) && betAmount > 0 && betAmount <= balance)
                                {
                                    balance -= betAmount;
                                    Console.WriteLine($"You bet {betAmount:C} on {selectedTeam.TeamName}.");
                                    Console.WriteLine($"New Balance: {balance:C}\n");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid bet amount. Please try again.\n");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid team selection. Please try again.\n");
                            }
                        }
                        else if (mainChoice == "2")
                        {
                            Console.WriteLine("Available teams:");
                            for (int i = 0; i < teams.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {teams[i].TeamName}");
                            }

                            Console.WriteLine("\nEnter the number of the team to view matches:");
                            string input = Console.ReadLine();

                            if (int.TryParse(input, out int choice) && choice > 0 && choice <= teams.Count)
                            {
                                var selectedTeam = teams[choice - 1];
                                Console.WriteLine($"\nMatches for {selectedTeam.TeamName}:");
                                foreach (var match in selectedTeam.Matches)
                                {
                                    Console.WriteLine($"- Opponent: {match.Opponent}, Date: {match.Date}");
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Invalid team selection. Please try again.\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice. Please try again.\n");
                        }
                    }
                } 
                else
                {
                    Console.WriteLine("Error: Unable to fetch data from the API.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

public class Team
{
    [JsonProperty("teamName")]
    public string TeamName { get; set; }

    [JsonProperty("numberOfPlayers")]
    public int NumberOfPlayers { get; set; }

    [JsonProperty("playerNames")]
    public List<string> PlayerNames { get; set; } = new List<string>();

    [JsonProperty("matches")]
    public List<Match> Matches { get; set; } = new List<Match>();

    [JsonConstructor]
    public Team(string teamName, int numberOfPlayers, object playerNames, List<Match> matches)
    {
        TeamName = teamName;
        NumberOfPlayers = numberOfPlayers;
        PlayerNames = playerNames is JArray array ? array.ToObject<List<string>>() : new List<string> { playerNames.ToString() };
        Matches = matches;
    }
}

public class Match
{
    [JsonProperty("opponent")]
    public string Opponent { get; set; }

    [JsonProperty("date")]
    public string Date { get; set; }
}
