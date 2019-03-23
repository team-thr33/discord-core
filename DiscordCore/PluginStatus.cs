using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordCore
{
    public enum PluginStatus
    {
        RUNNING,
        ERROR,
        LOADED,
        STOPPED,
        INITIALIZED,
        VALIDATED,
        INVALID,
    }

    public enum PluginLoadStatus
    {
        UNKNOWN,
        VALID,
        INVALID
    }
}