using System.Collections.Generic;
using System.Diagnostics;

namespace BehaviorTree
{
    public enum TickResult { Success, Failure, Running }

    public abstract class TreeNode
    {
        public virtual TickResult tick(TreeState state) { return TickResult.Failure; }
        public virtual int getStateSize() { return 0; }
        public TickResult tickRootNode(TreeState s)
        {
            s.reset(getStateSize());
            var r = tick(s);
            if(r != TickResult.Running)
            {
                s.reset(0);
            }
            return r;
        }
        protected TickResult tickChildNode(TreeState s, TreeNode n)
        {
            s.push(n.getStateSize());
            var r = n.tick(s);
            if(r != TickResult.Running)
            {
                s.pop(getStateSize());
            }
            return r;
        }
    }

    public abstract class ControlNode : TreeNode
    {
        public TreeNode[] children;
    }
    public class SequenceNode : ControlNode
    {
    }

    public abstract class DecoratorNode : TreeNode
    {
        public TreeNode child;
    }

    public abstract class LeftNode : TreeNode
    {
    }
}
