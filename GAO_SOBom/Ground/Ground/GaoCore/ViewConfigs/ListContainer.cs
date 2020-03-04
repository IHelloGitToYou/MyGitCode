using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaoCore.ViewConfigs
{
    public class ListContainer : IContainer
    {
        public ListContainer()
        {
            Container = "list";
            ShowFields = new List<ViewPropery>();
        }

        public List<ExtjsViewCommand> Commands;
        public string Container {get;set;}
        public Layout Layout { get; set; }
        public LayoutConfig ContainerLayoutConfig { get; set; }
        public List<ViewPropery> ShowFields;

        public void Add(ViewPropery propery)
        {
            ShowFields.Add(propery);
        }


        public IContainer UseCommands(params Type[] P_Commands)
        {
            if (Commands == null)
                Commands = new List<ExtjsViewCommand>();

            foreach (var item in P_Commands)
            {
                if (item.IsSubclassOf(typeof(ExtjsViewCommand)) == false)
                    continue;

                var itemObj = item.Assembly.CreateInstance(item.FullName) as ExtjsViewCommand;
                if (Commands.Where(o => o.CommandPath == itemObj.CommandPath).FirstOrDefault() == null)
                    Commands.Add(itemObj);
            }

            return this;
        }
    }
    
}
