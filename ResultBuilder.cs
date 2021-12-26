using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Syntax
{
    public class ResultBuilder
    {
        public void Generate(RichTextBox richTextBox, List<Lex> lexems)
        {
            richTextBox.Clear();
            string ident = "    ";
            string currentIdent = "";
            string result = "";
            int bracketBuffer = 0;
            ResultBuilderState state = ResultBuilderState.Program;

            foreach (Lex lex in lexems)
            {
                switch (state)
                {
                    case ResultBuilderState.Program:
                        if (lex.Value == ";")
                        {
                            result += $"{lex.Value}\n{currentIdent}";
                        }
                        else if (lex.Value == "begin")
                        {
                            state = ResultBuilderState.Main;
                            currentIdent += ident;
                            result += $"{lex.Value}\n{currentIdent}";
                        }
                        else
                        {
                            result += lex.Value;
                        }

                        break;

                    case ResultBuilderState.Main:
                        if (lex.Type == LexType.FunctionWord && lex.Value != "or" && lex.Value != "xor")
                        {                           
                            if (lex.Value == "end")
                            {
                                result += $"\n{lex.Value}";
                                break;
                            }

                            state = ResultBuilderState.Operator;
                        }

                        if (lex.Value == "or" || lex.Value == "xor")
                        {
                            result += $" {lex.Value} ";
                            break;
                        }

                        if (lex.Value == ";")
                        {
                            result += $"{lex.Value}\n{currentIdent}";
                            break;
                        }

                        result += $"{lex.Value}";
                        break;

                    case ResultBuilderState.Operator:
                        if(lex.Value == "xor" || lex.Value == "or")
                        {
                            state = ResultBuilderState.Main;
                            break;
                        }

                        result += lex.Value;

                        if (lex.Value == "(")
                        {
                            bracketBuffer++;
                        }
                        if (lex.Value == ")")
                        {
                            bracketBuffer--;
                        }
                        if (bracketBuffer == 0)
                        {
                            state = ResultBuilderState.Main;
                            currentIdent += ident;
                            result += $"\n{currentIdent}";
                        }
                        break;
                }

                richTextBox.Text += result;
                result = "";
                Paint(richTextBox, "xor");
                Paint(richTextBox, "or");
            }
        }

        private void Paint(RichTextBox richTextBox, string word)
        {
            var currentSelStart = richTextBox.SelectionStart;
            var currentSelLength = richTextBox.SelectionLength;

            richTextBox.SelectAll();

            var matches = Regex.Matches(richTextBox.Text, word);
            foreach (var match in matches.Cast<Match>())
            {
                richTextBox.Select(match.Index, match.Length);
                richTextBox.SelectionColor = Color.Red;
            }

            richTextBox.Select(currentSelStart, currentSelLength);
            richTextBox.SelectionColor = SystemColors.WindowText;
        }
    }
}
