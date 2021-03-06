﻿/*
        Copyright © Iain McDonald 2013-2016
        This file is part of Cm93.

        Cm93 is free software: you can redistribute it and/or modify
        it under the terms of the GNU General Public License as published by
        the Free Software Foundation, either version 3 of the License, or
        (at your option) any later version.

        Cm93 is distributed in the hope that it will be useful,
        but WITHOUT ANY WARRANTY; without even the implied warranty of
        MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
        GNU General Public License for more details.

        You should have received a copy of the GNU General Public License
        along with Cm93. If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cm93.Model.Config;
using Cm93.Model.Enumerations;
using Cm93.Model.Helpers;
using Cm93.Model.Interfaces;
using Cm93.Model.Structures;
using Cm93.GameEngine.Basic.AI;

namespace Cm93.GameEngine.Basic
{
	public class BasicGameEngine : IGameEngine
	{
		private PlayerBids PlayerBids { get; set; }
		private IArtificialIntelligence ArtificialIntelligence { get; set; }

		public BasicGameEngine()
		{
			PlayerBids = new PlayerBids();
			ArtificialIntelligence = new ArtificialIntelligence();
		}

		#region Player Bids

		public ILookup<Team, Bid> TeamBids
		{
			get { return PlayerBids.TeamBids; }
		}

		public void SubmitBid(Bid bid)
		{
			PlayerBids.SubmitBid(bid);
		}

		public void ProcessTransfers()
		{
			PlayerBids.ProcessTransfers();
		}

		#endregion

		#region Match Simulator

		public void Play(IFixture fixture, IDictionary<int, Player> homeTeamFormation, IDictionary<int, Player> awayTeamFormation, Action<double, double[,]> updateUi)
		{
			if (fixture.TeamHome.TeamName == Configuration.PlayerTeamName)
			{
				ArtificialIntelligence.SelectStartingFormation(awayTeamFormation, fixture.TeamAway, fixture.TeamHome);
				awayTeamFormation.Values.Execute(p => p.Location.Invert());
			}
			else if (fixture.TeamAway.TeamName == Configuration.PlayerTeamName)
			{
				ArtificialIntelligence.SelectStartingFormation(homeTeamFormation, fixture.TeamHome, fixture.TeamAway);
			}
			else
			{
				ArtificialIntelligence.SelectStartingFormation(homeTeamFormation, fixture.TeamHome, fixture.TeamAway);
				ArtificialIntelligence.SelectStartingFormation(awayTeamFormation, fixture.TeamAway, fixture.TeamHome);
				awayTeamFormation.Values.Execute(p => p.Location.Invert());
			}

			var matchSimulator = new MatchSimulator(homeTeamFormation, awayTeamFormation);

			matchSimulator.Play(fixture, updateUi);
		}

		#endregion

		#region Competitions

		public IList<ICompetition> Competitions(IList<Team> teams, IDictionary<string, Dictionary<Team, Place>> places)
		{
			return new CompetitionImpl(teams, places).CompetitionsWithFixtures();
		}

		#endregion
	}
}
