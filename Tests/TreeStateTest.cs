using Microsoft.VisualStudio.TestTools.UnitTesting;
using BehaviorTree;

namespace Tests
{
    [TestClass]
    public class TreeStateTest
    {
        class State
        {
            public int[] states;
            public State[] children;
        }
        void tickChild(TreeState s, State p, State c)
        {
            s.push(c.states.Length);
            for (int i = 0; i < c.states.Length; i++)
                s.setState(i, c.states[i]);
            for(int i = 0; i < c.children.Length; i++)
                tickChild(s, c, c.children[i]);
            s.pop(p.states.Length);
            for (int i = 0; i < p.states.Length; i++)
                Assert.AreEqual(p.states[i], s.getState(i));
        }
        void tickRoot(TreeState s, State r)
        {
            s.reset(r.states.Length);
            for (int i = 0; i < r.states.Length; i++)
                s.setState(i, r.states[i]);
            for(int i = 0; i < r.children.Length; i++)
                tickChild(s, r, r.children[i]);
            for (int i = 0; i < r.states.Length; i++)
                Assert.AreEqual(r.states[i], s.getState(i));
        }
        [TestMethod]
        public void TestPushPop()
        {
            var sn =
                new State() { states = new int[] { 2343, 5212, 3432 }, children = new State[] {
                    new State() { states = new int[]{}, children = new State[] {
                        new State() { states = new int[] { 3243, 54352}, children = new State[]{} } } },
                    new State() { states = new int[]{545432}, children = new State[] { } }} 
                };
            var s = new TreeState();
            tickRoot(s, sn);
        }
        [TestMethod]
        public void TestBranch()
        {
            var s = new TreeState();
            s.push(1);
            s.setState(0, 1111);
            s.branch();
            s.discard(true);
            Assert.AreEqual(1111, s.getState(0));
        }
    }
}
