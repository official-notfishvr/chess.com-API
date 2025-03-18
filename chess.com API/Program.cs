using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessAPI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var chessApiClient = new ChessApiClient();
            bool exitProgram = false;

            while (!exitProgram)
            {
                Console.Clear();
                Console.WriteLine("═════════════════════════════════════════════");
                Console.WriteLine("         Chess.com API Client");
                Console.WriteLine("═════════════════════════════════════════════");
                Console.WriteLine("1. View Player Profile");
                Console.WriteLine("2. View Daily Puzzle");
                Console.WriteLine("3. Get Titled Players");
                Console.WriteLine("4. View Chess.com Leaderboards");
                Console.WriteLine("5. View Club Information");
                Console.WriteLine("6. View Tournament Details");
                Console.WriteLine("0. Exit");
                Console.WriteLine("═════════════════════════════════════════════");
                Console.Write("Enter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    choice = -1;
                }

                try
                {
                    switch (choice)
                    {
                        case 0:
                            exitProgram = true;
                            break;
                        case 1:
                            await ShowPlayerProfileMenu(chessApiClient);
                            break;
                        case 2:
                            await ShowDailyPuzzle(chessApiClient);
                            break;
                        case 3:
                            await ShowTitledPlayers(chessApiClient);
                            break;
                        case 4:
                            await ShowLeaderboards(chessApiClient);
                            break;
                        case 5:
                            await ShowClubInformation(chessApiClient);
                            break;
                        case 6:
                            await ShowTournamentDetails(chessApiClient);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nAn error occurred: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
        static async Task ShowPlayerProfileMenu(ChessApiClient chessApiClient)
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine("           Player Information");
            Console.WriteLine("═════════════════════════════════════════════");
            Console.Write("Enter Chess.com username: ");
            var username = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Username cannot be empty. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            bool exitSubmenu = false;
            while (!exitSubmenu)
            {
                Console.Clear();
                Console.WriteLine("═════════════════════════════════════════════");
                Console.WriteLine($"       Information for {username}");
                Console.WriteLine("═════════════════════════════════════════════");
                Console.WriteLine("1. Basic Profile");
                Console.WriteLine("2. Game Statistics");
                Console.WriteLine("3. Recent Games");
                Console.WriteLine("4. Game Archives");
                Console.WriteLine("5. Club Memberships");
                Console.WriteLine("0. Back to Main Menu");
                Console.WriteLine("═════════════════════════════════════════════");
                Console.Write("Enter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int subChoice))
                {
                    subChoice = -1;
                }

                try
                {
                    switch (subChoice)
                    {
                        case 0:
                            exitSubmenu = true;
                            break;
                        case 1:
                            await ShowPlayerProfile(chessApiClient, username);
                            break;
                        case 2:
                            await ShowPlayerStats(chessApiClient, username);
                            break;
                        case 3:
                            await ShowRecentGames(chessApiClient, username);
                            break;
                        case 4:
                            await ShowGameArchives(chessApiClient, username);
                            break;
                        case 5:
                            await ShowPlayerClubs(chessApiClient, username);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Press any key to continue...");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nAn error occurred: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }
        static async Task ShowPlayerProfile(ChessApiClient chessApiClient, string username)
        {
            Console.Clear();
            var profile = await chessApiClient.GetPlayerProfile(username);

            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"   Profile of {profile?.username ?? "unknown"}");
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"Name: {profile?.name ?? "not available"}");
            Console.WriteLine($"Title: {profile?.title ?? "no title"}");
            Console.WriteLine($"Status: {profile?.status ?? "not available"}");
            Console.WriteLine($"League: {profile?.league ?? "not available"}");
            Console.WriteLine($"Location: {profile?.location ?? "not available"}");
            Console.WriteLine($"Followers: {profile?.followers ?? 0}");
            Console.WriteLine($"Country: {profile?.country ?? "unknown"}");
            Console.WriteLine($"Last Online: {UnixTimeToDateTime(profile?.last_online)}");
            Console.WriteLine($"Joined: {UnixTimeToDateTime(profile?.joined)}");
            Console.WriteLine($"Is Streamer: {(profile?.is_streamer ?? false ? "Yes" : "No")}");
            if (!string.IsNullOrEmpty(profile?.twitch_url))
            {
                Console.WriteLine($"Twitch URL: {profile.twitch_url}");
            }
            Console.WriteLine($"Profile URL: {profile?.url ?? "not available"}");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        static async Task ShowPlayerStats(ChessApiClient chessApiClient, string username)
        {
            Console.Clear();
            var stats = await chessApiClient.GetPlayerStats(username);

            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"  Stats of {username}");
            Console.WriteLine("═════════════════════════════════════════════");

            Console.WriteLine("\n-- Chess Ratings --");
            Console.WriteLine($"Blitz Rating: {stats?.chess_blitz?.last?.rating ?? 0}");
            Console.WriteLine($"Bullet Rating: {stats?.chess_bullet?.last?.rating ?? 0}");
            Console.WriteLine($"Rapid Rating: {stats?.chess_rapid?.last?.rating ?? 0}");
            Console.WriteLine($"Daily Rating: {stats?.chess_daily?.last?.rating ?? 0}");

            Console.WriteLine("\n-- Best Ratings --");
            Console.WriteLine($"Best Blitz: {stats?.chess_blitz?.best?.rating ?? 0} ({UnixTimeToDateTime(stats?.chess_blitz?.best?.date)})");
            Console.WriteLine($"Best Bullet: {stats?.chess_bullet?.best?.rating ?? 0} ({UnixTimeToDateTime(stats?.chess_bullet?.best?.date)})");
            Console.WriteLine($"Best Rapid: {stats?.chess_rapid?.best?.rating ?? 0} ({UnixTimeToDateTime(stats?.chess_rapid?.best?.date)})");
            Console.WriteLine($"Best Daily: {stats?.chess_daily?.best?.rating ?? 0} ({UnixTimeToDateTime(stats?.chess_daily?.best?.date)})");

            Console.WriteLine("\n-- Records --");
            if (stats?.chess_blitz?.record != null)
            {
                double winRate = CalculateWinRate(stats.chess_blitz.record.win, stats.chess_blitz.record.loss, stats.chess_blitz.record.draw);
                Console.WriteLine($"Blitz: {stats.chess_blitz.record.win}W / {stats.chess_blitz.record.loss}L / {stats.chess_blitz.record.draw}D ({winRate:F1}% win rate)");
            }
            if (stats?.chess_bullet?.record != null)
            {
                double winRate = CalculateWinRate(stats.chess_bullet.record.win, stats.chess_bullet.record.loss, stats.chess_bullet.record.draw);
                Console.WriteLine($"Bullet: {stats.chess_bullet.record.win}W / {stats.chess_bullet.record.loss}L / {stats.chess_bullet.record.draw}D ({winRate:F1}% win rate)");
            }
            if (stats?.chess_rapid?.record != null)
            {
                double winRate = CalculateWinRate(stats.chess_rapid.record.win, stats.chess_rapid.record.loss, stats.chess_rapid.record.draw);
                Console.WriteLine($"Rapid: {stats.chess_rapid.record.win}W / {stats.chess_rapid.record.loss}L / {stats.chess_rapid.record.draw}D ({winRate:F1}% win rate)");
            }
            if (stats?.chess_daily?.record != null)
            {
                double winRate = CalculateWinRate(stats.chess_daily.record.win, stats.chess_daily.record.loss, stats.chess_daily.record.draw);
                Console.WriteLine($"Daily: {stats.chess_daily.record.win}W / {stats.chess_daily.record.loss}L / {stats.chess_daily.record.draw}D ({winRate:F1}% win rate)");
            }

            Console.WriteLine("\n-- Tactics --");
            if (stats?.tactics?.highest != null)
            {
                Console.WriteLine($"Highest Tactics Rating: {stats.tactics.highest.rating} ({UnixTimeToDateTime(stats.tactics.highest.date)})");
            }

            Console.WriteLine("\n-- Puzzle Rush --");
            if (stats?.puzzle_rush?.best != null)
            {
                Console.WriteLine($"Best Score: {stats.puzzle_rush.best.score} (from {stats.puzzle_rush.best.total_attempts} attempts)");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        static async Task ShowRecentGames(ChessApiClient chessApiClient, string username)
        {
            Console.Clear();
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var games = await chessApiClient.GetPlayerGameArchive(username, currentYear, currentMonth);

            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"   Recent Games of {username} ({currentYear}/{currentMonth:00})");
            Console.WriteLine("═════════════════════════════════════════════");

            if (games == null || games.Count == 0)
            {
                Console.WriteLine("No recent games found for this month.");
            }
            else
            {
                int count = 0;
                foreach (var game in games)
                {
                    count++;
                    Console.WriteLine($"\nGame #{count} - {game.GetEndTimeAsDateTime()}");
                    Console.WriteLine($"{game.time_class} game - {(game.rated ? "Rated" : "Unrated")} - {game.rules}");
                    Console.WriteLine($"White: {game.white?.username ?? "unknown"} ({game.white?.rating ?? 0}) - {game.white?.result ?? "N/A"}");
                    Console.WriteLine($"Black: {game.black?.username ?? "unknown"} ({game.black?.rating ?? 0}) - {game.black?.result ?? "N/A"}");
                    Console.WriteLine($"URL: {game.url}");

                    if (count % 5 == 0 && count < games.Count)
                    {
                        Console.WriteLine("\nPress any key to see more games or Escape to return...");
                        var key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Escape) { break; }
                    }
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        static async Task ShowGameArchives(ChessApiClient chessApiClient, string username)
        {
            bool returnToMainMenu = false;
            while (!returnToMainMenu)
            {
                Console.Clear();
                var archives = await chessApiClient.GetPlayerGameArchives(username);
                Console.WriteLine("═════════════════════════════════════════════");
                Console.WriteLine($"   Game Archives for {username}");
                Console.WriteLine("═════════════════════════════════════════════");

                if (archives == null || archives.Count == 0)
                {
                    Console.WriteLine("No archives found.");
                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    return;
                }

                int currentPage = 0;
                int itemsPerPage = 10;
                int totalPages = (int)Math.Ceiling(archives.Count / (double)itemsPerPage);

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("═════════════════════════════════════════════");
                    Console.WriteLine($"   Game Archives for {username} - Page {currentPage + 1}/{totalPages}");
                    Console.WriteLine("═════════════════════════════════════════════");

                    var pageArchives = archives.OrderByDescending(a => a).Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();

                    for (int i = 0; i < pageArchives.Count; i++)
                    {
                        var archive = pageArchives[i];
                        var parts = archive.Split('/');
                        if (parts.Length >= 2)
                        {
                            var year = parts[parts.Length - 2];
                            var month = parts[parts.Length - 1];
                            Console.WriteLine($"{(currentPage * itemsPerPage) + i + 1}. Archive: {year}/{month}");
                        }
                        else
                        {
                            Console.WriteLine($"{(currentPage * itemsPerPage) + i + 1}. Archive: {archive}");
                        }
                    }

                    Console.WriteLine("\nOptions:");
                    Console.WriteLine("Enter archive number to view games");
                    if (currentPage > 0) Console.WriteLine("P - Previous page");
                    if (currentPage < totalPages - 1) Console.WriteLine("N - Next page");
                    Console.WriteLine("ESC - Return to main menu");

                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Escape)
                    {
                        returnToMainMenu = true;
                        break;
                    }
                    else if (key.Key == ConsoleKey.P && currentPage > 0)
                    {
                        currentPage--;
                    }
                    else if (key.Key == ConsoleKey.N && currentPage < totalPages - 1)
                    {
                        currentPage++;
                    }
                    else if (char.IsDigit(key.KeyChar))
                    {
                        var input = key.KeyChar.ToString();
                        var moredigits = Console.ReadLine();
                        input += moredigits;

                        if (int.TryParse(input, out int archiveIndex) &&archiveIndex > 0 && archiveIndex <= archives.Count)
                        {
                            await ViewGamesInArchive(chessApiClient, username, archives[archiveIndex - 1]);
                        }
                        else
                        {
                            Console.WriteLine("Invalid archive number. Press any key to continue...");
                            Console.ReadKey();
                        }
                    }
                }
            }
        }
        static async Task ViewGamesInArchive(ChessApiClient chessApiClient, string username, string archiveUrl)
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"   Loading games from archive...");
            Console.WriteLine("═════════════════════════════════════════════");

            try
            {
                var games = await chessApiClient.GetGamesFromArchive(archiveUrl);

                if (games == null || games.Count == 0)
                {
                    Console.WriteLine("No games found in this archive.");
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    return;
                }

                int currentPage = 0;
                int itemsPerPage = 10;
                int totalPages = (int)Math.Ceiling(games.Count / (double)itemsPerPage);

                while (true)
                {
                    Console.Clear();
                    var parts = archiveUrl.Split('/');
                    var archiveLabel = parts.Length >= 2 ? $"{parts[parts.Length - 2]}/{parts[parts.Length - 1]}" : archiveUrl;

                    Console.WriteLine("═════════════════════════════════════════════");
                    Console.WriteLine($"   Games for {username} - {archiveLabel}");
                    Console.WriteLine($"   Page {currentPage + 1}/{totalPages} - {games.Count} games total");
                    Console.WriteLine("═════════════════════════════════════════════");

                    var pageGames = games.Skip(currentPage * itemsPerPage).Take(itemsPerPage).ToList();

                    for (int i = 0; i < pageGames.Count; i++)
                    {
                        var game = pageGames[i];
                        var endDate = DateTimeOffset.FromUnixTimeMilliseconds(game.EndTime).DateTime.ToLocalTime();

                        string result;
                        switch (game.Result)
                        {
                            case "win":
                                result = game.White?.Result == "win" ? "1-0" : "0-1";
                                break;
                            case "draw":
                                result = "½-½";
                                break;
                            default:
                                result = game.Result;
                                break;
                        }
                        Console.WriteLine($"{(currentPage * itemsPerPage) + i + 1}. {endDate.ToShortDateString()} - " +$"{game.White?.Username} ({game.White?.Rating}) vs {game.Black?.Username} ({game.Black?.Rating}) - " + $"{result} - {game.TimeClass}");
                    }

                    Console.WriteLine("\nOptions:");
                    Console.WriteLine("Enter game number to view details");
                    if (currentPage > 0) Console.WriteLine("P - Previous page");
                    if (currentPage < totalPages - 1) Console.WriteLine("N - Next page");
                    Console.WriteLine("ESC - Return to archives");

                    var key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    else if (key.Key == ConsoleKey.P && currentPage > 0)
                    {
                        currentPage--;
                    }
                    else if (key.Key == ConsoleKey.N && currentPage < totalPages - 1)
                    {
                        currentPage++;
                    }
                    else if (char.IsDigit(key.KeyChar))
                    {
                        var input = key.KeyChar.ToString();
                        var moredigits = Console.ReadLine();
                        input += moredigits;

                        if (int.TryParse(input, out int gameIndex) && gameIndex > 0 && gameIndex <= games.Count)
                        {
                            await ViewGameDetails(games[gameIndex - 1]);
                        }
                        else
                        {
                            Console.WriteLine("Invalid game number. Press any key to continue...");
                            Console.ReadKey();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading games: {ex.Message}");
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
            }
        }
        static async Task ViewGameDetails(GameInfo game)
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"   Game Details");
            Console.WriteLine("═════════════════════════════════════════════");

            var endDate = DateTimeOffset.FromUnixTimeSeconds(game.EndTime).DateTime.ToLocalTime();

            string result;
            if (game.Result == "win")
            {
                result = game.White?.Result == "win" ? "1-0" : "0-1";
            }
            else if (game.Result == "draw")
            {
                result = "½-½";
            }
            else
            {
                result = game.Result;
            }

            Console.WriteLine($"Date: {endDate}");
            Console.WriteLine($"White: {game.White?.Username} ({game.White?.Rating})");
            Console.WriteLine($"Black: {game.Black?.Username} ({game.Black?.Rating})");
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"Time Control: {game.TimeControl}");
            Console.WriteLine($"Time Class: {game.TimeClass}");
            Console.WriteLine($"Game URL: {game.Url}");
            Console.WriteLine("\nPGN:");

            var pgn = game.Pgn;
            int lineLength = Console.WindowWidth - 4;

            for (int i = 0; i < pgn.Length; i += lineLength)
            {
                if (i + lineLength <= pgn.Length) { Console.WriteLine("  " + pgn.Substring(i, lineLength)); }
                else { Console.WriteLine("  " + pgn.Substring(i)); }
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
        static async Task ShowPlayerClubs(ChessApiClient chessApiClient, string username)
        {
            Console.Clear();
            var clubs = await chessApiClient.GetPlayerClubs(username);

            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"   Clubs for {username}");
            Console.WriteLine("═════════════════════════════════════════════");

            if (clubs == null || clubs.Count == 0)
            {
                Console.WriteLine("No clubs found.");
            }
            else
            {
                foreach (var club in clubs)
                {
                    Console.WriteLine($"\nClub: {club.name}");
                    Console.WriteLine($"Joined: {club.GetLastActivityAsDateTime()}");
                    Console.WriteLine($"Members: {club.members_count}");
                    Console.WriteLine($"URL: {club.url}");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        static async Task ShowDailyPuzzle(ChessApiClient chessApiClient)
        {
            Console.Clear();
            var puzzle = await chessApiClient.GetDailyPuzzle();

            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine("  Daily Puzzle");
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"Title: {puzzle?.title ?? "no puzzle available"}");
            Console.WriteLine($"Puzzle URL: {puzzle?.url ?? "not available"}");
            Console.WriteLine($"Publish Time: {puzzle?.GetPublishTimeAsDateTime() ?? "not available"}");
            Console.WriteLine($"FEN: {puzzle?.fen ?? "not available"}");
            Console.WriteLine("\nPGN:");
            Console.WriteLine(puzzle?.pgn ?? "not available");

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        static async Task ShowTitledPlayers(ChessApiClient chessApiClient)
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine("  Titled Players");
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine("Select title to view players:");
            Console.WriteLine("1. GM  (Grandmaster)");
            Console.WriteLine("2. IM  (International Master)");
            Console.WriteLine("3. FM  (FIDE Master)");
            Console.WriteLine("4. WGM (Woman Grandmaster)");
            Console.WriteLine("5. WIM (Woman International Master)");
            Console.WriteLine("6. WFM (Woman FIDE Master)");
            Console.WriteLine("7. NM  (National Master)");
            Console.WriteLine("8. CM  (Candidate Master)");
            Console.WriteLine("9. WCM (Woman Candidate Master)");
            Console.Write("\nSelect title (1-9): ");

            if (!int.TryParse(Console.ReadLine(), out int titleChoice) || titleChoice < 1 || titleChoice > 9)
            {
                Console.WriteLine("Invalid selection. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            string[] titles = { "GM", "IM", "FM", "WGM", "WIM", "WFM", "NM", "CM", "WCM" };
            string title = titles[titleChoice - 1];

            Console.Clear();
            Console.WriteLine($"Fetching {title} players... This may take a moment.");

            try
            {
                var players = await chessApiClient.GetTitledPlayers(title);

                Console.Clear();
                Console.WriteLine("═════════════════════════════════════════════");
                Console.WriteLine($"  {title} Players");
                Console.WriteLine("═════════════════════════════════════════════");

                if (players == null || players.Count == 0)
                {
                    Console.WriteLine("No players found.");
                }
                else
                {
                    Console.WriteLine($"Found {players.Count} {title} players.");
                    int count = 0;
                    foreach (var playerUsername in players)
                    {
                        count++;
                        Console.WriteLine($"{count}. {playerUsername}");

                        if (count % 20 == 0 && count < players.Count)
                        {
                            Console.WriteLine("\nPress any key to see more or Escape to return...");
                            var key = Console.ReadKey();
                            if (key.Key == ConsoleKey.Escape) { break; }
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        static async Task ShowLeaderboards(ChessApiClient chessApiClient)
        {
            Console.Clear();
            Console.WriteLine("Fetching leaderboards... This may take a moment.");
            var leaderboards = await chessApiClient.GetLeaderboard();

            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine("  Chess.com Leaderboards");
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine("Select category to view:");
            Console.WriteLine("1. Live Blitz");
            Console.WriteLine("2. Live Bullet");
            Console.WriteLine("3. Live Rapid");
            Console.WriteLine("4. Daily Chess");
            Console.WriteLine("5. Daily 960");
            Console.WriteLine("6. Tactics");
            Console.Write("\nSelect category (1-6): ");

            if (!int.TryParse(Console.ReadLine(), out int categoryChoice) || categoryChoice < 1 || categoryChoice > 6)
            {
                Console.WriteLine("Invalid selection. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            List<Leaderboard.LeaderboardPlayer> players = null;
            string categoryName = "";

            switch (categoryChoice)
            {
                case 1:
                    players = leaderboards.live_blitz;
                    categoryName = "Live Blitz";
                    break;
                case 2:
                    players = leaderboards.live_bullet;
                    categoryName = "Live Bullet";
                    break;
                case 3:
                    players = leaderboards.live_rapid;
                    categoryName = "Live Rapid";
                    break;
                case 4:
                    players = leaderboards.daily;
                    categoryName = "Daily Chess";
                    break;
                case 5:
                    players = leaderboards.daily960;
                    categoryName = "Daily 960";
                    break;
                case 6:
                    players = leaderboards.tactics;
                    categoryName = "Tactics";
                    break;
            }

            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"  {categoryName} Leaderboard");
            Console.WriteLine("═════════════════════════════════════════════");

            if (players == null || players.Count == 0)
            {
                Console.WriteLine("No leaderboard data available.");
            }
            else
            {
                Console.WriteLine($"{"Rank",4} | {"Player",20} | {"Rating",6} | {"Title",4}");
                Console.WriteLine(new string('-', 45));

                foreach (var player in players)
                {
                    string displayName = string.IsNullOrEmpty(player.title) ? player.username : $"{player.title} {player.username}";
                    if (displayName.Length > 20) { displayName = displayName.Substring(0, 17) + "..."; }
                    Console.WriteLine($"{player.rank,4} | {displayName,20} | {player.score,6} | {player.title,4}");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        static async Task ShowClubInformation(ChessApiClient chessApiClient)
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine("  Club Information");
            Console.WriteLine("═════════════════════════════════════════════");
            Console.Write("Enter club URL ID (e.g., 'chess-com-developer-community'): ");
            var clubId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(clubId))
            {
                Console.WriteLine("Club ID cannot be empty. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("Fetching club information... This may take a moment.");
            var club = await chessApiClient.GetClubProfile(clubId);

            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"  Club: {club.name}");
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"Club ID: {club.club_id}");
            Console.WriteLine($"Created: {club.GetCreatedAsDateTime()}");
            Console.WriteLine($"Last Activity: {club.GetLastActivityAsDateTime()}");
            Console.WriteLine($"Members: {club.members_count}");
            Console.WriteLine($"Average Rating: {club.average_daily_rating}");
            Console.WriteLine($"Country: {club.country ?? "Not specified"}");
            Console.WriteLine($"Visibility: {club.visibility}");
            Console.WriteLine($"Join Request: {club.join_request}");
            Console.WriteLine($"URL: {club.url}");

            Console.WriteLine("\nAdministrators:");
            foreach (var admin in club.admin)
            {
                Console.WriteLine($"- {admin}");
            }

            Console.WriteLine("\nDescription:");
            Console.WriteLine(club.description);

            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. View Club Members");
            Console.WriteLine("2. View Club Tournaments");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("\nSelect option: ");

            if (!int.TryParse(Console.ReadLine(), out int subChoice))
            {
                Console.WriteLine("Invalid option. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            switch (subChoice)
            {
                case 1:
                    await ShowClubMembers(chessApiClient, clubId, club.name);
                    break;
                case 2:
                    await ShowClubTournaments(chessApiClient, clubId, club.name);
                    break;
            }
        }
        static async Task ShowClubMembers(ChessApiClient chessApiClient, string clubId, string clubName)
        {
            Console.Clear();
            Console.WriteLine("Fetching club members... This may take a moment.");
            var members = await chessApiClient.GetClubMembers(clubId);

            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"  Members of {clubName}");
            Console.WriteLine("═════════════════════════════════════════════");

            if (members == null || members.Count == 0)
            {
                Console.WriteLine("No members found.");
            }
            else
            {
                Console.WriteLine($"Total Members: {members.Count}");
                int count = 0;
                foreach (var member in members)
                {
                    count++;
                    Console.WriteLine($"{count}. {member.username} (Joined: {member.GetJoinedAsDateTime()})");

                    if (count % 20 == 0 && count < members.Count)
                    {
                        Console.WriteLine("\nPress any key to see more or Escape to return...");
                        var key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Escape) { break; }
                        Console.WriteLine();
                    }
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        static async Task ShowClubTournaments(ChessApiClient chessApiClient, string clubId, string clubName)
        {
            Console.Clear();
            Console.WriteLine("Fetching club tournaments... This may take a moment.");
            var tournaments = await chessApiClient.GetClubTournaments(clubId);

            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"  Tournaments of {clubName}");
            Console.WriteLine("═════════════════════════════════════════════");

            if (tournaments == null || tournaments.Count == 0)
            {
                Console.WriteLine("No tournaments found.");
            }
            else
            {
                foreach (var tournament in tournaments)
                {
                    Console.WriteLine($"\nName: {tournament.name}");
                    Console.WriteLine($"Status: {tournament.status}");
                    Console.WriteLine($"Start Time: {tournament.GetStartTimeAsDateTime()}");
                    Console.WriteLine($"End Time: {tournament.GetEndTimeAsDateTime()}");
                    Console.WriteLine($"Type: {tournament.settings?.type ?? "Unknown"}");
                    Console.WriteLine($"Time Class: {tournament.settings?.time_class ?? "Unknown"}");
                    Console.WriteLine($"Time Control: {tournament.settings?.time_control ?? "Unknown"}");
                    Console.WriteLine($"Rated: {(tournament.settings?.rated ?? false ? "Yes" : "No")}");
                    Console.WriteLine($"URL: {tournament.url}");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        static async Task ShowTournamentDetails(ChessApiClient chessApiClient)
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine("  Tournament Details");
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine("Enter tournament ID/URL name (e.g., 'rapid-chess-league')");
            Console.Write("Tournament ID: ");
            var tournamentId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(tournamentId))
            {
                Console.WriteLine("Tournament ID cannot be empty. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("Fetching tournament information... This may take a moment.");
            var tournament = await chessApiClient.GetTournament(tournamentId);

            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"  Tournament: {tournament.name}");
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"Status: {tournament.status}");
            Console.WriteLine($"Start Time: {tournament.GetStartTimeAsDateTime()}");
            Console.WriteLine($"End Time: {tournament.GetEndTimeAsDateTime()}");
            Console.WriteLine($"Type: {tournament.settings?.type ?? "Unknown"}");
            Console.WriteLine($"Time Class: {tournament.settings?.time_class ?? "Unknown"}");
            Console.WriteLine($"Time Control: {tournament.settings?.time_control ?? "Unknown"}");
            Console.WriteLine($"Rated: {(tournament.settings?.rated ?? false ? "Yes" : "No")}");
            Console.WriteLine($"URL: {tournament.url}");

            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. View Tournament Rounds");
            Console.WriteLine("2. View Tournament Standings");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("\nSelect option: ");

            if (!int.TryParse(Console.ReadLine(), out int subChoice))
            {
                Console.WriteLine("Invalid option. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            switch (subChoice)
            {
                case 1:
                    await ShowTournamentRounds(chessApiClient, tournamentId, tournament.name);
                    break;
                case 2:
                    await ShowTournamentStandings(chessApiClient, tournamentId, tournament.name);
                    break;
            }
        }
        static async Task ShowTournamentRounds(ChessApiClient chessApiClient, string tournamentId, string tournamentName)
        {
            Console.Clear();
            Console.WriteLine("Fetching tournament rounds... This may take a moment.");
            var rounds = await chessApiClient.GetTournamentRounds(tournamentId);

            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"  Rounds of {tournamentName}");
            Console.WriteLine("═════════════════════════════════════════════");

            if (rounds == null || rounds.Count == 0)
            {
                Console.WriteLine("No rounds found.");
            }
            else
            {
                foreach (var round in rounds)
                {
                    Console.WriteLine($"\nRound: {round.name}");

                    if (round.games == null || round.games.Count == 0)
                    {
                        Console.WriteLine("  No games found for this round.");
                    }
                    else
                    {
                        Console.WriteLine($"  Games: {round.games.Count}");
                        int count = 0;

                        foreach (var game in round.games.Take(5))
                        {
                            count++;
                            Console.WriteLine($"  Game {count}: {game.white} vs {game.black} - Result: {game.result}");
                        }

                        if (round.games.Count > 5)
                        {
                            Console.WriteLine($"  ... and {round.games.Count - 5} more games");
                        }
                    }
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        static async Task ShowTournamentStandings(ChessApiClient chessApiClient, string tournamentId, string tournamentName)
        {
            Console.Clear();
            Console.WriteLine("Fetching tournament standings... This may take a moment.");
            var standings = await chessApiClient.GetTournamentStandings(tournamentId);

            Console.Clear();
            Console.WriteLine("═════════════════════════════════════════════");
            Console.WriteLine($"  Standings of {tournamentName}");
            Console.WriteLine("═════════════════════════════════════════════");

            if (standings?.standings == null || standings.standings.Count == 0)
            {
                Console.WriteLine("No standings found.");
            }
            else
            {
                Console.WriteLine($"{"Rank",4} | {"Player",20} | {"Points",6} | {"W",3}-{"L",3}-{"D",3} | {"Perf",5}");
                Console.WriteLine(new string('-', 60));

                foreach (var standing in standings.standings)
                {
                    string username = standing.username;
                    if (username.Length > 20)
                        username = username.Substring(0, 17) + "...";

                    Console.WriteLine($"{standing.rank,4} | {username,20} | {standing.points,6} | {standing.wins,3}-{standing.losses,3}-{standing.draws,3} | {standing.performance,5}");

                    if (standing.rank % 20 == 0 && standing.rank < standings.standings.Count)
                    {
                        Console.WriteLine("\nPress any key to see more or Escape to return...");
                        var key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Escape) { break; }
                        Console.WriteLine();
                    }
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        static string UnixTimeToDateTime(long? unixTime)
        {
            if (unixTime == null || unixTime == 0) { return "unknown"; }
            return DateTimeOffset.FromUnixTimeSeconds(unixTime.Value).DateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
        static double CalculateWinRate(int wins, int losses, int draws)
        {
            int totalGames = wins + losses + draws;
            if (totalGames == 0) return 0;
            return (double)(wins * 100) / totalGames;
        }
    }
}