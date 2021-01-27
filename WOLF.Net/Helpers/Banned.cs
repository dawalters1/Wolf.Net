using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        public List<int> Banned { get; internal set; } = new List<int>();

        public bool IsBanned(int id) => Banned.Any(r => r == id);

        public void Ban(params int[] ids) => Banned.AddRange(ids.Where(r => !Banned.Any(s => s == r)).ToList());

        public void Unban(params int[] ids) => Banned.RemoveAll(r => ids.Any(s => s == r));
    }
}