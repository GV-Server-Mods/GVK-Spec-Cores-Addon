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
            foreach (var grid in grids)
            {				
				List<IMySlimBlock> blocks = new List<IMySlimBlock>();
				var core = SpecBlockHooks.GetMainSpecCore(grid);
				var stats = new Dictionary<int, float>();
				SpecBlockHooks.GetSpecCoreLimits(core, stats, SpecBlockHooks.GetSpecCoreLimitsEnum.CurrentStaticOrDynamic);
				if(stats.ContainsKey(23) && stats[23]!=0)
				{
					grid.GetBlocks(blocks, x=>x.FatBlock is IMyTerminalBlock);
					foreach(var block in blocks)
					{
						if(((IMyTerminalBlock)(block.FatBlock)).DefinitionDisplayNameText.Contains("Thruster"))
						{
							(block.FatBlock as IMyThrust).ThrustMultiplier = stats[23];
							Log.ChatError($"SetMultiplier to: {(block.FatBlock as IMyThrust).ThrustMultiplier}");
						}						
					}
										
				}
            }
        }
    }
}