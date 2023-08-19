using Microsoft.VisualStudio.TestTools.UnitTesting;
using BehaviorTree;

namespace Tests
{
    [TestClass]
    public class TreeNodeTest
    {
        class TestNode : TreeNode
        {
            protected override TickResult tick(TreeState state, bool i)
            {
                Assert.IsFalse(ticked);
                ticked = true;
                Assert.IsFalse(init);
                init = i;
                return result;
            }
            public bool ticked = false;
            public bool init = false;
            public TickResult result;
        }
        void testControlNode<T>(int inState, bool inInit, TickResult c0Result, TickResult c1Result,
            TickResult outResult, int outState,
            bool c0Ticked, bool c0Init,
            bool c1Ticked, bool c1Init) where T : ControlNode, new()
        {
            var s = new TreeState();
            s.push(1);
            s.setState(0, inState);
            s.pop(0);
            var tn0 = new TestNode() { result = c0Result };
            var tn1 = new TestNode() { result = c1Result };
            var cn = new T() { children = new TreeNode[] { tn0, tn1 } };
            Assert.AreEqual(outResult, cn.tickNode(s, inInit));
            s.reset();
            s.push(1);
            Assert.AreEqual(outState, s.getState(0));
            s.pop(0);
            Assert.AreEqual(c0Ticked, tn0.ticked);
            Assert.AreEqual(c0Init, tn0.init);
            Assert.AreEqual(c1Ticked, tn1.ticked);
            Assert.AreEqual(c1Init, tn1.init);

        }
        [TestMethod]
        public void TestReactiveSequenceNode()
        {
            /////////////////////////////////////////////////////////////////////////////////////////
            // 0, true
            /////////////////////////////////////////////////////////////////////////////////////////
            testControlNode<ReactiveSequenceNode>(0, true, TickResult.Failure, TickResult.Failure,
                TickResult.Failure, 0, true, true, false, false);
            testControlNode<ReactiveSequenceNode>(0, true, TickResult.Failure, TickResult.Success,
                TickResult.Failure, 0, true, true, false, false);
            testControlNode<ReactiveSequenceNode>(0, true, TickResult.Failure, TickResult.Running,
                TickResult.Failure, 0, true, true, false, false);

            testControlNode<ReactiveSequenceNode>(0, true, TickResult.Success, TickResult.Failure,
                TickResult.Failure, 1, true, true, true, true);
            testControlNode<ReactiveSequenceNode>(0, true, TickResult.Success, TickResult.Success,
                TickResult.Success, 1, true, true, true, true);
            testControlNode<ReactiveSequenceNode>(0, true, TickResult.Success, TickResult.Running,
                TickResult.Running, 1, true, true, true, true);

            testControlNode<ReactiveSequenceNode>(0, true, TickResult.Running, TickResult.Failure,
                TickResult.Running, 0, true, true, false, false);
            testControlNode<ReactiveSequenceNode>(0, true, TickResult.Running, TickResult.Success,
                TickResult.Running, 0, true, true, false, false);
            testControlNode<ReactiveSequenceNode>(0, true, TickResult.Running, TickResult.Running,
                TickResult.Running, 0, true, true, false, false);

            /////////////////////////////////////////////////////////////////////////////////////////
            // 0, false
            /////////////////////////////////////////////////////////////////////////////////////////
            testControlNode<ReactiveSequenceNode>(0, false, TickResult.Failure, TickResult.Failure,
                TickResult.Failure, 0, true, false, false, false);
            testControlNode<ReactiveSequenceNode>(0, false, TickResult.Failure, TickResult.Success,
                TickResult.Failure, 0, true, false, false, false);
            testControlNode<ReactiveSequenceNode>(0, false, TickResult.Failure, TickResult.Running,
                TickResult.Failure, 0, true, false, false, false);

            testControlNode<ReactiveSequenceNode>(0, false, TickResult.Success, TickResult.Failure,
                TickResult.Failure, 1, true, false, true, true);
            testControlNode<ReactiveSequenceNode>(0, false, TickResult.Success, TickResult.Success,
                TickResult.Success, 1, true, false, true, true);
            testControlNode<ReactiveSequenceNode>(0, false, TickResult.Success, TickResult.Running,
                TickResult.Running, 1, true, false, true, true);

            testControlNode<ReactiveSequenceNode>(0, false, TickResult.Running, TickResult.Failure,
                TickResult.Running, 0, true, false, false, false);
            testControlNode<ReactiveSequenceNode>(0, false, TickResult.Running, TickResult.Success,
                TickResult.Running, 0, true, false, false, false);
            testControlNode<ReactiveSequenceNode>(0, false, TickResult.Running, TickResult.Running,
                TickResult.Running, 0, true, false, false, false);

            /////////////////////////////////////////////////////////////////////////////////////////
            // 1, true
            /////////////////////////////////////////////////////////////////////////////////////////
            testControlNode<ReactiveSequenceNode>(1, true, TickResult.Failure, TickResult.Failure,
                TickResult.Failure, 0, true, true, false, false);
            testControlNode<ReactiveSequenceNode>(1, true, TickResult.Failure, TickResult.Success,
                TickResult.Failure, 0, true, true, false, false);
            testControlNode<ReactiveSequenceNode>(1, true, TickResult.Failure, TickResult.Running,
                TickResult.Failure, 0, true, true, false, false);

            testControlNode<ReactiveSequenceNode>(1, true, TickResult.Success, TickResult.Failure,
                TickResult.Failure, 1, true, true, true, true);
            testControlNode<ReactiveSequenceNode>(1, true, TickResult.Success, TickResult.Success,
                TickResult.Success, 1, true, true, true, true);
            testControlNode<ReactiveSequenceNode>(1, true, TickResult.Success, TickResult.Running,
                TickResult.Running, 1, true, true, true, true);

            testControlNode<ReactiveSequenceNode>(1, true, TickResult.Running, TickResult.Failure,
                TickResult.Running, 0, true, true, false, false);
            testControlNode<ReactiveSequenceNode>(1, true, TickResult.Running, TickResult.Success,
                TickResult.Running, 0, true, true, false, false);
            testControlNode<ReactiveSequenceNode>(1, true, TickResult.Running, TickResult.Running,
                TickResult.Running, 0, true, true, false, false);

            /////////////////////////////////////////////////////////////////////////////////////////
            // 1, false
            /////////////////////////////////////////////////////////////////////////////////////////
            testControlNode<ReactiveSequenceNode>(1, false, TickResult.Failure, TickResult.Failure,
                TickResult.Failure, 0, true, true, false, false);
            testControlNode<ReactiveSequenceNode>(1, false, TickResult.Failure, TickResult.Success,
                TickResult.Failure, 0, true, true, false, false);
            testControlNode<ReactiveSequenceNode>(1, false, TickResult.Failure, TickResult.Running,
                TickResult.Failure, 0, true, true, false, false);

            testControlNode<ReactiveSequenceNode>(1, false, TickResult.Success, TickResult.Failure,
                TickResult.Failure, 1, true, true, true, false);
            testControlNode<ReactiveSequenceNode>(1, false, TickResult.Success, TickResult.Success,
                TickResult.Success, 1, true, true, true, false);
            testControlNode<ReactiveSequenceNode>(1, false, TickResult.Success, TickResult.Running,
                TickResult.Running, 1, true, true, true, false);

            testControlNode<ReactiveSequenceNode>(1, false, TickResult.Running, TickResult.Failure,
                TickResult.Running, 0, true, true, false, false);
            testControlNode<ReactiveSequenceNode>(1, false, TickResult.Running, TickResult.Success,
                TickResult.Running, 0, true, true, false, false);
            testControlNode<ReactiveSequenceNode>(1, false, TickResult.Running, TickResult.Running,
                TickResult.Running, 0, true, true, false, false);

        }
    }
}
