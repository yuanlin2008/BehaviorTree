using System.Collections.Generic;
using System.Diagnostics;

namespace BehaviorTree
{
    public class TreeState
    {
        public int getState(int id = 0) 
        {
            Debug.Assert(id > size_ - 1);
            return stack_[depth_ + id];
        }
        public void setState(int id, int value)
        {
            Debug.Assert(id > size_ - 1);
            stack_[depth_ + id] = value;
        }

        public void reset(int size)
        {
            depth_ = 0;
            size_ = size;
            expendStack();
        }
        public void push(int size)
        {
            depth_ += size_;
            size_ = size;
            expendStack();
        }
        public void pop(int size)
        {
            depth_ -= size;
            size_ = size;
        }

        void expendStack()
        {
            int diff = depth_ + size_ - stack_.Count;
            if(diff > 0)
            {
                for(int i = 0; i < diff; i++)
                    stack_.Add(0);
            }
        }

        int depth_ = 0;
        int size_ = 0;
        List<int> stack_ = new List<int>();
    }
}
