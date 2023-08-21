using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorTree
{
    public class Manager
    {
        public static Dictionary<string, Type> nodeTypes
        {
            get
            {
                if (nodeTypes_ != null)
                    return nodeTypes_;
                nodeTypes_ = new Dictionary<string, Type>();
                foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach(var t in assembly.GetTypes())
                    {
                        if(t.IsSubclassOf(typeof(TreeNode))
                            && !t.IsAbstract
                            && Attribute.IsDefined(t, typeof(NodeName)))
                        {
                            nodeTypes.Add(((NodeName)(Attribute.GetCustomAttribute(t, typeof(NodeName)))).name, t);
                        }
                    }
                }
                return nodeTypes_;
            }
        }


        static Dictionary<string, Type> nodeTypes_;
    }
}
