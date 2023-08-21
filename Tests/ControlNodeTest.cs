using Microsoft.VisualStudio.TestTools.UnitTesting;
using BehaviorTree;
using TS = BehaviorTree.TickResult;

namespace Tests
{
    [TestClass]
    public class ControlNodeTest
    {
        void test<T>(int inState, bool inInit, TS c0Result, TS c1Result, TS c2Result,
            TS outResult, int outState, 
            RS c0r, RS c1r, RS c2r) where T : ControlNode, new()
        {
            var s = new TreeState();
            s.push(1);
            s.setState(0, inState);
            s.pop(0);
            var tn0 = new TestNode() { result = c0Result };
            var tn1 = new TestNode() { result = c1Result };
            var tn2 = new TestNode() { result = c2Result };
            var cn = new T() { children = new TreeNode[] { tn0, tn1, tn2 } };
            Assert.AreEqual(outResult, cn.tickNode(s, inInit, null));
            s.reset();
            s.push(1);
            Assert.AreEqual(outState, s.getState(0));
            s.pop(0);
            Assert.AreEqual(c0r, tn0.run);
            Assert.AreEqual(c1r, tn1.run);
            Assert.AreEqual(c2r, tn2.run);

        }
        [TestMethod]
        public void TestSequenceNode()
        {
            test<SequenceNode>(0,true,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,0, RS.I,RS.N,RS.N);
            test<SequenceNode>(0,true,  TS.Running,TS.Failure,TS.Failure, /**/TS.Running,0, RS.I,RS.N,RS.N);
            test<SequenceNode>(0,true,  TS.Success,TS.Failure,TS.Failure, /**/TS.Failure,1, RS.I,RS.I,RS.N);
            test<SequenceNode>(0,true,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.I,RS.I,RS.N);
            test<SequenceNode>(0,true,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.I,RS.I,RS.I);
            test<SequenceNode>(0,true,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.I,RS.I,RS.I);
            test<SequenceNode>(0,true,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.I,RS.I,RS.I);

            test<SequenceNode>(0,false,  TS.Success,TS.Failure,TS.Failure, /**/TS.Failure,1, RS.R,RS.I,RS.N);
            test<SequenceNode>(0,false,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.R,RS.I,RS.N);
            test<SequenceNode>(0,false,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.R,RS.I,RS.I);
            test<SequenceNode>(0,false,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.R,RS.I,RS.I);
            test<SequenceNode>(0,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.R,RS.I,RS.I);

            test<SequenceNode>(1,false,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.N,RS.R,RS.N);
            test<SequenceNode>(1,false,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.N,RS.R,RS.I);
            test<SequenceNode>(1,false,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.N,RS.R,RS.I);
            test<SequenceNode>(1,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.N,RS.R,RS.I);

            test<SequenceNode>(2,false,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.N,RS.N,RS.R);
            test<SequenceNode>(2,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.N,RS.N,RS.R);
        }
        [TestMethod]
        public void TestFallbackNode()
        {
            test<FallbackNode>(0,true,  TS.Success,TS.Success,TS.Success, /**/TS.Success,0, RS.I,RS.N,RS.N);
            test<FallbackNode>(0,true,  TS.Running,TS.Success,TS.Success, /**/TS.Running,0, RS.I,RS.N,RS.N);
            test<FallbackNode>(0,true,  TS.Failure,TS.Success,TS.Success, /**/TS.Success,1, RS.I,RS.I,RS.N);
            test<FallbackNode>(0,true,  TS.Failure,TS.Running,TS.Success, /**/TS.Running,1, RS.I,RS.I,RS.N);
            test<FallbackNode>(0,true,  TS.Failure,TS.Failure,TS.Success, /**/TS.Success,2, RS.I,RS.I,RS.I);
            test<FallbackNode>(0,true,  TS.Failure,TS.Failure,TS.Running, /**/TS.Running,2, RS.I,RS.I,RS.I);
            test<FallbackNode>(0,true,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,2, RS.I,RS.I,RS.I);

            test<FallbackNode>(0,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,0, RS.R,RS.N,RS.N);
            test<FallbackNode>(0,false,  TS.Running,TS.Success,TS.Success, /**/TS.Running,0, RS.R,RS.N,RS.N);
            test<FallbackNode>(0,false,  TS.Failure,TS.Success,TS.Success, /**/TS.Success,1, RS.R,RS.I,RS.N);
            test<FallbackNode>(0,false,  TS.Failure,TS.Running,TS.Success, /**/TS.Running,1, RS.R,RS.I,RS.N);
            test<FallbackNode>(0,false,  TS.Failure,TS.Failure,TS.Success, /**/TS.Success,2, RS.R,RS.I,RS.I);
            test<FallbackNode>(0,false,  TS.Failure,TS.Failure,TS.Running, /**/TS.Running,2, RS.R,RS.I,RS.I);
            test<FallbackNode>(0,false,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,2, RS.R,RS.I,RS.I);

            test<FallbackNode>(1,false,  TS.Failure,TS.Success,TS.Success, /**/TS.Success,1, RS.N,RS.R,RS.N);
            test<FallbackNode>(1,false,  TS.Failure,TS.Running,TS.Success, /**/TS.Running,1, RS.N,RS.R,RS.N);
            test<FallbackNode>(1,false,  TS.Failure,TS.Failure,TS.Success, /**/TS.Success,2, RS.N,RS.R,RS.I);
            test<FallbackNode>(1,false,  TS.Failure,TS.Failure,TS.Running, /**/TS.Running,2, RS.N,RS.R,RS.I);
            test<FallbackNode>(1,false,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,2, RS.N,RS.R,RS.I);

            test<FallbackNode>(2,false,  TS.Failure,TS.Failure,TS.Success, /**/TS.Success,2, RS.N,RS.N,RS.R);
            test<FallbackNode>(2,false,  TS.Failure,TS.Failure,TS.Running, /**/TS.Running,2, RS.N,RS.N,RS.R);
            test<FallbackNode>(2,false,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,2, RS.N,RS.N,RS.R);
        }
        [TestMethod]
        public void TestReactiveSequenceNode()
        {
            test<ReactiveSequenceNode>(0,true,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,0, RS.I,RS.N,RS.N);
            test<ReactiveSequenceNode>(0,true,  TS.Running,TS.Failure,TS.Failure, /**/TS.Running,0, RS.I,RS.N,RS.N);
            test<ReactiveSequenceNode>(0,true,  TS.Success,TS.Failure,TS.Failure, /**/TS.Failure,1, RS.I,RS.I,RS.N);
            test<ReactiveSequenceNode>(0,true,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.I,RS.I,RS.N);
            test<ReactiveSequenceNode>(0,true,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.I,RS.I,RS.I);
            test<ReactiveSequenceNode>(0,true,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.I,RS.I,RS.I);
            test<ReactiveSequenceNode>(0,true,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.I,RS.I,RS.I);

            test<ReactiveSequenceNode>(0,false,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,0, RS.R,RS.N,RS.N);
            test<ReactiveSequenceNode>(0,false,  TS.Running,TS.Failure,TS.Failure, /**/TS.Running,0, RS.R,RS.N,RS.N);
            test<ReactiveSequenceNode>(0,false,  TS.Success,TS.Failure,TS.Failure, /**/TS.Failure,1, RS.R,RS.I,RS.N);
            test<ReactiveSequenceNode>(0,false,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.R,RS.I,RS.N);
            test<ReactiveSequenceNode>(0,false,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.R,RS.I,RS.I);
            test<ReactiveSequenceNode>(0,false,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.R,RS.I,RS.I);
            test<ReactiveSequenceNode>(0,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.R,RS.I,RS.I);

            test<ReactiveSequenceNode>(1,false,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,0, RS.I,RS.N,RS.N);
            test<ReactiveSequenceNode>(1,false,  TS.Running,TS.Failure,TS.Failure, /**/TS.Running,0, RS.I,RS.N,RS.N);
            test<ReactiveSequenceNode>(1,false,  TS.Success,TS.Failure,TS.Failure, /**/TS.Failure,1, RS.I,RS.R,RS.N);
            test<ReactiveSequenceNode>(1,false,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.I,RS.R,RS.N);
            test<ReactiveSequenceNode>(1,false,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.I,RS.R,RS.I);
            test<ReactiveSequenceNode>(1,false,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.I,RS.R,RS.I);
            test<ReactiveSequenceNode>(1,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.I,RS.R,RS.I);

            test<ReactiveSequenceNode>(2,false,  TS.Failure,TS.Failure,TS.Failure, /**/TS.Failure,0, RS.I,RS.N,RS.N);
            test<ReactiveSequenceNode>(2,false,  TS.Running,TS.Failure,TS.Failure, /**/TS.Running,0, RS.I,RS.N,RS.N);
            test<ReactiveSequenceNode>(2,false,  TS.Success,TS.Failure,TS.Failure, /**/TS.Failure,1, RS.I,RS.I,RS.N);
            test<ReactiveSequenceNode>(2,false,  TS.Success,TS.Running,TS.Failure, /**/TS.Running,1, RS.I,RS.I,RS.N);
            test<ReactiveSequenceNode>(2,false,  TS.Success,TS.Success,TS.Failure, /**/TS.Failure,2, RS.I,RS.I,RS.R);
            test<ReactiveSequenceNode>(2,false,  TS.Success,TS.Success,TS.Running, /**/TS.Running,2, RS.I,RS.I,RS.R);
            test<ReactiveSequenceNode>(2,false,  TS.Success,TS.Success,TS.Success, /**/TS.Success,2, RS.I,RS.I,RS.R);
        }
    }
}
