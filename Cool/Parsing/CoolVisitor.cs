//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\Me\Cybernetic\ComputerScience\4Year\1er semestre\COMP\Compiler\Cool - Ivan\Code\CoolCompiler\Cool\Parsing\Cool.g4 by ANTLR 4.7.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="CoolParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.1")]
[System.CLSCompliant(false)]
public interface ICoolVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="CoolParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProgram([NotNull] CoolParser.ProgramContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>classes</c>
	/// labeled alternative in <see cref="CoolParser.programBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClasses([NotNull] CoolParser.ClassesContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>eof</c>
	/// labeled alternative in <see cref="CoolParser.programBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEof([NotNull] CoolParser.EofContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CoolParser.classDefine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassDefine([NotNull] CoolParser.ClassDefineContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>method</c>
	/// labeled alternative in <see cref="CoolParser.feature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethod([NotNull] CoolParser.MethodContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>property</c>
	/// labeled alternative in <see cref="CoolParser.feature"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty([NotNull] CoolParser.PropertyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CoolParser.formal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFormal([NotNull] CoolParser.FormalContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>parentheses</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParentheses([NotNull] CoolParser.ParenthesesContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>methodCall</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodCall([NotNull] CoolParser.MethodCallContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>comparisson</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparisson([NotNull] CoolParser.ComparissonContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>int</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInt([NotNull] CoolParser.IntContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ownMethodCall</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOwnMethodCall([NotNull] CoolParser.OwnMethodCallContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>isvoid</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIsvoid([NotNull] CoolParser.IsvoidContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>block</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlock([NotNull] CoolParser.BlockContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>while</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhile([NotNull] CoolParser.WhileContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>id</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitId([NotNull] CoolParser.IdContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>boolNot</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBoolNot([NotNull] CoolParser.BoolNotContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>arithmetic</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArithmetic([NotNull] CoolParser.ArithmeticContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>assignment</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignment([NotNull] CoolParser.AssignmentContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>new</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNew([NotNull] CoolParser.NewContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>letIn</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLetIn([NotNull] CoolParser.LetInContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>if</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIf([NotNull] CoolParser.IfContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>string</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitString([NotNull] CoolParser.StringContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>boolean</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBoolean([NotNull] CoolParser.BooleanContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>case</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCase([NotNull] CoolParser.CaseContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>negative</c>
	/// labeled alternative in <see cref="CoolParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNegative([NotNull] CoolParser.NegativeContext context);
}
