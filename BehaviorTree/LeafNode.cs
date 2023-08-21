namespace BehaviorTree
{
    public abstract class LeafNode : TreeNode
    {
    }

    public abstract class ConditionNode : LeafNode
    {
        protected virtual bool tickCondition(TreeState state, bool init, object context) { return false; }
        protected sealed override TickResult tick(TreeState state, bool init, object context) 
        {
            return tickCondition(state, init, context) ? TickResult.Success : TickResult.Failure;
        }
    }

    public abstract class ActionNode : LeafNode
    {
    }
}