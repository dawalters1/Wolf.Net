using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;

namespace WOLF.Net.Commands.Attributes
{
    public class TagRole : BaseAttribute
    {
        private Enums.Subscribers.Privilege[] _privileges;

        public TagRole(params Enums.Subscribers.Privilege[] privileges)
        {
            if (privileges.Length == 0)
                throw new Exception("Privileges cannot be null");

            _privileges = privileges;
        }

        public override bool Validate(WolfBot bot, CommandData command)
        {
            var validateResult = _privileges.Any(r => command.Subscriber.Privileges.HasFlag(r));

            //Trigger failed perms if this is false

            Console.WriteLine(validateResult);

            return validateResult;
        }
    }
}