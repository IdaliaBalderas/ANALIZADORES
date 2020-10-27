using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using frmMain;

namespace ANALIZADORES
{
    public partial class Form3 : Form
    {

        String numLineas;
        List<String> palabrasReservadas;
        bool checkJ = false;
        bool checkC = false;
        Font font;
        String code;
        List<String> palabrasReservadas2;
        Expresiones csLexer = new Expresiones();
        bool load;

        public Form3()
        {
            InitializeComponent();
            nLineas.Text = "";
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (checkC == true)
            {

            }
        }

        int numero;
        //para mostrar el numero de lineas del codigo
        public void calculaLineas()
        {
            numLineas = insercion.Text;
            String[] numero = numLineas.Split(new char[] { '\n' });
            int contador = numero.Count();
            for (int i = 0; i < contador; i++)
            {
                String cont = i.ToString() + "" + System.Environment.NewLine;
                nLineas.Text += cont;
            }
        }

        private void evaluaCodigo()
        {
            datos.Rows.Clear();

            int n = 0, e = 0;

            foreach (var tk in csLexer.GetTokens(insercion.Text))
            {
                if (tk.Name == "SIMBOLO RECONOCIDO") e++;

                if (tk.Name == "IDENTIFICADOR")
                    if (palabrasReservadas.Contains(tk.Lexema))
                        tk.Name = "RESERVADO";
                datos.Rows.Add(tk.Name, tk.Lexema, tk.Index, tk.Linea, tk.Columna);
                n++;
            }

        }

        private void Parse()
        {
            grammar gramatica = new grammar();
            LanguageData languaje = new LanguageData(gramatica);
            Parser parser = new Parser(languaje);
            ParseTree parseTree = parser.Parse(insercion.Text);
            ParseTreeNode node = parseTree.Root;

            if (node == null)
            {
                textBox1.ForeColor = Color.Red;
                textBox1.Text = ">>>>>";
                insercion.ForeColor = Color.Red;

                for (int i = 0; i < parseTree.ParserMessages.Count; i++)
                {
                    textBox1.Text += parseTree.ParserMessages[i].Message + "\n---LINEA----: " + parseTree.ParserMessages[i].Location.Line + "\n";
                }
            }
            else
            {
                textBox1.ForeColor = Color.Black;
                insercion.ForeColor = Color.Green;
                textBox1.Text = "---CORRECTO---" + "\n";
                var arbol = new ParseTreeClass(parseTree);
                var nodes = arbol.Traverse();

                textBox1.Text += ("\nArbol:\n" + arbol);
            }
        }

        public void javaX()
        {

            using (System.IO.StreamReader sr = new StreamReader(@"..\..\Expresiones.cs"))
            {

                csLexer.AddTokenRule(@"\s+", "ESPACIO", true);
                csLexer.AddTokenRule(@"[\(\)\{\}\[\];,]", "DELIMITADOR");
                csLexer.AddTokenRule(@"[\.=\+\-/*%]", "OPERADOR");
                csLexer.AddTokenRule(@">|<|==|>=|<=|!", "COMPARADOR");
                csLexer.AddTokenRule(@"\d*\.?\d+", "NUMERO");
                csLexer.AddTokenRule(@"\b[_a-zA-Z][\w]*\b", "IDENTIFICADOR");
                csLexer.AddTokenRule("\".*?\"", "CADENA");
                csLexer.AddTokenRule(@"'\\.'|'[^\\]'", "CARACTER");
                csLexer.AddTokenRule("//[^\r\n]*", "COMENTARIO1");
                csLexer.AddTokenRule("/[*].*?[*]/", "COMENTARIO2");

                palabrasReservadas = new List<string>() {"abstract", "boolean", "break", "byte",
                "byvalue", "case", "catch", "char", "class", "const", "continue", "default",
                "do", "double", "else", "extends", "false", "final", "finally", "float",
                "for", "goto", "if", "implements", "import", "instanceof", "int", "interface",
                "long", "native", "new", "null", "package", "private", "protected", "public",
                "return", "short", "static", "super", "string", "switch", "synchronized", "this",
                "threadsafe", "throw[s]", "transient", "true", "try", "void", "while",
                "cast", "operator", "future", "outer", "generic", "rest", "inner", "var"};

                csLexer.evaluar(RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

                load = true;
                evaluaCodigo();
                insercion.Focus();
            }
        }

        public List<string> getIds()
        {
            var lista = new List<string>();

            for (int i = 0; i < datos.Rows.Count - 1; i++)
            {
                if (datos.Rows[i].Cells["Column1"].Value.ToString() == "IDENTIFICADOR")
                {
                    lista.Add(datos.Rows[i].Cells["Column2"].Value.ToString());
                }
            }

            return lista;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nLineas.Text = "";
            calculaLineas();

            if (checkC == true)
            {
                javaX();
                if (load)
                    evaluaCodigo();
                string input = insercion.Text;
                string[] tokens = input.Split(null);

                var grammar = new Gramm(this);

                var id = getIds();
                var str = "";

                for (int i = 0; i < id.Count - 1; i++)
                {
                    str += id[i] + " ";
                }
                str += id[id.Count - 1];

                Console.WriteLine(str);

                grammar.addProductionRule("<id>", str);
                var parser = new Parserr(grammar);
                Parse();


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkC = false;
            insercion.Text = "";
            datos.Rows.Clear();
            nLineas.Text = "1";
        }

        private void label1_Click(object sender, EventArgs e)
        {
            DestroyHandle();
        }

    }
}
