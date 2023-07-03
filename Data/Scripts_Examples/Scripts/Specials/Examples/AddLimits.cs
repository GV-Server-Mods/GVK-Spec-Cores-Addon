using System.Collections.Generic;
using System.Text;
using Digi;
using Sandbox.ModAPI;
using Scripts.Specials.ShipClass;
using VRage.Game.Components;

namespace ServerMod
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation)]
    public class AddLimits : MySessionComponentBase
    {
        static AddLimits()
        {
            SpecBlockHooks.OnReady += HooksOnOnReady;
        }

        private static void HooksOnOnReady()
        {
            SpecBlockHooks.OnSpecBlockCreated += HooksOnOnSpecBlockCreated;
        }

        private static HashSet<object> SpecBlocks = new HashSet<object>();

        private static void HooksOnOnSpecBlockCreated(object obj)
        {
            SpecBlocks.Add(obj);
        }
        
        public override void UpdateBeforeSimulation()
        {
            base.UpdateBeforeSimulation();
            if (!SpecBlockHooks.IsReady()) return;

            int mlt = 1+MyAPIGateway.Session.GameplayFrameCounter / 300;
            if (MyAPIGateway.Session.GameplayFrameCounter % 300 == 0)
            {
                var staticV = new Dictionary<int, float>();
                var dynamicV = new Dictionary<int, float>();
                
                foreach (var o in SpecBlocks)
                {
                    staticV[1] = mlt * 3;
                    dynamicV[1] = mlt;
                    SpecBlockHooks.SetSpecCoreCustomValues(o, staticV, dynamicV);
                    Log.ChatError("SetSpecCoreCustomValues");
                }
            }    
        }
    }
}