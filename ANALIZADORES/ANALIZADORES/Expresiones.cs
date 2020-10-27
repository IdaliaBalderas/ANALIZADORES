using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ANALIZADORES
{
    class Expresiones
    {
        Regex rex;
        StringBuilder patron;
        bool requiereCompilar;
        List<string> TNames;
        int[] GNumbers;

        public Expresiones()
        {
            requiereCompilar = true;
            TNames = new List<string>();
        }

        public void AddTokenRule(string pattern, string token_name, bool ignore = false)
        {
            if (string.IsNullOrWhiteSpace(token_name))
                throw new ArgumentException(string.Format("{0} no es un nombre válido.", token_name));

            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentException(string.Format("El patrón {0} no es válido.", pattern));

            if (patron == null)
                patron = new StringBuilder(string.Format("(?<{0}>{1})", token_name, pattern));
            else
                patron.Append(string.Format("|(?<{0}>{1})", token_name, pattern));

            if (!ignore)
                TNames.Add(token_name);

            requiereCompilar = true;
        }

        public void Reset()
        {
            rex = null;
            patron = null;
            requiereCompilar = true;
            TNames.Clear();
            GNumbers = null;
        }

        public IEnumerable<Token> GetTokens(string text)
        {
            if (requiereCompilar) throw new Exception("Compilación Requerida, llame al método Compile(options).");

            Match match = rex.Match(text);

            if (!match.Success) yield break;

            int Linea = 0, start = 0, index = 0;

            while (match.Success)
            {
                if (match.Index > index)
                {
                    string token = text.Substring(index, match.Index - index);

                    yield return new Token("SIMBOLO NO RECONOCIDO", token, index, Linea, (index - start) + 1);

                    Linea += contadorL(token, index, ref start);
                }

                for (int i = 0; i < GNumbers.Length; i++)
                {
                    if (match.Groups[GNumbers[i]].Success)
                    {
                        string name = rex.GroupNameFromNumber(GNumbers[i]);

                        yield return new Token(name, match.Value, match.Index, Linea, (match.Index - start) + 1);

                        break;
                    }
                }

                Linea += contadorL(match.Value, match.Index, ref start);
                index = match.Index + match.Length;
                match = match.NextMatch();
            }

            if (text.Length > index)
            {
                yield return new Token("SIMBOLO NO RECONOCIDO", text.Substring(index), index, Linea, (index - start) + 1);
            }
        }

        public void evaluar(RegexOptions options)
        {
            if (patron == null) throw new Exception("Agrege uno o más patrones, llame al método AddTokenRule(pattern, token_name).");

            if (requiereCompilar)
            {
                try
                {
                    rex = new Regex(patron.ToString(), options);

                    GNumbers = new int[TNames.Count];
                    string[] gn = rex.GetGroupNames();

                    for (int i = 0, idx = 0; i < gn.Length; i++)
                    {
                        if (TNames.Contains(gn[i]))
                        {
                            GNumbers[idx++] = rex.GroupNumberFromName(gn[i]);
                        }
                    }

                    requiereCompilar = false;
                }
                catch (Exception ex) { throw ex; }
            }
        }
        private int contadorL(string token, int index, ref int line_start)
        {
            int line = 0;

            for (int i = 0; i < token.Length; i++)
                if (token[i] == '\n')
                {
                    line++;
                    line_start = index + i + 1;
                }

            return line;
        }

    }
}
