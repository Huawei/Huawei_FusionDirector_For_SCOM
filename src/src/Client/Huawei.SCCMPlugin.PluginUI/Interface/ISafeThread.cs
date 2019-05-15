using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Interface
{
    public interface ISafeThread
    {
        void SafeInvoke(Action action);
    }
}
