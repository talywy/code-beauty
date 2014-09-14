using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TVGenius.Model
{
    public class MarkAttribute : Attribute
    {
        public string Mark { get; set; }

        /// <summary>
        /// 获取Mark值
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string Get(Type tp, string name)
        {
            MemberInfo[] mi = tp.GetMember(name);
            if (mi.Length > 0)
            {
                MarkAttribute attr = Attribute.GetCustomAttribute(mi[0], typeof(MarkAttribute)) as MarkAttribute;
                if (attr != null)
                {
                    return attr.Mark;
                }
            }
            return null;
        }

        public static string Get(object enm)
        {
            if (enm != null)
            {
                MemberInfo[] mi = enm.GetType().GetMember(enm.ToString());
                if (mi.Length > 0)
                {
                    MarkAttribute attr = Attribute.GetCustomAttribute(mi[0], typeof(MarkAttribute)) as MarkAttribute;
                    if (attr != null)
                    {
                        return attr.Mark;
                    }
                }
            }

            return null;
        }
    }
}
