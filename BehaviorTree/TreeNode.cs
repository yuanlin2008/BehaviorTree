namespace BehaviorTree
{
    public enum TickResult { Failure, Success, Running }

    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
    public class NodeName : System.Attribute
    {
        public NodeName(string name) { name_ = name; }
        public string name { get { return name_; } }
        string name_;
    }

    public abstract class TreeNode
    {
        /// <summary>
        /// 节点在图中的位置.
        /// </summary>
        public int x, y;

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
