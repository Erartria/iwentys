﻿using System;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Models.Transferable
{
    public class TournamentInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TournamentType Type { get; set; }

        public static TournamentInfoResponse Wrap(TournamentEntity tournamentEntity)
        {
            return new TournamentInfoResponse
            {
                Id = tournamentEntity.Id,
                Name = tournamentEntity.Name,
                Description = tournamentEntity.Description,
                StartTime = tournamentEntity.StartTime,
                EndTime = tournamentEntity.EndTime,
                Type = tournamentEntity.Type
            };
        }
    }
}