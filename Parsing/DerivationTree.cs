using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grammars;

namespace Parsing
{
    public class DerivationTree
    {
        private DerivationTree[] _children;

        public DerivationTree(Symbol root, ProductionAttr production, params DerivationTree[] children)
        {
            Production = production;
            Root = root;
            _children = children.Length > 0 ? children : new DerivationTree[0];
        }

        public Symbol Root { get; }

        public ProductionAttr Production { get; set; }
        public Token Terminal { get; set; }
        private bool _epsLeaf=true;
        public bool IsLeaf => _children.Length == 0 && _epsLeaf;

        public bool IsTerminal => Root is Terminal;


        public dynamic Evaluate()
        {
            if (IsTerminal) return Terminal.Text;
            var temp = _children.Select(child => child.Evaluate()).ToList();
            temp.Reverse();
            return Production.Evaluate(temp.ToArray());
        }

        public static DerivationTree FromRightMost(IEnumerable<(ProductionAttr, List<Token>)> productions)
        {
            return FromRightMost(productions.Reverse().ToArray());
        }

        private static DerivationTree FromRightMost(IList<(ProductionAttr, List<Token>)> productions)
        {
            var root = new DerivationTree(productions[0].Item1.Left, productions[0].Item1);

            foreach (var production in productions)
            {
                production.Item2.Reverse();
                

                var sentence = production.Item1.Right.Reverse().ToList();

                var childToOpen = root.MostToOpen(production.Item1.Left.Name);


                childToOpen.Production = production.Item1;
                if (production.Item1.IsEpsilon)
                {
                    childToOpen._epsLeaf = false;
                    continue;
                }
                childToOpen._children = new DerivationTree[sentence.Count];

                for (int i = 0, j = 0; i < sentence.Count; i++)
                {
                    childToOpen._children[i] = new DerivationTree(sentence[i], null);
                    if (childToOpen._children[i].IsTerminal)
                        childToOpen._children[i].Terminal = production.Item2[j++];
                }
            }

            return root;
        }

        public static DerivationTree FromLeftMost(IEnumerable<Production> productions)
        {
            return FromLeftMost(productions.ToArray());
        }

        private static DerivationTree FromLeftMost(Production[] productions)
        {
            var root = new DerivationTree(productions[0].Left, productions[0] as ProductionAttr);

            foreach (var production in productions)
            {
                var sentence = production.Right;

                var childToOpen = root.LeftMostToOpen();


                childToOpen._children = new DerivationTree[sentence.Length];

                for (var i = 0; i < sentence.Length; i++)
                    childToOpen._children[i] = new DerivationTree(sentence[i], production as ProductionAttr);
            }

            return root;
        }

        private DerivationTree MostToOpen(string name)
        {
            if (IsTerminal)
                return null;

            if (IsLeaf && !IsTerminal && Root.Name == name)
                return this;

            foreach (var derivationTree in _children)
            {
                var leftMost = derivationTree.MostToOpen(name);

                if (leftMost != null)
                    return leftMost;
            }

            return null;
        }

        private DerivationTree LeftMostToOpen()
        {
            if (IsTerminal)
                return null;

            if (IsLeaf && !IsTerminal)
                return this;

            foreach (var derivationTree in _children)
            {
                var leftMost = derivationTree.LeftMostToOpen();

                if (leftMost != null)
                    return leftMost;
            }

            return null;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            ToString(builder, 0, new Stack<int>(), false);
            return builder.ToString();
        }

        private void ToString(StringBuilder builder, int level, Stack<int> stops, bool last)
        {
            for (var i = 0; i < level; i++)
                if (i == level - 1)
                    builder.Append(last ? "└" : "├");
                else if (stops.Contains(i))
                    builder.Append("│");
                else
                    builder.Append(" ");

            builder.AppendLine(Root + ": " + Terminal?.Text);

            if (!IsLeaf)
                stops.Push(level);

            for (var i = 0; i < _children.Length; i++)
            {
                if (i == _children.Length - 1)
                    stops.Pop();

                _children[i].ToString(builder, level + 1, stops, i == _children.Length - 1);
            }
        }
    }
}