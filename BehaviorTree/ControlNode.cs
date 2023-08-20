namespace BehaviorTree
{
    public abstract class ControlNode : TreeNode
    {
        public TreeNode[] children;
    }

    public class SequenceNode : ControlNode
    {
        protected override int getStateSize() { return 1; }

        protected override TickResult tick(TreeState state, bool init)
        {
            if (init)
                state.setState(0, 0);
            for(int i = state.getState(0); i < children.Length; i++)
            {
                state.setState(0, i);
                var r = children[i].tickNode(state, init);
                if(r == TickResult.Success)
                    init = true;
                else
                    return r;
            }
            return TickResult.Success; 
        }
    }

    public class FallbackNode : ControlNode
    {
        protected override int getStateSize() { return 1; }

        protected override TickResult tick(TreeState state, bool init)
        {
            if (init)
                state.setState(0, 0);
            for(int i = state.getState(0); i < children.Length; i++)
            {
                state.setState(0, i);
                var r = children[i].tickNode(state, init);
                if(r == TickResult.Failure)
                    init = true;
                else
                    return r;
            }
            return TickResult.Failure; 
        }
    }

    public class ReactiveSequenceNode : ControlNode
    {
        protected override int getStateSize() { return 1; }

        protected override TickResult tick(TreeState state, bool init)
        {
            if (init)
                state.setState(0, 0);
            var curId = state.getState(0);
            state.branch();
            for(int i = 0; i < curId; i++)
            {
                state.setState(0, i);
                var r = children[i].tickNode(state, true);
                if(r != TickResult.Success)
                {
                    state.discard(false);
                    return r;
                }
            }
            state.discard(true);
            for(int i = curId; i < children.Length; i++)
            {
                state.setState(0, i);
                var r = children[i].tickNode(state, init);
                if(r == TickResult.Success)
                    init = true;
                else
                    return r;
            }
            return TickResult.Success; 
        }
    }

    public class ReactiveFallbackNode : ControlNode
    {
        protected override int getStateSize() { return 1; }

        protected override TickResult tick(TreeState state, bool init)
        {
            if (init)
                state.setState(0, 0);
            var curId = state.getState(0);
            state.branch();
            for(int i = 0; i < curId; i++)
            {
                state.setState(0, i);
                var r = children[i].tickNode(state, true);
                if(r != TickResult.Failure)
                {
                    state.discard(false);
                    return r;
                }
            }
            state.discard(true);
            for(int i = curId; i < children.Length; i++)
            {
                state.setState(0, i);
                var r = children[i].tickNode(state, init);
                if(r == TickResult.Failure)
                    init = true;
                else
                    return r;
            }
            return TickResult.Failure; 
        }
    }

}
