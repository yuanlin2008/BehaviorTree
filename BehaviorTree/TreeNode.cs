using System.Diagnostics;

namespace BehaviorTree
{
    public enum TickResult { Failure, Success, Running }

    public abstract class TreeNode
    {
        /// <summary>
        /// 以此节点为根节点tick行为树.
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
        /// 获得节点需要的int状态大小.
        /// </summary>
        /// <returns></returns>
        protected virtual int getStateSize() { return 0; }

        /// <summary>
        /// 节点的tick逻辑.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="init">是否为初始运行</param>
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
