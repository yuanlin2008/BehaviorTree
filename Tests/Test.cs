using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BehaviorTree;

namespace Tests
{
    [TestClass]
    public class Test
    {
        class TestNode : TreeNode
        {
            public override TickResult tick(TreeState state, bool i)
            {
                Assert.IsFalse(ticked);
                ticked = true;
                Assert.IsFalse(init);
                init = i;
                return result_;
            }
            public void reset(TickResult r)
            {
                result_ = r;
                ticked = false;
                init = false;
            }
            public bool ticked = false;
            public bool init = false;
            TickResult result_;
        }
        [TestMethod]
        public void TestReactiveSequenceNode()
        {
            var s = new TreeState();
            var tn0 = new TestNode();
            var tn1 = new TestNode();
            var rsn = new ReactiveSequenceNode() { 
                children = new TreeNode[] { tn0, tn1 } 
            };
            tn0.reset(TickResult.Success);
            tn1.reset(TickResult.Success);
            Assert.AreEqual(TickResult.Success, rsn.tickRootNode(s));
            Assert.IsFalse(s.isRunning);
            Assert.IsTrue(tn0.ticked);
            Assert.IsTrue(tn0.init);
            Assert.IsTrue(tn1.ticked);
            Assert.IsTrue(tn1.init);

            tn0.reset(TickResult.Failure);
            tn1.reset(TickResult.Success);
            Assert.AreEqual(TickResult.Failure, rsn.tickRootNode(s));
            Assert.IsFalse(s.isRunning);

            tn0.reset(TickResult.Success);
            tn1.reset(TickResult.Failure);
            Assert.AreEqual(TickResult.Failure, rsn.tickRootNode(s));
            Assert.IsFalse(s.isRunning);
        }
    }
}
