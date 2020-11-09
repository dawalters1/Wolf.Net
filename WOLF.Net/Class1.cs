using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WOLF.Net.Commands;
using WOLF.Net.Commands.Attributes;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Constants;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net
{
    public class Class1
    {
        public static void Main(string[] args)
                   => new Class1().Main().GetAwaiter().GetResult();

        public async Task Main()
        {
            var test = new CommandManager();

            //test.ProcessMessage(); //Cannot create placeholder message for this test anymore be does work (or did)

            await Task.Delay(-1);
        }

        public void Test()
        {
           // Event.
        }
    }

    [CommandCollection("test"), MessageType(Enums.Messages.MessageType.Both)]
    public class Test: CommandContext
    {

        [Command("test1"), Default]
        public async Task Test1()
        {
            Console.WriteLine(JsonConvert.SerializeObject(Command, Formatting.Indented));
        }
        [Command("test5"), Default]
        public async Task Test5()
        {
            Console.WriteLine(JsonConvert.SerializeObject(Command, Formatting.Indented));
        }
        [Command("test8"), Default]
        public async Task Test8()
        {
            Console.WriteLine(JsonConvert.SerializeObject(Command, Formatting.Indented));
        }
    }
}
