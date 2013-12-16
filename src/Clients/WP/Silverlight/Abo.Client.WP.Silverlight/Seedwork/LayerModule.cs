using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Abo.Client.WP.Silverlight.Seedwork
{
    public class LayerModule : Module
    {
        protected sealed override void Load(ContainerBuilder builder)
        {
            foreach (var childModule in ChildModules)
            {
                childModule.Load(builder);
            }
            OnMap(builder);
            base.Load(builder);
        }

        protected virtual void OnMap(ContainerBuilder builder)
        {
        }

        protected virtual IEnumerable<LayerModule> ChildModules
        {
            get { return Enumerable.Empty<LayerModule>(); }
        }

        public virtual void OnPostContainerBuild(IContainer container)
        {
        }
    }
}
