using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntax
{
    public class IdGenerator
    {
        private Dictionary<(LexType, string), int> _id = new Dictionary<(LexType, string), int>();

        public string GetId(LexType type, string lexValue)
        {
            switch (type)
            {
                case LexType.FunctionWord:
                    return "F" + GetNumber(LexType.FunctionWord, lexValue).ToString();
                case LexType.Delimeter:
                    return "D" + GetNumber(LexType.Delimeter, lexValue).ToString();
                case LexType.Number:
                    return "N" + GetNumber(LexType.Number, lexValue).ToString();
                case LexType.Identifier:
                    return "I" + GetNumber(LexType.Identifier, lexValue).ToString();
            }

            throw new ArgumentException(nameof(type));
        }

        private int GetNumber(LexType type, string lexValue)
        {
            var key = (type, lexValue);

            if (_id.Keys.Contains(key))
            {
                return _id[key];
            }
            else
            {
                var keysByType = _id.Keys.Where(a => a.Item1 == type);

                if (keysByType.Count() != 0)
                {
                    int lastNumberByType = _id[keysByType.Last()];
                    int currentNumber = lastNumberByType + 1;
                    _id.Add(key, currentNumber);
                    return currentNumber;
                }

                _id.Add(key, 0);
                return _id[key];
            }
        }
    }
}