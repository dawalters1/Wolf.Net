using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        private List<int> SuperAuthorized = new List<int>() { 29976610 };

        public List<int> Authorized = new List<int>();

        private List<int> combined => SuperAuthorized.Concat(Authorized).ToList();

        public bool IsAuthorized(int id) => combined.Concat(Authorized).Any(r => r == id);

        public void Authorize(int id)
        {
            if (combined.Any(r => r == id))
                return;

            Authorized.Add(id);
        }

        public void Authorize(params int[] ids) => Authorized.AddRange(ids.Where(r => !combined.Any(s => s == r)).ToList());


        public void Deauthorize(int id) => Authorized.Remove(id);


        public void Deauthorize(params int[] ids) => Authorized.RemoveAll(r => ids.Any(s => s == r));
    }
}