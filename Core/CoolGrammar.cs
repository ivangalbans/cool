using System.Linq;
using System.Collections.Generic;
using Grammars;
using AST.Nodes;
using AST.Nodes.Abstract;

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
            var AssignExp0      = _coolGrammar.NonTerminal("AssignExp0");
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
            var semicolon       = _coolGrammar.Terminal(";");
            var cclass          = _coolGrammar.Terminal("class");
            var TYPE            = _coolGrammar.Terminal("TYPE");
            var openBrace       = _coolGrammar.Terminal("{");
            var closedBrace     = _coolGrammar.Terminal("}");
            var openBracket     = _coolGrammar.Terminal("(");
            var closedBracket   = _coolGrammar.Terminal(")");
            var inherits        = _coolGrammar.Terminal("inherits");
            var epsilon         = _coolGrammar.Epsilon;
            var ID              = _coolGrammar.Terminal("ID");
            var colon           = _coolGrammar.Terminal(":");
            var comma           = _coolGrammar.Terminal(",");
            var assign          = _coolGrammar.Terminal("<-");
            var let             = _coolGrammar.Terminal("let");
            var cin             = _coolGrammar.Terminal("in");
            var not             = _coolGrammar.Terminal("not");
            var less            = _coolGrammar.Terminal("<");
            var lessEqual       = _coolGrammar.Terminal("<=");
            var greater         = _coolGrammar.Terminal(">");
            var greaterEqual    = _coolGrammar.Terminal(">=");
            var equal           = _coolGrammar.Terminal("=");
            var add             = _coolGrammar.Terminal("+");
            var sub             = _coolGrammar.Terminal("-");
            var mul             = _coolGrammar.Terminal("*");
            var div             = _coolGrammar.Terminal("/");
            var neg             = _coolGrammar.Terminal("~");
            var isvoid          = _coolGrammar.Terminal("isvoid");
            var dot             = _coolGrammar.Terminal(".");
            var arroba          = _coolGrammar.Terminal("@");
            var cif             = _coolGrammar.Terminal("if");
            var fi              = _coolGrammar.Terminal("fi");
            var then            = _coolGrammar.Terminal("then");
            var celse           = _coolGrammar.Terminal("else");
            var cwhile          = _coolGrammar.Terminal("while");
            var loop            = _coolGrammar.Terminal("loop");
            var pool            = _coolGrammar.Terminal("pool");
            var ccase           = _coolGrammar.Terminal("case");
            var esac            = _coolGrammar.Terminal("esac");
            var of              = _coolGrammar.Terminal("of");
            var cnew            = _coolGrammar.Terminal("new");
            var integer         = _coolGrammar.Terminal("integer");
            var cstring         = _coolGrammar.Terminal("string");
            var ctrue           = _coolGrammar.Terminal("true");
            var cfalse          = _coolGrammar.Terminal("false");
            var cvoid           = _coolGrammar.Terminal("void");
            var lambda          = _coolGrammar.Terminal("=>");
            #endregion

            #region Productions
            Program             %= (Class + semicolon + Program).With(p => new ProgramNode(p[0], p[2]));
            Program             %= (Class + semicolon).With(p => new ProgramNode(p[0]));
            Class               %= (cclass + TYPE + Inheritance + openBrace + List_Feature + closedBrace).With(p => new ClassNode(p[1], p[2], p[4]));
            Inheritance         %= (inherits + TYPE).With(p => p[1]);
            Inheritance         %= (epsilon).With(p => new Token(0, "Object", "Object", 0, 0));
            List_Feature        %= (Feature + semicolon + List_Feature).With(p =>
                                    {
                                        var features = new List<FeatureNode>() { p[0] };
                                        features.AddRange(p[2]);
                                        return features;
                                    });
            List_Feature        %= (epsilon).With(p => new List<FeatureNode>());
            Feature             %= (ID + openBracket + List_Formal + closedBracket + colon + TYPE + openBrace + Exp + closedBrace).With(p => new MethodNode(p[0], p[2], p[5], p[7]));
            Feature             %= (ID + colon + TYPE + Assign).With(p => new AttributeNode(p[0], p[2], p[3]));
            List_Formal         %= (Formal + Tail_Formal).With(p =>
                                    {
                                        var formals = new List<(Token, Token)> { p[0].Item1, p[0].Item2 };
                                        formals.AddRange(p[1]);
                                        return formals;
                                    });
            List_Formal         %= (epsilon).With(p => new List<(Token, Token)>()); ;
            Tail_Formal         %= (comma + Formal + Tail_Formal).With(p =>
                                    {
                                        var formals = new List<(Token, Token)> { p[1].Item1, p[1].Item2 };
                                        formals.AddRange(p[2]);
                                        return formals;
                                    });
            Tail_Formal         %= (epsilon).With(p => new List<(Token, Token)>());
            Formal              %= (ID + colon + TYPE).With(p => (p[0], p[2]));
            Assign              %= (assign + Exp).With(p => p[1]);
            Assign              %= (epsilon).With(p => null);
            Exp                 %= (ID + assign + Exp).With(p => new AssignmentNode(p[0], p[2]));
            Exp                 %= (Exp0).With(p => p[0]);
            Exp0                %= (let + Assignments + cin + Exp0).With(p => new LetNode(p[1], p[3]));
            Exp0                %= (Exp1).With(p => p[0]);
            Assignments         %= (ID + colon + TYPE + AssignExp0 + Tail_Assignment).With(p =>
                                    {
                                        var expressionInit = new List<(Token, Token, ExpressionNode)>() { p[0], p[2], p[3]};
                                        expressionInit.AddRange(p[4]);
                                        return expressionInit;
                                    });
            Tail_Assignment     %= (comma + Assignments).With(p => p[1]);
            Tail_Assignment     %= (epsilon).With(p => new List<(Token, Token, ExpressionNode)>());
            AssignExp0          %= (assign + Exp0).With(p => p[1]);
            AssignExp0          %= (epsilon).With(p => null);
            Exp1                %= (not + Exp1).With(p => new NotNode(p[1]));
            Exp1                %= (Exp2).With(p => p[0]);
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
            Exp5                %= (isvoid + Exp5).With(p => new IsVoidNode(p[1]));
            Exp5                %= (Exp6).With(p => p[0]);
            Exp6                %= (neg + Exp6).With(p => new NegNode(p[1]));
            Exp6                %= (Exp7).With(p => p[0]);
            Exp7                %= (Exp7 + Arroba + dot + ID + openBracket + List_Param + closedBracket).With(p => new DispatchExplicitNode(p[0], p[1], p[3], p[5]));
            Exp7                %= (Exp8).With(p => p[0]);
            Arroba              %= (arroba + TYPE).With(p => p[1]);
            Arroba              %= (epsilon).With(p => null);
            List_Param          %= (Param + Tail_Param).With(p =>
                                    {
                                        var paramList = new List<ExpressionNode>() { p[0] };
                                        paramList.AddRange(p[1]);
                                        return paramList;
                                    });
            List_Param          %= (epsilon).With(p => new List<ExpressionNode>());
            Param               %= (Exp).With(p => p[0]);
            Tail_Param          %= (comma + Param + Tail_Param).With(p =>
                                    {
                                        var paramList = new List<ExpressionNode>() { p[1] };
                                        paramList.AddRange(p[2]);
                                        return paramList;
                                    });
            Tail_Param          %= (epsilon).With(p => new List<ExpressionNode>());
            Exp8                %= (ID + openBracket + List_Param + closedBracket).With(p => new DispatchImplicitNode(p[0], p[2]));
            Exp8                %= (cif + Exp + then + Exp + celse + Exp + fi).With(p => new IfNode(p[1], p[3], p[5]));
            Exp8                %= (cwhile + Exp + loop + Exp + pool).With(p => new WhileNode(p[1], p[3]));
            Exp8                %= (openBrace + Expressions + closedBrace).With(p => new BlockNode(p[1]));
            Exp8                %= (ccase + Exp + of + Cases + esac).With(p => new CaseNode(p[1], p[3]));
            Exp8                %= (cnew + TYPE).With(p => new NewNode(p[1]));
            Exp8                %= (openBracket + Exp + closedBracket).With(p => p[1]);
            Exp8                %= (ID).With(p => new IdentifierNode(p[0]));
            Exp8                %= (integer).With(p => new IntNode(p[0]));
            Exp8                %= (cstring).With(p => new StringNode(p[0]));
            Exp8                %= (ctrue).With(p => new BoolNode(p[0]));
            Exp8                %= (cfalse).With(p => new BoolNode(p[0]));
            Exp8                %= (cvoid).With(p => new VoidNode(p[0]));
            Cases               %= (ID + colon + TYPE + lambda + Exp + semicolon + Cases).With(p =>
                                    {
                                        var caseList = new List<(Token, Token, ExpressionNode)>() { (p[0], p[2], p[4]) };
                                        caseList.AddRange(p[6]);
                                        return caseList;
                                    });
            Cases               %= (ID + colon + TYPE + lambda + Exp + semicolon).With(p => new List<(Token, Token, ExpressionNode)>() { (p[0], p[2], p[4]) });
            Expressions         %= (Exp + semicolon + Expressions).With(p =>
                                    {
                                        var expressions = new List<ExpressionNode>() { p[0] };
                                        expressions.AddRange(p[2]);
                                        return expressions;
                                    });
            Expressions         %= (Exp + semicolon).With(p => new List<ExpressionNode>() { p[0] });
            #endregion
        }
    }
}