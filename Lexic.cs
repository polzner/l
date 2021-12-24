using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Syntax
{
    public class Lexic
    {
        private string[] Words = { "program", "var", "integer", "real", "bool", "begin",
                                    "end", "if", "then", "else", "while", "do", "read", 
                                    "write", "true", "false", "and", "or", "xor"};
        private string[] Delimiter = {";", ",", "(", ")", "+", "-", "*", "/", "=", ">", "<" };
        private List<Lex> _lexemes = new List<Lex>();
        private string _wordBufer;
        private StringReader _stringReader;
        private char[] _currentSymbol = new char[1];
        private State _state;
        private IdGenerator _id;

        public Lexic(IdGenerator id)
        {
            _id = id;
        }

        public IEnumerable<Lex> Lexemes => _lexemes;

        public void Analysis(string text)
        {
            _stringReader = new StringReader(text + '.');

            while(_state != State.Final)
            {
                char currentSymbol = _currentSymbol[0];

                switch (_state)
                {
                    case State.Start:
                        if (currentSymbol == ' ' || currentSymbol == '\n'
                            || currentSymbol == '\t' || currentSymbol == '\0' || currentSymbol == '\r')
                        {
                            SetNextSymbol();
                        }
                        else if (char.IsLetter(currentSymbol))
                        {
                            ClearBufer();
                            AddInBufer(currentSymbol);
                            SetNextSymbol();
                            _state = State.Word;
                        }
                        else if (char.IsDigit(currentSymbol))
                        {
                            ClearBufer();
                            AddInBufer(currentSymbol);
                            SetNextSymbol();
                            _state = State.Number;
                        }
                        else if (currentSymbol == ':')
                        {
                            ClearBufer();
                            AddInBufer(currentSymbol);
                            SetNextSymbol();
                            _state = State.Assign;
                        }
                        else if (currentSymbol == '.')
                        {
                            _state = State.Final;
                        }
                        else
                        {
                            _state = State.Delimiter;
                        }
                        break;

                    case State.Number:
                        if (char.IsDigit(currentSymbol))
                        {
                            AddInBufer(currentSymbol);
                            SetNextSymbol();
                        }
                        else
                        {
                            AddLex(LexType.Number, _wordBufer);
                            ClearBufer();
                            _state = State.Start;
                        }
                        break;

                    case State.Delimiter:
                        ClearBufer();
                        AddInBufer(currentSymbol);

                        if (Delimiter.Contains(_wordBufer))
                        {
                            AddLex(LexType.Delimeter, _wordBufer);
                            SetNextSymbol();
                            _state = State.Start;
                        }
                        else
                        {
                            _state = State.Error;
                        }
                        break;

                    case State.Word:
                        if (char.IsLetterOrDigit(currentSymbol))
                        {
                            AddInBufer(currentSymbol);
                            SetNextSymbol();
                        }
                        else
                        {
                            if (Words.Contains(_wordBufer))
                            {
                                AddLex(LexType.FunctionWord, _wordBufer);
                            }
                            else
                            {
                                AddLex(LexType.Identifier, _wordBufer);
                            }
                            _state = State.Start;
                        }
                        break;                    

                    case State.Assign:
                        if (currentSymbol == '=')
                        {
                            AddInBufer(currentSymbol);
                            AddLex(LexType.Delimeter, _wordBufer);
                            ClearBufer();
                            SetNextSymbol();
                        }
                        else
                        {
                            AddLex(LexType.Delimeter, _wordBufer);
                        }
                        _state = State.Start;
                        break;

                    case State.Error:
                        MessageBox.Show("Ошибка в программе");
                        _state = State.Final;
                        break;
                }
            }
        }

        private void AddLex(LexType type, string name)
        {
            string id = _id.GetId(type, name);
            _lexemes.Add(new Lex(type, name, id));
        }

        private void ClearBufer()
        {
            _wordBufer = "";
        }

        private void AddInBufer(char currentSymbol)
        {
            _wordBufer += currentSymbol;
        }

        public void SetNextSymbol()
        {
            _stringReader.Read(_currentSymbol, 0, 1);
        }
    }
}