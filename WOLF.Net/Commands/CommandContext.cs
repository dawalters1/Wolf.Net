using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Group;
using WOLF.Net.Entities.Subscriber;

namespace WOLF.Net.Commands
{
    public abstract class CommandContext
    {
		//public WolfClient Client { get; set; }

		public CommandData Command { get; set; }

	}
}
