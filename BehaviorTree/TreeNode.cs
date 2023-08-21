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
        /// �ڵ���ͼ�е�λ��.
        /// </summary>
        public int x, y;

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
