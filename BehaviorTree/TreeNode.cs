using System.Diagnostics;

namespace BehaviorTree
{
    public enum TickResult { Failure, Success, Running }

    public abstract class TreeNode
    {
        /// <summary>
        /// �Դ˽ڵ�Ϊ���ڵ�tick��Ϊ��.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public TickResult tickRoot(TreeState s)
        {
            bool isRunning = s.isRunning;
            s.reset();
            var r = tickNode(s, !isRunning);
            Debug.Assert(s.isRunning == (r == TickResult.Running));
            return r;
        }

        /// <summary>
        /// ��ýڵ���Ҫ��int״̬��С.
        /// </summary>
        /// <returns></returns>
        protected virtual int getStateSize() { return 0; }

        /// <summary>
        /// �ڵ��tick�߼�.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="init">�Ƿ�Ϊ��ʼ����</param>
        /// <returns></returns>
        protected virtual TickResult tick(TreeState state, bool init) 
        { 
            return TickResult.Failure; 
        }

        public TickResult tickNode(TreeState state, bool init)
        {
            var lastSize = state.push(getStateSize());
            var result = tick(state, init);
            if(result != TickResult.Running)
                state.pop(lastSize);
            return result;
        }
    }
}
