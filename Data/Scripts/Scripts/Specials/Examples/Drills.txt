using System.Collections.Generic;
using System.Text;
using Digi;
using Sandbox.Definitions;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using Scripts.Specials.ShipClass;
using VRage.Game;
using VRage.Game.Components;

namespace ServerMod
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class Drills : MySessionComponentBase
    {
        static Drills()
        {
            SpecBlockHooks.OnReady += OnHooksReady;
        }

        private static void OnHooksReady()
        {
            SpecBlockHooks.RegisterCustomLimitConsumer("Drills", block => new LimitedDrills(block));
        }

        /// <summary>
        /// Drills that instead of Enabled/Disabled are reducing its `DrillHarvestMultiplier` to 0
        /// Vanilla drills can drill even if they are Enabled = false 
        /// </summary>
        public class LimitedDrills : SpecBlockHooks.ILimitedBlock
        {
            private IMyShipDrill Drill;
            private float HarvestMlt;
            private float PowerConsumption;
            
            public LimitedDrills(IMyTerminalBlock block)
            {
                Drill = (block as IMyShipDrill);
                var subtype = Drill.BlockDefinition.SubtypeName;
                var tier = 1;//subtype.GetTier();
            
                HarvestMlt = 1 + 2f * ((tier-1) / 3f);
                PowerConsumption = (Drill.SlimBlock.BlockDefinition as MyCubeBlockDefinition).CubeSize == MyCubeSize.Large ? 2.5f : 0.5f;
                PowerConsumption *= 1 + (tier-1);
                Drill.ResourceSink?.SetMaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId, PowerConsumption);
                Drill.ResourceSink?.SetRequiredInputByType(MyResourceDistributorComponent.ElectricityId, PowerConsumption);
                
                Drill.DrillHarvestMultiplier = HarvestMlt;
            }

            public bool IsDrainingPoints()
            {
                return Drill.DrillHarvestMultiplier >= HarvestMlt;
            }

            public void Disable()
            {
                Drill.DrillHarvestMultiplier = 0f;
            }

            public void CanWork()
            {
                Drill.DrillHarvestMultiplier = HarvestMlt;
            }

            public bool CanBeDisabled()
            {
                return true;
            }

            public bool CheckConditions(IMyTerminalBlock specblock)
            {
                return true;
            }
        }
    }
}