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

        }
    }
}