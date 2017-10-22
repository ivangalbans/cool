using Grammars;

namespace Core
{
    public class CoolGrammar
    {

        public CoolGrammar()
        {
            Grammar _coolGrammar = new Grammar();

            #region Non-Terminals
            var Program         = _coolGrammar.NonTerminal("Program", true);
            var Class           = _coolGrammar.NonTerminal("Class");
            var Inheritance     = _coolGrammar.NonTerminal("Inheritance");
            var List_Feature    = _coolGrammar.NonTerminal("List_Feature");
            var List_Formal     = _coolGrammar.NonTerminal("List_Formal");
            var List_Param      = _coolGrammar.NonTerminal("List_Param");
            var Tail_Param      = _coolGrammar.NonTerminal("Tail_Param");
            var Tail_Formal     = _coolGrammar.NonTerminal("Tail_Formal");
            var Tail_Assignment = _coolGrammar.NonTerminal("Tail_Assignment");
            var Feature         = _coolGrammar.NonTerminal("Feature");
            var Param           = _coolGrammar.NonTerminal("Param");
            var Formal          = _coolGrammar.NonTerminal("Formal");
            var Assign          = _coolGrammar.NonTerminal("Assign");
            var Assignments     = _coolGrammar.NonTerminal("Assignment");
            var Assign_Exp0     = _coolGrammar.NonTerminal("Assign_Exp0");
            var Exp             = _coolGrammar.NonTerminal("Exp");
            var Exp0            = _coolGrammar.NonTerminal("Exp0");
            var Exp1            = _coolGrammar.NonTerminal("Exp1");
            var Exp2            = _coolGrammar.NonTerminal("Exp2");
            var Exp3            = _coolGrammar.NonTerminal("Exp3");
            var Exp4            = _coolGrammar.NonTerminal("Exp4");
            var Exp5            = _coolGrammar.NonTerminal("Exp5");
            var Exp6            = _coolGrammar.NonTerminal("Exp6");
            var Exp7            = _coolGrammar.NonTerminal("Exp7");
            var Exp8            = _coolGrammar.NonTerminal("Exp8");
            var Arroba          = _coolGrammar.NonTerminal("Arroba");
            var Cases           = _coolGrammar.NonTerminal("Cases");
            var Expressions     = _coolGrammar.NonTerminal("Expressions");
            #endregion

            #region Terminals
            var semicolon = _coolGrammar.Terminal(";");
            var cclass = _coolGrammar.Terminal("class");
            var TYPE = _coolGrammar.Terminal("TYPE");
            var openBrace = _coolGrammar.Terminal("{");
            var closedBrace = _coolGrammar.Terminal("}");
            var openBracket = _coolGrammar.Terminal("(");
            var closedBracket = _coolGrammar.Terminal(")");
            var inherits = _coolGrammar.Terminal("inherits");
            var epsilon = _coolGrammar.Epsilon;
            var ID = _coolGrammar.Terminal("ID");
            var colon = _coolGrammar.Terminal(":");
            var comma = _coolGrammar.Terminal(",");
            var assign = _coolGrammar.Terminal("<-");
            var let = _coolGrammar.Terminal("let");
            var cin = _coolGrammar.Terminal("in");
            var not = _coolGrammar.Terminal("not");
            var less = _coolGrammar.Terminal("<");
            var lessEqual = _coolGrammar.Terminal("<=");
            var greater = _coolGrammar.Terminal(">");
            var greaterEqual = _coolGrammar.Terminal(">=");
            var equal = _coolGrammar.Terminal("=");
            var add = _coolGrammar.Terminal("+");
            var sub = _coolGrammar.Terminal("-");
            var mul = _coolGrammar.Terminal("*");
            var div = _coolGrammar.Terminal("/");
            var neg = _coolGrammar.Terminal("~");
            var isvoid = _coolGrammar.Terminal("isvoid");
            var point = _coolGrammar.Terminal(".");
            var arroba = _coolGrammar.Terminal("@");
            var cif = _coolGrammar.Terminal("if");
            var fi = _coolGrammar.Terminal("fi");
            var then = _coolGrammar.Terminal("then");
            var celse = _coolGrammar.Terminal("else");
            var cwhile = _coolGrammar.Terminal("while");
            var loop = _coolGrammar.Terminal("loop");
            var pool = _coolGrammar.Terminal("pool");
            var ccase = _coolGrammar.Terminal("case");
            var esac = _coolGrammar.Terminal("esac");
            var of = _coolGrammar.Terminal("of");
            var cnew = _coolGrammar.Terminal("new");
            var integer = _coolGrammar.Terminal("integer");
            var cstring = _coolGrammar.Terminal("string");
            var ctrue = _coolGrammar.Terminal("true");
            var cfalse = _coolGrammar.Terminal("false");
            var cvoid = _coolGrammar.Terminal("void");
            var lambda = _coolGrammar.Terminal("=>");
            #endregion

        }
    }
}