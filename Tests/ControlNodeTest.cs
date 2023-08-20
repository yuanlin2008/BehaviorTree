using Microsoft.VisualStudio.TestTools.UnitTesting;
using BehaviorTree;
using TS = BehaviorTree.TickResult;

namespace Tests
{
    [TestClass]
    public class ControlNodeTest
    {
        enum RS { N, I, R}
        class TestNode : TreeNode
        {
            protected override TS tick(TreeState state, bool i)
            {
                Assert.AreEqual(RS.N, s);
                s = i ? RS.I : RS.R;
                return r;
            }
            public TS r;
            public RS s;
        }

        void testControlNode<T>(int inState, bool inInit, TS c0Result, TS c1Result, TS c2Result,
            TS outResult, int outState, 
            RS c0, RS c1, RS c2) where T : ControlNode, new()
        {
            var s = new TreeState();
            s.push(1);
            s.setState(0, inState);
            s.pop(0);
            var tn0 = new TestNode() { r = c0Result };
            var tn1 = new TestNode() { r = c1Result };
            var tn2 = new TestNode() { r = c2Result };
            var cn = new T() { children = new TreeNode[] { tn0, tn1, tn2 } };
            Assert.AreEqual(outResult, cn.tickNode(s, inInit));
            s.reset();
            s.push(1);
            Assert.AreEqual(outState, s.getState(0));
            s.pop(0);
            Assert.AreEqual(c0, tn0.s);
            Assert.AreEqual(c1, tn1.s);
            Assert.AreEqual(c2, tn2.s);

        }
        [TestMethod]
        public void TestSequenceNode()
        {
            testControlNode<SequenceNode>(0,true,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,0, RS.I,RS.N,RS.N);
            testControlNode<SequenceNode>(0,true,  TS.Running,TS.Failure,TS.Failure, /**/TS.Running,0, RS.I,RS.N,RS.N);
            testControlNode<SequenceNode>(0,true,  TS.Success,TS.Failure,TS.Failure, /**/TS.Failure,1, RS.I,RS.I,RS.N);
            testControlNode<SequenceNode>(0,true,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.I,RS.I,RS.N);
            testControlNode<SequenceNode>(0,true,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.I,RS.I,RS.I);
            testControlNode<SequenceNode>(0,true,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.I,RS.I,RS.I);
            testControlNode<SequenceNode>(0,true,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.I,RS.I,RS.I);

            testControlNode<SequenceNode>(0,false,  TS.Success,TS.Failure,TS.Failure, /**/TS.Failure,1, RS.R,RS.I,RS.N);
            testControlNode<SequenceNode>(0,false,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.R,RS.I,RS.N);
            testControlNode<SequenceNode>(0,false,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.R,RS.I,RS.I);
            testControlNode<SequenceNode>(0,false,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.R,RS.I,RS.I);
            testControlNode<SequenceNode>(0,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.R,RS.I,RS.I);

            testControlNode<SequenceNode>(1,false,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.N,RS.R,RS.N);
            testControlNode<SequenceNode>(1,false,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.N,RS.R,RS.I);
            testControlNode<SequenceNode>(1,false,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.N,RS.R,RS.I);
            testControlNode<SequenceNode>(1,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.N,RS.R,RS.I);

            testControlNode<SequenceNode>(2,false,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.N,RS.N,RS.R);
            testControlNode<SequenceNode>(2,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.N,RS.N,RS.R);
        }
        [TestMethod]
        public void TestFallbackNode()
        {
            testControlNode<FallbackNode>(0,true,  TS.Success,TS.Success,TS.Success, /**/TS.Success,0, RS.I,RS.N,RS.N);
            testControlNode<FallbackNode>(0,true,  TS.Running,TS.Success,TS.Success, /**/TS.Running,0, RS.I,RS.N,RS.N);
            testControlNode<FallbackNode>(0,true,  TS.Failure,TS.Success,TS.Success, /**/TS.Success,1, RS.I,RS.I,RS.N);
            testControlNode<FallbackNode>(0,true,  TS.Failure,TS.Running,TS.Success, /**/TS.Running,1, RS.I,RS.I,RS.N);
            testControlNode<FallbackNode>(0,true,  TS.Failure,TS.Failure,TS.Success, /**/TS.Success,2, RS.I,RS.I,RS.I);
            testControlNode<FallbackNode>(0,true,  TS.Failure,TS.Failure,TS.Running, /**/TS.Running,2, RS.I,RS.I,RS.I);
            testControlNode<FallbackNode>(0,true,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,2, RS.I,RS.I,RS.I);

            testControlNode<FallbackNode>(0,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,0, RS.R,RS.N,RS.N);
            testControlNode<FallbackNode>(0,false,  TS.Running,TS.Success,TS.Success, /**/TS.Running,0, RS.R,RS.N,RS.N);
            testControlNode<FallbackNode>(0,false,  TS.Failure,TS.Success,TS.Success, /**/TS.Success,1, RS.R,RS.I,RS.N);
            testControlNode<FallbackNode>(0,false,  TS.Failure,TS.Running,TS.Success, /**/TS.Running,1, RS.R,RS.I,RS.N);
            testControlNode<FallbackNode>(0,false,  TS.Failure,TS.Failure,TS.Success, /**/TS.Success,2, RS.R,RS.I,RS.I);
            testControlNode<FallbackNode>(0,false,  TS.Failure,TS.Failure,TS.Running, /**/TS.Running,2, RS.R,RS.I,RS.I);
            testControlNode<FallbackNode>(0,false,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,2, RS.R,RS.I,RS.I);

            testControlNode<FallbackNode>(1,false,  TS.Failure,TS.Success,TS.Success, /**/TS.Success,1, RS.N,RS.R,RS.N);
            testControlNode<FallbackNode>(1,false,  TS.Failure,TS.Running,TS.Success, /**/TS.Running,1, RS.N,RS.R,RS.N);
            testControlNode<FallbackNode>(1,false,  TS.Failure,TS.Failure,TS.Success, /**/TS.Success,2, RS.N,RS.R,RS.I);
            testControlNode<FallbackNode>(1,false,  TS.Failure,TS.Failure,TS.Running, /**/TS.Running,2, RS.N,RS.R,RS.I);
            testControlNode<FallbackNode>(1,false,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,2, RS.N,RS.R,RS.I);

            testControlNode<FallbackNode>(2,false,  TS.Failure,TS.Failure,TS.Success, /**/TS.Success,2, RS.N,RS.N,RS.R);
            testControlNode<FallbackNode>(2,false,  TS.Failure,TS.Failure,TS.Running, /**/TS.Running,2, RS.N,RS.N,RS.R);
            testControlNode<FallbackNode>(2,false,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,2, RS.N,RS.N,RS.R);
        }
        [TestMethod]
        public void TestReactiveSequenceNode()
        {
            testControlNode<ReactiveSequenceNode>(0,true,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,0, RS.I,RS.N,RS.N);
            testControlNode<ReactiveSequenceNode>(0,true,  TS.Running,TS.Failure,TS.Failure, /**/TS.Running,0, RS.I,RS.N,RS.N);
            testControlNode<ReactiveSequenceNode>(0,true,  TS.Success,TS.Failure,TS.Failure, /**/TS.Failure,1, RS.I,RS.I,RS.N);
            testControlNode<ReactiveSequenceNode>(0,true,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.I,RS.I,RS.N);
            testControlNode<ReactiveSequenceNode>(0,true,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.I,RS.I,RS.I);
            testControlNode<ReactiveSequenceNode>(0,true,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.I,RS.I,RS.I);
            testControlNode<ReactiveSequenceNode>(0,true,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.I,RS.I,RS.I);

            testControlNode<ReactiveSequenceNode>(0,false,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,0, RS.R,RS.N,RS.N);
            testControlNode<ReactiveSequenceNode>(0,false,  TS.Running,TS.Failure,TS.Failure, /**/TS.Running,0, RS.R,RS.N,RS.N);
            testControlNode<ReactiveSequenceNode>(0,false,  TS.Success,TS.Failure,TS.Failure, /**/TS.Failure,1, RS.R,RS.I,RS.N);
            testControlNode<ReactiveSequenceNode>(0,false,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.R,RS.I,RS.N);
            testControlNode<ReactiveSequenceNode>(0,false,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.R,RS.I,RS.I);
            testControlNode<ReactiveSequenceNode>(0,false,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.R,RS.I,RS.I);
            testControlNode<ReactiveSequenceNode>(0,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.R,RS.I,RS.I);

            testControlNode<ReactiveSequenceNode>(1,false,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,0, RS.I,RS.N,RS.N);
            testControlNode<ReactiveSequenceNode>(1,false,  TS.Running,TS.Failure,TS.Failure, /**/TS.Running,0, RS.I,RS.N,RS.N);
            testControlNode<ReactiveSequenceNode>(1,false,  TS.Success,TS.Failure,TS.Failure, /**/TS.Failure,1, RS.I,RS.R,RS.N);
            testControlNode<ReactiveSequenceNode>(1,false,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.I,RS.R,RS.N);
            testControlNode<ReactiveSequenceNode>(1,false,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.I,RS.R,RS.I);
            testControlNode<ReactiveSequenceNode>(1,false,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.I,RS.R,RS.I);
            testControlNode<ReactiveSequenceNode>(1,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.I,RS.R,RS.I);

            testControlNode<ReactiveSequenceNode>(2,false,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,0, RS.I,RS.N,RS.N);
            testControlNode<ReactiveSequenceNode>(2,false,  TS.Running,TS.Failure,TS.Failure, /**/TS.Running,0, RS.I,RS.N,RS.N);
            testControlNode<ReactiveSequenceNode>(2,false,  TS.Success,TS.Failure,TS.Failure, /**/TS.Failure,1, RS.I,RS.I,RS.N);
            testControlNode<ReactiveSequenceNode>(2,false,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.I,RS.I,RS.N);
            testControlNode<ReactiveSequenceNode>(2,false,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.I,RS.I,RS.R);
            testControlNode<ReactiveSequenceNode>(2,false,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.I,RS.I,RS.R);
            testControlNode<ReactiveSequenceNode>(2,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.I,RS.I,RS.R);
        }
    }
}
