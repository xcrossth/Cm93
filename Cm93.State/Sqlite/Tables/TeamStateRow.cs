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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cm93.State.Sqlite.Tables
{
	[Table("TeamStates")]
	public class TeamStateRow
	{
		[Key]
		[Column(Order = 1)]
		public long TeamId { get; set; }

		[Key]
		[Column(Order = 2)]
		public long StateId { get; set; }

		public long Balance { get; set; }
		public long Division { get; set; }

		public virtual TeamRow Team { get; set; }
		public virtual StateRow State { get; set; }
	}
}
