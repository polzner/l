using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax
{
    public class Lex
    {
        public string Id { get; private set; }
        public string Value { get; private set; }
        public LexType Type { get; private set; }

        public Lex(LexType type, string value, string id)
        {
            Type = type;
            Value = value;
            Id = id;
        }
    }
}
