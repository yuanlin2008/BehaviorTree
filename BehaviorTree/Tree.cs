using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BehaviorTree
{
    [NodeName("SubTree")]
    public class SubTreeNode : TreeNode
    {
        public Tree subtree;
        protected override TickResult tick(TreeState state, bool init, object context) 
        {
            if (subtree == null || subtree.root == null)
                return TickResult.Failure;
            return subtree.root.tickNode(state, init, context);
        }
    }

    public class Tree
    {
        public string name;
        public TreeNode root;
        public TreeNode[] others;

        public TickResult tick(TreeState s, object context)
        {
            if (root == null)
                return TickResult.Failure;
            bool isRunning = s.isRunning;
            s.reset();
            var r = root.tickNode(s, !isRunning, context);
            Debug.Assert(s.isRunning == (r == TickResult.Running));
            return r;
        }
    }
}
