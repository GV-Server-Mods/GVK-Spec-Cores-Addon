using System;
using System.Collections.Generic;
using System.Text;
using Digi;
using MIG.Shared.CSharp;
using MIG.Shared.SE;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using Scripts.Specials.ShipClass;
using VRage.Game.Components;

namespace ServerMod
{


    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class CustomAntenna : MySessionComponentBase
    {
        static CustomAntenna()
        {
            SpecBlockHooks.OnReady += HooksOnOnReady;
        }

        private static void HooksOnOnReady()
        {
            SpecBlockHooks.OnSpecBlockCreated += SpecBlockHooksOnOnSpecBlockCreated;
        }

        private static void SpecBlockHooksOnOnSpecBlockCreated(object obj)
        {
            var bl = SpecBlockHooks.GetBlockSpecCore(obj);
            if (bl is IMyRadioAntenna)
            {
                new RadioAntennaExtension((IMyRadioAntenna)bl, obj);
            }
        }

        public override void UpdateBeforeSimulation()
        {
            base.UpdateBeforeSimulation();
            FrameExecutor.Update();
        }
    }
    
    public class RadioAntennaExtension
    {
        public IMyRadioAntenna antenna;
        public object specBlock;
        private static IMyTerminalControlSlider radiusSlider;
        private static Dictionary<int, float> buffer = new Dictionary<int, float>();
        private static List<int> bufferUpgrades = new List<int>();
        
        public RadioAntennaExtension(IMyRadioAntenna antenna, object specBlock)
        {
            this.specBlock = specBlock;
            this.antenna = antenna;
            if (MyAPIGateway.Session.IsServer)
            {
                FrameExecutor.addFrameLogic(new AutoTimer(100, 100), antenna, l => CheckAntenna());
            }
        }

        protected virtual void CheckAntenna()
        {
            if (!antenna.Enabled) { antenna.Enabled = true; }
            if (!antenna.EnableBroadcasting) { antenna.EnableBroadcasting = true; }
            if (!antenna.ShowShipName) { antenna.ShowShipName = true; }

            buffer.Clear();
            bufferUpgrades.Clear();
            SpecBlockHooks.GetSpecCoreLimits(specBlock, buffer, antenna.CubeGrid.IsStatic ? SpecBlockHooks.GetSpecCoreLimitsEnum.StaticLimits : SpecBlockHooks.GetSpecCoreLimitsEnum.DynamicLimits);
            SpecBlockHooks.GetSpecCoreUpgrades(specBlock, bufferUpgrades);
            
            antenna.HudText = GetAntennaText(bufferUpgrades);
            
            if (antenna.Radius < buffer[-50])
            {
                antenna.Radius = buffer[-50];
            }
        }
        
        private static string GetAntennaText(List<int> upgrades)
        {
            var s = new StringBuilder("Custom Antenna text");
            foreach (var u in upgrades)
            {
                s.Append(" " + u);
            }
            return s.ToString();
        }
        
    }
}