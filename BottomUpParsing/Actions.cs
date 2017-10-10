namespace BottomUpParsing
{
    public class Actions
    {
        public ActionType ActionType { get; set; }
        public int ActionParameter { get; set; }

        public bool Equals(Actions action)
        {
            return ActionType == action.ActionType && ActionParameter == action.ActionParameter;
        }
    }
}