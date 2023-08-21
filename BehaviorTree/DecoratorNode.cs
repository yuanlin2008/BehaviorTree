namespace BehaviorTree
{
    public abstract class DecoratorNode : TreeNode
    {
        public TreeNode child;
    }

    [NodeName("Invert")]
    public class InvertNode : DecoratorNode
    {
        protected override TickResult tick(TreeState state, bool init)
        {
            if (child == null)
                return TickResult.Failure;
            var r = child.tickNode(state, init);
            return 
                (r == TickResult.Failure) ? 
                    TickResult.Success : 
                    (r == TickResult.Success) ? TickResult.Failure : r;
        }
    }

    [NodeName("ForceSuccess")]
    public class ForceSuccessNode : DecoratorNode
    {
        protected override TickResult tick(TreeState state, bool init)
        {
            if (child == null)
                return TickResult.Success;
            var r = child.tickNode(state, init);
            return (r == TickResult.Running) ? r : TickResult.Success;
        }
    }

    [NodeName("ForceFailure")]
    public class ForceFailureNode : DecoratorNode
    {
        protected override TickResult tick(TreeState state, bool init)
        {
            if (child == null)
                return TickResult.Success;
            var r = child.tickNode(state, init);
            return (r == TickResult.Running) ? r : TickResult.Failure;
        }
    }

    [NodeName("Repeat")]
    public class RepeatNode : DecoratorNode
    {
        public int cycles = 0;
        protected override int getStateSize() { return 1; }
        protected override TickResult tick(TreeState state, bool init)
        {
            if (child == null)
                return TickResult.Failure;

            if (init)
                state.setState(0, 0);
            var curCycle = state.getState(0);
            while(curCycle < cycles || cycles <= 0)
            {
                var r = child.tickNode(state, init);
                if (r != TickResult.Success)
                    return r;
                init = true;
                curCycle++;
                state.setState(0, curCycle);
            }
            return TickResult.Success;
        }
    }

    [NodeName("Retry")]
    public class RetryNode : DecoratorNode
    {
        public int cycles = 0;
        protected override int getStateSize() { return 1; }
        protected override TickResult tick(TreeState state, bool init)
        {
            if (child == null)
                return TickResult.Failure;

            if (init)
                state.setState(0, 0);
            var curCycle = state.getState(0);
            while(curCycle < cycles || cycles <= 0)
            {
                var r = child.tickNode(state, init);
                if (r != TickResult.Failure)
                    return r;
                init = true;
                curCycle++;
                state.setState(0, curCycle);
            }
            return TickResult.Failure;
        }
    }
}