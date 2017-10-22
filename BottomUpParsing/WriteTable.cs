using System.IO;
using Grammars;

namespace BottomUpParsing
{
    public class WriteTable
    {
        public WriteTable(Grammar G, string path = "../../../Core/", string NameClass = "CoolTable")
        {
            var c = new StreamWriter(path + NameClass + ".cs");
            var automaton = new Lr1Automaton(G);

            var table1 = new Lr1Table(automaton);
            c.WriteLine("using System;\n" +
                        "using System.Collections.Generic;\n" +
                        "using Grammars;\nusing BottomUpParsing;\n\n" +
                        "namespace Core {\n\n" +
                        "\tpublic class " + NameClass + "{\n\n" +
                        "\t\tpublic Lr1Table table{get;set;}" + "\n" +
                        "\t\tpublic " + NameClass + "(Grammar G){\n" +
                        "\t\t\ttable= new Lr1Table();\n");
            foreach (var t in table1._table)
                //TODO: Error a la hora de imprimir(arreglar)
                if (t.Value.ActionType != ActionType.None)
                    if (t.Value.ActionType != ActionType.Goto)
                        c.WriteLine("\t\t\ttable.Add(" + "(" + t.Key.Item1 + "," + "G.Terminal(" + "\"" +
                                    t.Key.Item2.Name + "\")" + ")" + ", " + "new Actions(){ActionType = ActionType." +
                                    t.Value.ActionType + "," + "ActionParameter = " + t.Value.ActionParameter + "}" +
                                    ")" + ";\n");
                    //c.WriteLine("\t\t\ttable._table[ (" + t.Key.Item1 + "," + "new Teminal(" + "\"" + t.Key.Item2.Name + "\"" + "," + "G" + ") ) ] = " + "new Actions(){ActionType = ActionType." + t.Value.ActionType + "," + "ActionParameter = " + t.Value.ActionParameter + "}"  + ";\n");
                    else
                        c.WriteLine("\t\t\ttable.Add(" + "(" + t.Key.Item1 + "," + "G.NonTerminal(" + "\"" +
                                    t.Key.Item2.Name + "\")" + ")" + ", " + "new Actions(){ActionType = ActionType." +
                                    t.Value.ActionType + "," + "ActionParameter = " + t.Value.ActionParameter + "}" +
                                    ")" + ";\n");
            c.WriteLine("\t\t}\n\t}\n}");
            c.Flush();
            c.Close();
        }
    }
}