﻿using SengokuProvider.Library.Models.Common;

namespace SengokuProvider.Library.Models.Leagues
{
    public class OnboardTournamentToLeagueCommand : ICommand
    {
        public required int[] TournamentIds { get; set; }
        public required int LeagueId { get; set; }
        public CommandRegistry Topic { get; set; }
        public string? Response { get; set; }

        public bool Validate()
        {
            if (TournamentIds.Length > 0 && LeagueId > 0) return true;
            return false;
        }
    }
    public class OnboardPlayerToLeagueCommand : ICommand
    {
        public required int[] PlayerIds { get; set; }
        public required int LeagueId { get; set; }
        public CommandRegistry Topic { get; set; }
        public string? Response { get; set; }

        public bool Validate()
        {
            if (PlayerIds.Length > 0 && LeagueId > 0) return true;
            return false;
        }
    }
    public class GetLeaderboardResultsByLeagueCommand : ICommand
    {
        public required int LeagueId { get; set; }
        public CommandRegistry Topic { get; set; }
        public string? Response { get; set; }

        public bool Validate()
        {
            if (LeagueId > 0) return true;
            return false;
        }
    }
    public class GetLeaderboardsByOrgCommand : ICommand
    {
        public required int OrgId { get; set; }
        public CommandRegistry Topic { get; set; }
        public string? Response { get; set; }

        public bool Validate()
        {
            if (OrgId > 0) return true;
            return false;
        }
    }
    public class CreateLeagueByOrgCommand : ICommand
    {
        public required string LeagueName { get; set; }
        public required int OrgId { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public int GameId { get; set; }
        public string? Description { get; set; }
        public CommandRegistry Topic { get; set; }
        public string? Response { get; set; }

        public bool Validate()
        {
            if (!string.IsNullOrEmpty(LeagueName) &&
                !string.IsNullOrEmpty(LeagueName) &&
                OrgId > 0 && GameId > 0) return true;
            return false;
        }
    }
}
