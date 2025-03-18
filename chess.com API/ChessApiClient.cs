using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChessAPI
{
    public class ChessApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.chess.com/pub";

        public ChessApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "ChessApiClient/1.0");
        }
        private async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(BaseUrl + endpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching data: {response.StatusCode}");
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Deserialize<T>(jsonString, options);
        }

        #region Player Methods
        public async Task<PlayerProfile> GetPlayerProfile(string username)
        {
            return await GetAsync<PlayerProfile>($"/player/{username}");
        }
        public async Task<PlayerStats> GetPlayerStats(string username)
        {
            return await GetAsync<PlayerStats>($"/player/{username}/stats");
        }
        public async Task<List<Game>> GetPlayerGameArchive(string username, int year, int month)
        {
            var result = await GetAsync<GameArchive>($"/player/{username}/games/{year}/{month:00}");
            return result?.games ?? new List<Game>();
        }
        public async Task<List<string>> GetPlayerGameArchives(string username)
        {
            var result = await GetAsync<ArchivesResponse>($"/player/{username}/games/archives");
            return result?.archives ?? new List<string>();
        }
        public async Task<List<ClubProfile>> GetPlayerClubs(string username)
        {
            var result = await GetAsync<PlayerClubsResponse>($"/player/{username}/clubs");
            return result?.clubs ?? new List<ClubProfile>();
        }

        #endregion
        #region Club Methods
        public async Task<ClubProfile> GetClubProfile(string urlId)
        {
            return await GetAsync<ClubProfile>($"/club/{urlId}");
        }
        public async Task<List<ClubMember>> GetClubMembers(string urlId)
        {
            var result = await GetAsync<ClubMembersResponse>($"/club/{urlId}/members");
            return result?.members ?? new List<ClubMember>();
        }
        public async Task<List<Tournament>> GetClubTournaments(string urlId)
        {
            var result = await GetAsync<TournamentsResponse>($"/club/{urlId}/tournaments");
            return result?.tournaments ?? new List<Tournament>();
        }
        #endregion
        #region Tournament Methods
        public async Task<Tournament> GetTournament(string tournamentId)
        {
            return await GetAsync<Tournament>($"/tournament/{tournamentId}");
        }
        public async Task<List<TournamentRound>> GetTournamentRounds(string tournamentId)
        {
            var result = await GetAsync<TournamentRoundsResponse>($"/tournament/{tournamentId}/rounds");
            return result?.rounds ?? new List<TournamentRound>();
        }
        public async Task<TournamentStandings> GetTournamentStandings(string tournamentId)
        {
            return await GetAsync<TournamentStandings>($"/tournament/{tournamentId}/standings");
        }
        #endregion
        public async Task<List<GameInfo>> GetGamesFromArchive(string archiveUrl)
        {
            var result = await GetAsync<GamesResponse>(archiveUrl.Replace(BaseUrl, ""));
            return result?.Games ?? new List<GameInfo>();
        }
        public async Task<DailyPuzzle> GetDailyPuzzle()
        {
            return await GetAsync<DailyPuzzle>("/puzzle");
        }
        public async Task<List<string>> GetTitledPlayers(string title)
        {
            var result = await GetAsync<TitledPlayersResponse>($"/titled/{title}");
            return result?.players ?? new List<string>();
        }
        public async Task<Leaderboard> GetLeaderboard()
        {
            return await GetAsync<Leaderboard>("/leaderboards");
        }
    }

    #region Player Models
    public class PlayerProfile
    {
        [JsonPropertyName("username")] public string username { get; set; }
        [JsonPropertyName("name")] public string name { get; set; }
        [JsonPropertyName("title")] public string title { get; set; }
        [JsonPropertyName("url")] public string url { get; set; }
        [JsonPropertyName("avatar")] public string avatar { get; set; }
        [JsonPropertyName("followers")] public int followers { get; set; }
        [JsonPropertyName("country")] public string country { get; set; }
        [JsonPropertyName("last_online")] public long last_online { get; set; }
        [JsonPropertyName("joined")] public long joined { get; set; }
        [JsonPropertyName("is_streamer")] public bool is_streamer { get; set; }
        [JsonPropertyName("status")] public string status { get; set; }
        [JsonPropertyName("league")] public string league { get; set; }
        [JsonPropertyName("location")] public string location { get; set; }
        [JsonPropertyName("twitch_url")] public string twitch_url { get; set; }
    }
    public class PlayerStats
    {
        [JsonPropertyName("chess_blitz")] public ChessStats chess_blitz { get; set; }
        [JsonPropertyName("chess_bullet")] public ChessStats chess_bullet { get; set; }
        [JsonPropertyName("chess_rapid")] public ChessStats chess_rapid { get; set; }
        [JsonPropertyName("chess_daily")] public ChessStats chess_daily { get; set; }
        [JsonPropertyName("tactics")] public TacticsStats tactics { get; set; }
        [JsonPropertyName("lessons")] public LessonsStats lessons { get; set; }
        [JsonPropertyName("puzzle_rush")] public PuzzleRushStats puzzle_rush { get; set; }

        public class ChessStats
        {
            [JsonPropertyName("last")] public RatingStats last { get; set; }
            [JsonPropertyName("best")] public RatingStats best { get; set; }
            [JsonPropertyName("record")] public RecordStats record { get; set; }
        }

        public class RatingStats
        {
            [JsonPropertyName("rating")] public int rating { get; set; }
            [JsonPropertyName("date")] public long date { get; set; }
            [JsonPropertyName("rd")] public int rd { get; set; }
        }

        public class RecordStats
        {
            [JsonPropertyName("win")] public int win { get; set; }
            [JsonPropertyName("loss")] public int loss { get; set; }
            [JsonPropertyName("draw")] public int draw { get; set; }
            [JsonPropertyName("time_per_move")] public int? time_per_move { get; set; }
            [JsonPropertyName("timeout_percent")] public double? timeout_percent { get; set; }
        }

        public class TacticsStats
        {
            [JsonPropertyName("highest")] public RatingStats highest { get; set; }
            [JsonPropertyName("lowest")] public RatingStats lowest { get; set; }
        }

        public class LessonsStats
        {
            [JsonPropertyName("highest")] public RatingStats highest { get; set; }
            [JsonPropertyName("lowest")] public RatingStats lowest { get; set; }
        }

        public class PuzzleRushStats
        {
            [JsonPropertyName("best")] public PuzzleRushBest best { get; set; }

            public class PuzzleRushBest
            {
                [JsonPropertyName("total_attempts")] public int total_attempts { get; set; }
                [JsonPropertyName("score")] public int score { get; set; }
            }
        }
    }
    public class PlayerClubsResponse
    {
        [JsonPropertyName("clubs")] public List<ClubProfile> clubs { get; set; }
    }
    #endregion
    #region Game Models
    public class GameArchive
    {
        [JsonPropertyName("games")] public List<Game> games { get; set; }
    }
    public class Game
    {
        [JsonPropertyName("url")] public string url { get; set; }
        [JsonPropertyName("pgn")] public string pgn { get; set; }
        [JsonPropertyName("time_control")] public string time_control { get; set; }
        [JsonPropertyName("end_time")] public long end_time { get; set; }
        [JsonPropertyName("rated")] public bool rated { get; set; }
        [JsonPropertyName("tcn")] public string tcn { get; set; }
        [JsonPropertyName("uuid")] public string uuid { get; set; }
        [JsonPropertyName("initial_setup")] public string initial_setup { get; set; }
        [JsonPropertyName("fen")] public string fen { get; set; }
        [JsonPropertyName("time_class")] public string time_class { get; set; }
        [JsonPropertyName("rules")] public string rules { get; set; }
        [JsonPropertyName("white")] public Player white { get; set; }
        [JsonPropertyName("black")] public Player black { get; set; }

        public class Player
        {
            [JsonPropertyName("username")] public string username { get; set; }
            [JsonPropertyName("rating")] public int rating { get; set; }
            [JsonPropertyName("result")] public string result { get; set; }
            [JsonPropertyName("@id")] public string id { get; set; }
            [JsonPropertyName("uuid")] public string uuid { get; set; }
        }

        public string GetEndTimeAsDateTime()
        {
            return DateTimeOffset.FromUnixTimeSeconds(end_time).DateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
    public class ArchivesResponse
    {
        [JsonPropertyName("archives")] public List<string> archives { get; set; }
    }
    public class GameInfo
    {
        public string Url { get; set; }
        public string Pgn { get; set; }
        public long EndTime { get; set; }
        public GameData White { get; set; }
        public GameData Black { get; set; }
        public string TimeClass { get; set; }
        public string TimeControl { get; set; }
        public string Result { get; set; }

        public override string ToString()
        {
            var endDate = DateTimeOffset.FromUnixTimeMilliseconds(EndTime).DateTime.ToLocalTime();
            return $"{White?.Username} ({White?.Rating}) vs {Black?.Username} ({Black?.Rating}) - {Result} - {TimeClass} - {endDate.ToShortDateString()}";
        }
    }
    public class GameData
    {
        public string Username { get; set; }
        public int Rating { get; set; }
        public string Result { get; set; }
    }
    public class GamesResponse
    {
        public List<GameInfo> Games { get; set; }
    }
    #endregion
    #region Club Models
    public class ClubProfile
    {
        [JsonPropertyName("@id")] public string id { get; set; }
        [JsonPropertyName("name")] public string name { get; set; }
        [JsonPropertyName("club_id")] public int club_id { get; set; }
        [JsonPropertyName("url")] public string url { get; set; }
        [JsonPropertyName("icon")] public string icon { get; set; }
        [JsonPropertyName("country")] public string country { get; set; }
        [JsonPropertyName("average_daily_rating")] public int average_daily_rating { get; set; }
        [JsonPropertyName("members_count")] public int members_count { get; set; }
        [JsonPropertyName("created")] public long created { get; set; }
        [JsonPropertyName("last_activity")] public long last_activity { get; set; }
        [JsonPropertyName("admin")] public List<string> admin { get; set; }
        [JsonPropertyName("description")] public string description { get; set; }
        [JsonPropertyName("visibility")] public string visibility { get; set; }
        [JsonPropertyName("join_request")] public string join_request { get; set; }

        public string GetCreatedAsDateTime()
        {
            return DateTimeOffset.FromUnixTimeSeconds(created).DateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string GetLastActivityAsDateTime()
        {
            return DateTimeOffset.FromUnixTimeSeconds(last_activity).DateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
    public class ClubMembersResponse
    {
        [JsonPropertyName("members")] public List<ClubMember> members { get; set; }
    }
    public class ClubMember
    {
        [JsonPropertyName("username")] public string username { get; set; }
        [JsonPropertyName("joined")] public long joined { get; set; }

        public string GetJoinedAsDateTime()
        {
            return DateTimeOffset.FromUnixTimeSeconds(joined).DateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
    #endregion
    #region Tournament Models
    public class TournamentsResponse
    {
        [JsonPropertyName("tournaments")] public List<Tournament> tournaments { get; set; }
    }
    public class Tournament
    {
        [JsonPropertyName("url")] public string url { get; set; }
        [JsonPropertyName("@id")] public string id { get; set; }
        [JsonPropertyName("name")] public string name { get; set; }
        [JsonPropertyName("status")] public string status { get; set; }
        [JsonPropertyName("start_time")] public long start_time { get; set; }
        [JsonPropertyName("end_time")] public long end_time { get; set; }
        [JsonPropertyName("settings")] public TournamentSettings settings { get; set; }

        public class TournamentSettings
        {
            [JsonPropertyName("type")] public string type { get; set; }
            [JsonPropertyName("time_class")] public string time_class { get; set; }
            [JsonPropertyName("time_control")] public string time_control { get; set; }
            [JsonPropertyName("rated")] public bool rated { get; set; }
            [JsonPropertyName("variant")] public string variant { get; set; }
            [JsonPropertyName("at_team_tour")] public bool at_team_tour { get; set; }
        }

        public string GetStartTimeAsDateTime()
        {
            return DateTimeOffset.FromUnixTimeSeconds(start_time).DateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string GetEndTimeAsDateTime()
        {
            return DateTimeOffset.FromUnixTimeSeconds(end_time).DateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
    public class TournamentRoundsResponse
    {
        [JsonPropertyName("rounds")] public List<TournamentRound> rounds { get; set; }
    }
    public class TournamentRound
    {
        [JsonPropertyName("url")] public string url { get; set; }
        [JsonPropertyName("@id")] public string id { get; set; }
        [JsonPropertyName("name")] public string name { get; set; }
        [JsonPropertyName("games")] public List<TournamentGame> games { get; set; }

        public class TournamentGame
        {
            [JsonPropertyName("url")] public string url { get; set; }
            [JsonPropertyName("white")] public string white { get; set; }
            [JsonPropertyName("black")] public string black { get; set; }
            [JsonPropertyName("pgn")] public string pgn { get; set; }
            [JsonPropertyName("result")] public string result { get; set; }
            [JsonPropertyName("time_control")] public string time_control { get; set; }
            [JsonPropertyName("end_time")] public long end_time { get; set; }
        }
    }
    public class TournamentStandings
    {
        [JsonPropertyName("standings")] public List<TournamentStanding> standings { get; set; }

        public class TournamentStanding
        {
            [JsonPropertyName("rank")] public int rank { get; set; }
            [JsonPropertyName("player_id")] public string player_id { get; set; }
            [JsonPropertyName("username")] public string username { get; set; }
            [JsonPropertyName("points")] public double points { get; set; }
            [JsonPropertyName("tie_break")] public double tie_break { get; set; }
            [JsonPropertyName("performance")] public int performance { get; set; }
            [JsonPropertyName("wins")] public int wins { get; set; }
            [JsonPropertyName("losses")] public int losses { get; set; }
            [JsonPropertyName("draws")] public int draws { get; set; }
        }
    }
    #endregion
    public class DailyPuzzle
    {
        [JsonPropertyName("title")] public string title { get; set; }
        [JsonPropertyName("url")] public string url { get; set; }
        [JsonPropertyName("publish_time")] public long? publish_time { get; set; }
        [JsonPropertyName("fen")] public string fen { get; set; }
        [JsonPropertyName("pgn")] public string pgn { get; set; }
        [JsonPropertyName("image")] public string image { get; set; }

        public string GetPublishTimeAsDateTime()
        {
            if (publish_time.HasValue)
            {
                return DateTimeOffset.FromUnixTimeSeconds(publish_time.Value).DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return "unknown";
        }
    }
    public class TitledPlayersResponse
    {
        [JsonPropertyName("players")] public List<string> players { get; set; }
    }
    public class Leaderboard
    {
        [JsonPropertyName("daily")] public List<LeaderboardPlayer> daily { get; set; }
        [JsonPropertyName("daily960")] public List<LeaderboardPlayer> daily960 { get; set; }
        [JsonPropertyName("live_blitz")] public List<LeaderboardPlayer> live_blitz { get; set; }
        [JsonPropertyName("live_bullet")] public List<LeaderboardPlayer> live_bullet { get; set; }
        [JsonPropertyName("live_rapid")] public List<LeaderboardPlayer> live_rapid { get; set; }
        [JsonPropertyName("tactics")] public List<LeaderboardPlayer> tactics { get; set; }

        public class LeaderboardPlayer
        {
            [JsonPropertyName("username")] public string username { get; set; }
            [JsonPropertyName("rank")] public int rank { get; set; }
            [JsonPropertyName("score")] public int score { get; set; }
            [JsonPropertyName("avatar")] public string avatar { get; set; }
            [JsonPropertyName("title")] public string title { get; set; }
            [JsonPropertyName("name")] public string name { get; set; }
            [JsonPropertyName("url")] public string url { get; set; }
        }
    }
}