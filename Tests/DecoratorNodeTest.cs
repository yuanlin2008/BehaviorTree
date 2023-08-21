using Microsoft.VisualStudio.TestTools.UnitTesting;
using BehaviorTree;
using TS = BehaviorTree.TickResult;

namespace Tests
{
    [TestClass]
    public class DecoratorNodeTest
    {
        void test<T>(bool inInit, TS cResult, TS outResult, RS cr) where T : DecoratorNode, new()
        {
            var n = new T();
            var s = new TreeState();
            var c = new TestNode() { result = cResult };
            n.child = c;
            Assert.AreEqual(outResult, n.tickNode(s, inInit));
            Assert.AreEqual(cr, c.run);
        }

        void testRepeatNode(bool inInit, int inState, int cycles, TS cResult, TS outResult, int outState, RS cr)
        {
            var n = new RepeatNode() { cycles = cycles };
            var s = new TreeState();
            s.push(1);
            s.setState(0, inState);
            s.pop(0);
            var c = new TestNode() { result = cResult };
            n.child = c;
            Assert.AreEqual(outResult, n.tickNode(s, inInit));
            s.reset();
            s.push(1);
            Assert.AreEqual(outState, s.getState(0));
            s.pop(0);
            Assert.AreEqual(cr, c.run);
        }

        [TestMethod]
        public void TestInvertNode()
        {
            test<InvertNode>(true, TS.Success, TS.Failure, RS.I);
            test<InvertNode>(true, TS.Failure, TS.Success, RS.I);
            test<InvertNode>(true, TS.Running, TS.Running, RS.I);
            test<InvertNode>(false, TS.Success, TS.Failure, RS.R);
            test<InvertNode>(false, TS.Failure, TS.Success, RS.R);
            test<InvertNode>(false, TS.Running, TS.Running, RS.R);
        }
        [TestMethod]
        public void TestForceSuccessNode()
        {
            test<ForceSuccessNode>(true, TS.Success, TS.Success, RS.I);
            test<ForceSuccessNode>(true, TS.Failure, TS.Success, RS.I);
            test<ForceSuccessNode>(true, TS.Running, TS.Running, RS.I);
            test<ForceSuccessNode>(false, TS.Success, TS.Success, RS.R);
            test<ForceSuccessNode>(false, TS.Failure, TS.Success, RS.R);
            test<ForceSuccessNode>(false, TS.Running, TS.Running, RS.R);
        }
        [TestMethod]
        public void TestForceFailureNode()
        {
            test<ForceFailureNode>(true, TS.Success, TS.Failure, RS.I);
            test<ForceFailureNode>(true, TS.Failure, TS.Failure, RS.I);
            test<ForceFailureNode>(true, TS.Running, TS.Running, RS.I);
            test<ForceFailureNode>(false, TS.Success, TS.Failure, RS.R);
            test<ForceFailureNode>(false, TS.Failure, TS.Failure, RS.R);
            test<ForceFailureNode>(false, TS.Running, TS.Running, RS.R);
        }
        [TestMethod]
        public void TestRepeatNode()
        {
            testRepeatNode(true, 0, 1, TS.Success, TS.Success, 1, RS.I);
            testRepeatNode(true, 0, 1, TS.Failure, TS.Failure, 0, RS.I);
            testRepeatNode(true, 0, 1, TS.Running, TS.Running, 0, RS.I);
        }
    }
}
