using System.Collections.Generic;
using System.Diagnostics;

namespace BehaviorTree
{
    public class TreeState
    {
        public bool isRunning { get; set; }

        public void reset(int size)
        {
            stacks_[0].top = 0;
            stacks_[0].size = size;
            branch_ = 0;
            expendStack();
        }
        public bool isBranch { get { return branch_ == 1; } }
        public void branch()
        {
            Debug.Assert(branch_ == 0);
            var mStk = stacks_[0];
            var bStk = stacks_[1];
            bStk.top = 0;
            bStk.size = mStk.size;
            branch_ = 1;
            expendStack();
            // Main => Branch.
            for(int i = 0; i < bStk.size; i++)
                bStk.states[i] = mStk.states[mStk.top + i];
        }
        public void discard(bool b)
        {
            Debug.Assert(branch_ == 1);
            branch_ = 0;
            var mStk = stacks_[0];
            var bStk = stacks_[1];
            mStk.top += bStk.top;
            mStk.size = bStk.size;
            expendStack();
            // Branch => Main
            for(int i = 0; i < bStk.top + bStk.size; i++)
                mStk.states[mStk.top + i] = bStk.states[i];
        }
        public void push(int size)
        {
            var stack = getStack();
            stack.top += stack.size;
            stack.size = size;
            expendStack();
        }
        public void pop(int size)
        {
            var stack = getStack();
            stack.top -= size;
            stack.size = size;
        }
        public int getState(int id) 
        {
            var stack = getStack();
            Debug.Assert(id < stack.size);
            return stack.states[stack.top + id];
        }
        public void setState(int id, int value)
        {
            var stack = getStack();
            Debug.Assert(id < stack.size);
            stack.states[stack.top + id] = value;
        }

        void expendStack()
        {
            var stack = getStack();
            int diff = stack.top + stack.size - stack.states.Count;
            if(diff > 0)
            {
                for(int i = 0; i < diff; i++)
                    stack.states.Add(0);
            }
        }

        StateStack getStack()
        {
            return stacks_[branch_];
        }

        class StateStack
        {
            public int top = 0;
            public int size = 0;
            public List<int> states = new List<int>();
        }

        StateStack[] stacks_ = new StateStack[] { new StateStack(), new StateStack() };
        int branch_ = 0;
    }
}
