using Grammars;
using AST.Nodes;

namespace Core
{
    public class CoolGrammar
    {
        public readonly Grammar _coolGrammar;

        public CoolGrammar()
        {
            _coolGrammar = new Grammar();

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
            var AssignExp0     = _coolGrammar.NonTerminal("AssignExp0");
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
            var dot = _coolGrammar.Terminal(".");
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

            #region Productions
            Program             %= (Class + semicolon + Program);
            Program             %= (Class + semicolon);
            Class               %= (cclass + TYPE + Inheritance + openBrace + List_Feature + closedBrace);
            Inheritance         %= (inherits + TYPE);
            Inheritance         %= (epsilon);
            List_Feature        %= (Feature + semicolon + List_Feature);
            List_Feature        %= (epsilon);
            Feature             %= (ID + openBracket + List_Formal + closedBracket + colon + TYPE + openBrace + Exp + closedBrace);
            Feature             %= (ID + colon + TYPE + Assign);
            List_Formal         %= (Formal + Tail_Formal);
            List_Formal         %= (epsilon);
            Tail_Formal         %= (comma + Formal + Tail_Formal);
            Tail_Formal         %= (epsilon);
            Formal              %= (ID + colon + TYPE);
            Assign              %= (assign + Exp);
            Assign              %= (epsilon);
            Exp                 %= (ID + assign + Exp);
            Exp                 %= (Exp0);
            Exp0                %= (let + Assignments + cin + Exp0);
            Exp0                %= (Exp1);
            Assignments         %= (ID + colon + TYPE + AssignExp0 + Tail_Assignment);
            Tail_Assignment     %= (comma + Assignments);
            Tail_Assignment     %= (epsilon);
            AssignExp0          %= (assign + Exp0);
            AssignExp0          %= (epsilon);
            Exp1                %= (not + Exp1);
            Exp1                %= Exp2.With(p => p[0]);
            Exp2                %= (Exp3 + less + Exp3).With(p => new Less(p[0], p[2]));
            Exp2                %= (Exp3 + lessEqual + Exp3).With(p => new LessEqual(p[0], p[2]));
            Exp2                %= (Exp3 + equal + Exp3).With(p => new EqualNode(p[0], p[2]));
            Exp2                %= (Exp3).With(p => p[0]);
            Exp3                %= (Exp3 + add + Exp4).With(p => new AddNode(p[0], p[2]));
            Exp3                %= (Exp3 + sub + Exp4).With(p => new SubNode(p[0], p[2]));
            Exp3                %= (Exp4).With(p => p[0]);
            Exp4                %= (Exp4 + mul + Exp5).With(p => new MulNode(p[0], p[2]));
            Exp4                %= (Exp4 + div + Exp5).With(p => new DivNode(p[0], p[2]));
            Exp4                %= (Exp5).With(p => p[0]);
            Exp5                %= (isvoid + Exp5);
            Exp5                %= (Exp6).With(p => p[0]);
            Exp6                %= (neg + Exp6);
            Exp6                %= (Exp7);
            Exp7                %= (Exp7 + Arroba + dot + ID + openBracket + List_Param + closedBracket);
            Exp7                %= Exp8;
            Arroba              %= (arroba + TYPE);
            Arroba              %= (epsilon);
            List_Param          %= (Param + Tail_Param);
            List_Param          %= (epsilon);
            Param               %= (Exp);
            Tail_Param          %= (comma + Param + Tail_Param);
            Tail_Param          %= (epsilon);
            Exp8                %= (ID + openBracket + List_Param + closedBracket);
            Exp8                %= (cif + Exp + then + Exp + celse + Exp + fi);
            Exp8                %= (cwhile + Exp + loop + Exp + pool);
            Exp8                %= (openBrace + Expressions + closedBrace);
            Exp8                %= (ccase + Exp + of + Cases + esac);
            Exp8                %= (cnew + TYPE);
            Exp8                %= (openBracket + Exp + closedBracket);
            Exp8                %= (ID);
            Exp8                %= (integer);
            Exp8                %= (cstring);
            Exp8                %= (ctrue);
            Exp8                %= (cfalse);
            Exp8                %= (cvoid);
            Cases               %= (ID + colon + TYPE + lambda + Exp + semicolon + Cases);
            Cases               %= (ID + colon + TYPE + lambda + Exp + semicolon);
            Expressions         %= (Exp + semicolon + Expressions);
            Expressions         %= (Exp + semicolon);
            #endregion
        }
    }
}