using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Syntax
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            textBox2.Clear();
            Lexic lexic = new Lexic(new IdGenerator());
            lexic.Analysis(textBox1.Text);
            StringBuilder result = new StringBuilder();

            foreach (var lex in lexic.Lexemes)
            {
                result.Append(lex.Id + " ");
            }

            textBox2.Text = result.ToString();

            ResultBuilder resultBuilder = new ResultBuilder();
            resultBuilder.Generate(richTextBox1, lexic.Lexemes.ToList());
        }
    }
}
