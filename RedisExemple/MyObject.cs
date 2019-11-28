using System;
using System.Collections.Generic;
using System.Text;

namespace RedisExemple
{
    [Serializable]
    class MyObject
    {
        public int Int32 { get; set; }
        private protected float Float { get; set; }
        private protected string String { get; set; }
        public MyObject(bool Bool)
        {
            Int32 = int.MaxValue;
            Float = float.MinValue;
            String = "One giant string......................................................................................................................................................";
            Console.WriteLine(Bool ? "Created" : null);
        }
    }
}
