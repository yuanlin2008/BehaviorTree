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
        void testState(TreeState state, State s)
        {
            var size = state.push(s.states.Length);
            for (int i = 0; i < s.states.Length; i++)
                state.setState(i, s.states[i]);
            for (int i = 0; i < s.children.Length; i++)
                testState(state, s.children[i]);
            for (int i = 0; i < s.states.Length; i++)
                Assert.AreEqual(s.states[i], state.getState(i));
            state.pop(size);
        }
        [TestMethod]
        public void TestPushPop()
        {
            var sn =
                new State() { states = new int[] { 2343, 5212, 3432 }, children = new State[] {
                    new State() { states = new int[]{}, children = new State[] {
                        new State() { states = new int[] { 3243, 54352}, children = new State[]{} },
                        new State() { states = new int[] {}, children = new State[]{} },
                        new State() { states = new int[] { 12345, 54321}, children = new State[]{} } } },
                    new State() { states = new int[]{545432}, children = new State[] { } } } 
                };
            var s = new TreeState();
            testState(s, sn);
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
