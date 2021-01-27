using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        private readonly List<int> SuperAuthorized = new List<int>() {  };

        public List<int> Authorized { get; internal set; } = new List<int>();

        private List<int> Combined => SuperAuthorized.Concat(Authorized).ToList();

        public bool IsAuthorized(int id) => Combined.Concat(Authorized).Any(r => r == id);

        public void Authorize(int id)
        {
            if (Combined.Any(r => r == id))
                return;

            Authorized.Add(id);
        }

        public void Authorize(params int[] ids) => Authorized.AddRange(ids.Where(r => !Combined.Any(s => s == r)).ToList());


        public void Deauthorize(int id) => Authorized.Remove(id);


        public void Deauthorize(params int[] ids) => Authorized.RemoveAll(r => ids.Any(s => s == r));
    }
}