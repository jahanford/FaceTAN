using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceTAN.Core.Data
{
    public class ApiKeyStore
    {
        public ApiKeyStore(string[] keys)
        {
            ApiKeys = new LinkedList<string>(keys);
            CurrentKey = ApiKeys.First;
        }

        private LinkedList<string> ApiKeys;
        private LinkedListNode<string> CurrentKey;

        public string GetCurrentKey()
        {
            return CurrentKey.Value;
        }

        public void NextKey()
        {
            if (CurrentKey != ApiKeys.Last)
                CurrentKey = CurrentKey.Next;
            else 
                CurrentKey = ApiKeys.First;
        }
    }
}
