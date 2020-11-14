using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Subscribers;

namespace WOLF.Net.Commands.Commands
{
    public abstract class CommandContext
    {
		//public WolfClient Client { get; set; }

		public WolfBot Bot { get; set; }
		public CommandData Command { get; set; }

	}
}
