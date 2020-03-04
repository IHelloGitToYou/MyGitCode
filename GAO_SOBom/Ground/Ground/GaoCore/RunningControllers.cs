using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GaoCore
{
    public class RunningControllers
    {
        /// <summary>
        /// 缓存DLL的数据库名
        /// </summary>
        public static Dictionary<Assembly, string> CacheAssemblyDBs = new Dictionary<Assembly, string>();
        static List<Assembly> Assemblys = new List<Assembly>();
        public static void Register(Assembly assembly)
        {
            if (Assemblys.Where(o => o.FullName == assembly.FullName).FirstOrDefault() == null)
            {
                var attribute = assembly.CustomAttributes.Where(o => o.AttributeType == typeof(ControllerEntity)).FirstOrDefault();
                if (attribute == null)
                    throw new StructureException("Dll未指定");

                if (attribute.ConstructorArguments[0].Value.GetType() == typeof(bool))
                    CacheAssemblyDBs.Add(assembly, "X-LOGIN-ID");
                else
                    CacheAssemblyDBs.Add(assembly, attribute.ConstructorArguments[0].Value.ToString());

                Assemblys.Add(assembly);
                attribute.Constructor.Invoke(attribute.ConstructorArguments.Select(o => o.Value).ToArray());
            }
        }

        public static List<Assembly> GetAll()
        {
            return Assemblys;
        }
    }
}
