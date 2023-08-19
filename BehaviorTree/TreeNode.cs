using System.Collections.Generic;
using System.Diagnostics;

namespace BehaviorTree
{
    public enum TickResult { Success, Failure, Running }

    public abstract class TreeNode
    {
        protected virtual TickResult tick(TreeState state, bool init) { return TickResult.Failure; }
        protected virtual int getStateSize() { return 0; }
        public TickResult tickNode(TreeState state, bool init)
        {
            var lastSize = state.push(getStateSize());
            var result = tick(state, init);
            if(result != TickResult.Running)
                state.pop(lastSize);
            return result;
        }

        public TickResult tickRoot(TreeState s)
        {
            bool isRunning = s.isRunning;
            s.reset();
            var r = tickNode(s, !isRunning);
            Debug.Assert(s.isRunning == (r == TickResult.Running));
            return r;
        }
    }

    public abstract class ControlNode : TreeNode
    {
        public TreeNode[] children;
    }

    public class SequenceNode : ControlNode
    {
        protected override int getStateSize()
        {
            return 1;
        }
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

    public sealed class ReactiveSequenceNode : ControlNode
    {
        protected override int getStateSize()
        {
            return 1;
        }
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

    public abstract class DecoratorNode : TreeNode
    {
        public TreeNode child;
    }

    public abstract class LeftNode : TreeNode
    {
    }
}
