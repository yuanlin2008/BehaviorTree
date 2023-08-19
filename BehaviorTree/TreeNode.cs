using System.Collections.Generic;
using System.Diagnostics;

namespace BehaviorTree
{
    public enum TickResult { Success, Failure, Running }

    public abstract class TreeNode
    {
        public virtual TickResult tick(TreeState state, bool init) { return TickResult.Failure; }
        public virtual int getStateSize() { return 0; }
        public TickResult tickRootNode(TreeState s)
        {
            bool isRunning = s.isRunning;
            s.reset(getStateSize());
            var r = tick(s, !isRunning);
            if(r == TickResult.Running)
                s.isRunning = true;
            else
                s.reset(0);
            return r;
        }
        protected TickResult tickChildNode(TreeState s, TreeNode n, bool init)
        {
            s.push(n.getStateSize());
            var r = n.tick(s, init);
            if(r != TickResult.Running)
                s.pop(getStateSize());
            return r;
        }
    }

    public abstract class ControlNode : TreeNode
    {
        public TreeNode[] children;
    }
    public sealed class SequenceNode : ControlNode
    {
        public override int getStateSize()
        {
            return 1;
        }
        public override TickResult tick(TreeState state, bool init)
        {
            if (init)
                state.setState(0, 0);
            for(int i = state.getState(0); i < children.Length; i++)
            {
                state.setState(0, i);
                var r = tickChildNode(state, children[i], init);
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
        public override int getStateSize()
        {
            return 1;
        }
        public override TickResult tick(TreeState state, bool init)
        {
            if (init)
                state.setState(0, 0);
            var curId = state.getState(0);
            state.branch();
            for(int i = 0; i < curId; i++)
            {
                var r = tickChildNode(state, children[i], true);
                if(r == TickResult.Failure)
                    return r;
                else if(r == TickResult.Running)
                {
                    state.discard(false);
                    state.setState(0, i);
                    return r;
                }
            }
            state.discard(true);
            for(int i = curId; i < children.Length; i++)
            {
                state.setState(0, i);
                var r = tickChildNode(state, children[i], init);
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
