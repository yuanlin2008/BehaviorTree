using Microsoft.VisualStudio.TestTools.UnitTesting;
using BehaviorTree;
using TS = BehaviorTree.TickResult;

namespace Tests
{
    /// <summary>
    /// Run State.
    /// </summary>
    enum RS 
    { 
        N, // Idle.
        I, // Init.
        R  // Run
    }

    class TestNode : TreeNode
    {
        protected override TS tick(TreeState state, bool init, object context)
        {
            Assert.AreEqual(RS.N, run);
            run = init ? RS.I : RS.R;
            return result;
        }
        public TS result;
        public RS run = RS.N;
    }
}