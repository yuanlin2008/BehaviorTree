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
        protected override TickResult tick(TreeState state, bool init) 
        {
            if (subtree == null || subtree.root == null)
                return TickResult.Failure;
            return subtree.root.tickNode(state, init);
        }
    }

    public class Tree
    {
        public string name;
        public TreeNode root;
        public TreeNode[] others;

        public TickResult tick(TreeState s)
        {
            if (root == null)
                return TickResult.Failure;
            bool isRunning = s.isRunning;
            s.reset();
            var r = root.tickNode(s, !isRunning);
            Debug.Assert(s.isRunning == (r == TickResult.Running));
            return r;
        }
    }
}
