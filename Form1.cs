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

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.DataSource = lexic.Lexemes;

            for (int i = 0; i < 3; i++)
            {
                dataGridView1.Columns[i].Width = 198;
            }

            foreach (var lex in lexic.Lexemes)
            {
                if(lex.Value == ";")
                {
                    result.Append(lex.Id);
                    result.AppendLine();
                }
                else if(lex.Value == "begin")
                {
                    result.Append(lex.Id + "\n");
                    result.AppendLine();
                }
                else
                {
                    result.Append(lex.Id + " ");
                }
            }

            textBox2.Text = result.ToString();

            ResultBuilder resultBuilder = new ResultBuilder();
            resultBuilder.Generate(richTextBox1, lexic.Lexemes.ToList());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
