using System;

namespace RingSizeCreator
{
    /// <summary>
    /// This is the main class that identifies the assembly as a Rhino Plugin.
    /// It's mostly boilerplate code.
    /// </summary>
    public class RingSizeCreatorPlugin : Rhino.PlugIns.PlugIn
    {
        public RingSizeCreatorPlugin()
        {
            Instance = this;
        }

        public static RingSizeCreatorPlugin Instance { get; private set; }

        // You can override methods here to change loading behavior if needed.
    }
}