using System.Collections.Generic;
using System.Text;
using Digi;
using Scripts.Specials.ShipClass;
using VRage.Game.Components;

namespace ServerMod
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class Events : MySessionComponentBase
    {
        static Events()
        {
            SpecBlockHooks.OnReady += HooksOnOnReady;
        }

        private static void HooksOnOnReady()
        {
            SpecBlockHooks.OnLimitedBlockCreated += HooksOnOnLimitedBlockCreated;
            SpecBlockHooks.OnSpecBlockCreated += HooksOnOnSpecBlockCreated;
        }

        private static void HooksOnOnSpecBlockCreated(object obj)
        {
            Log.ChatError("MOD:SpecBlockCreated:"+obj);
        }

        private static void HooksOnOnLimitedBlockCreated(object obj)
        {
            Log.ChatError("MOD:LimitedBlockCreated:"+obj);
        }
    }
}