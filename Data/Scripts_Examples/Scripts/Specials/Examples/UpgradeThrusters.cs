using System.Collections.Generic;
using Digi;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using Scripts.Specials.ShipClass;
using VRage.Game.Components;
using VRage.Game.ModAPI;

namespace ServerMod
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class UpgradeThrusters : MySessionComponentBase
    {
        static UpgradeThrusters()
        {
            SpecBlockHooks.OnReady += HooksOnOnReady;
        }

        private static void HooksOnOnReady()
        {
            SpecBlockHooks.OnSpecBlockChanged += OnSpecBlockChanged;
        }

        private static void OnSpecBlockChanged(object specBlock, List<IMyCubeGrid> grids)
        {
            Log.ChatError($"OnSpecBlockChanged: SpecBlock={specBlock} Grids={grids.Count}");
            foreach (var grid in grids)
            {
                var blocksByType = SpecBlockHooks.GetGridBlocksByType(grid);
                if (blocksByType != null)
                {
                    HashSet<IMyCubeBlock> thrusters;
                    if (blocksByType.TryGetValue(typeof(MyThrust), out thrusters))
                    {
                        foreach (var thruster in thrusters)
                        {
                            (thruster as IMyThrust).ThrustMultiplier = specBlock == null ? 1 : 4;
                            Log.ChatError($"SetMultiplier {(thruster as IMyThrust).ThrustMultiplier}");
                        }
                    } 
                    else
                    {
                        Log.ChatError("blocksByType Thruster is null");
                    }
                }
                else
                {
                    Log.ChatError("blocksByType is null");
                }
            }
        }
    }
}