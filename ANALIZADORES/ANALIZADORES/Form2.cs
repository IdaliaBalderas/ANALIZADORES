using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace ANALIZADORES
{
    public partial class Form2 : Form
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

        public Form2()
        {
            InitializeComponent();
            nLineas.Text = "";
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (checkC == true)
            {
                Cmm();
            }
            else
            {
                if (checkJ == true)
                {
                    Java();
                }
            }
        }

        int numero;
        //para mostrar el numero de lineas del codigo
        public void calculaLineas()
        {
            numLineas = insercion.Text;
            String[] numero = numLineas.Split(new char[] { '\n' });
            int contador = numero.Count();
            for(int i=0; i<contador; i++)
            {
                String cont = i.ToString() + "" + System.Environment.NewLine;
                nLineas.Text += cont;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nLineas.Text = "";
            calculaLineas();
            if (checkC == true)
            {
                Form2_Load(sender, e);

            }
            else
            {
                if (checkJ == true)
                {
                    Form2_Load(sender, e);

                }
                else
                {
                    MessageBox.Show("Seleccione una opcion");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            insercion.Text = "";
            datos.Rows.Clear();
            nLineas.Text = "1";
            checkC = false;
            checkJ = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            insercion.Text = "";
            datos.Rows.Clear();
            nLineas.Text = "1";
            checkC = false;
            checkJ = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
        }

        /*
            csLexer.AddTokenRule("\".*?\"", "CADENA");
            csLexer.AddTokenRule(@"'\\.'|'[^\\]'", "CARACTER");
            csLexer.AddTokenRule(@"//[^\r\n]*", "COMENTARIO1");
            csLexer.AddTokenRule(@"/[*].*?[*]/", "COMENTARIO2");
            csLexer.AddTokenRule(@"\..[^\n]*", "COMENTARIO3");
            csLexer.AddTokenRule(@"[-|+]?\d*\.?\d+", "NUMERO");
            csLexer.AddTokenRule(@"[\(\)\{\}\[\];,]", "DELIMITADOR");
            csLexer.AddTokenRule(@">|<|==|>=|<=|!|!=", "COMPARADOR");
            csLexer.AddTokenRule(@"\+\+|\-\-","ACUMULADOR");
            csLexer.AddTokenRule(@"[=\+\-/*%]|\^|\√", "OPERADOR");
            csLexer.AddTokenRule(@"\\n|\\r","RETORNO");
            csLexer.AddTokenRule(@"\.","REFERENCIADOR");
            csLexer.AddTokenRule(@"'\\.'|'[^\\]'", "CARACTER");
            csLexer.AddTokenRule(@"\s+", "ESPACIO", true);
         */

        public void Java()
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
                evaluar1();
                insercion.Focus();

            }
        }


        public void Cmm()
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

                palabrasReservadas2 = new List<string>() {"asm", "auto", "bool", "break", "case",
                "catch", "char", "class", "const", "const_cast", "continue", "default", "delete",
                "do", "double", "dynamic_cast", "else", "enum", "explicit", "extern", "false",
                "float", "for", "friend", "goto", "if", "inline", "int", "long", "mutable",
                "namespace", "new", "operator", "private", "protected", "public", "register",
                "reinterpret_cast", "return", "short", "signed", "sizeof", "static", "static_cast",
                "struct", "switch", "template", "this", "throw", "true", "try", "typedef", "typeid",
                "typename", "union", "unsigned", "using", "virtual", "void", "volatile", "while"};

                csLexer.evaluar(RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

                load = true;
                evaluar2();
                insercion.Focus();

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkJ = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkC = true;
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            checkC = true;
        }

        private void evaluar1()
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
        private void evaluar2()
        {
            datos.Rows.Clear();

            int n = 0, e = 0;

            foreach (var tk in csLexer.GetTokens(insercion.Text))
            {
                if (tk.Name == "SIMBOLO RECONOCIDO") e++;

                if (tk.Name == "IDENTIFICADOR")
                    if (palabrasReservadas2.Contains(tk.Lexema))
                        tk.Name = "RESERVADO";
                datos.Rows.Add(tk.Name, tk.Lexema, tk.Index, tk.Linea, tk.Columna);
                n++;
            }

        }

    }
}
