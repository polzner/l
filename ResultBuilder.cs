using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
                        if (lex.Type == LexType.FunctionWord)
                        {
                            if (lex.Value == "end")
                            {
                                result += $"\n{lex.Value}";
                                break;
                            }

                            state = ResultBuilderState.Operator;
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

                            Paint(richTextBox, lex.Value);
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
            }
        }

        private void PaintWord(RichTextBox richTextBox, string word)
        {
            richTextBox.Select(0, 0);
            richTextBox.Text += word;
            richTextBox.Select(richTextBox.Text.Length - word.Length, richTextBox.Text.Length);

            richTextBox.SelectionColor = Color.Red;
        }

        private void Paint(RichTextBox richTextBox, string word)
        {
            richTextBox.Text += word;

            if (richTextBox.Find(word) > 0)
            {
                int my1stPosition = richTextBox.Find(word);
                richTextBox.SelectionStart = my1stPosition;
                richTextBox.SelectionLength = word.Length;
                richTextBox.SelectionColor = Color.Red;
            }
        }
    }
}
