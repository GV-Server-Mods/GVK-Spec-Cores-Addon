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
			SpecBlockHooks.OnLimitedBlockCreated += OnLimitedBlockCreated;
            //SpecBlockHooks.OnSpecBlockDestroyed += OnSpecBlockDestroyed;
        }
		
		//This takes effect works after the grid is cut/pasted or spec block is deleted
        private static void OnSpecBlockChanged(object specBlock, List<IMyCubeGrid> grids)
        {
            foreach (var grid in grids)
            {				
				List<IMySlimBlock> blocks = new List<IMySlimBlock>();
				var core = SpecBlockHooks.GetMainSpecCore(grid);
				var stats = new Dictionary<int, float>();
				SpecBlockHooks.GetSpecCoreLimits(core, stats, SpecBlockHooks.GetSpecCoreLimitsEnum.CurrentStaticOrDynamic);
				grid.GetBlocks(blocks, x=>x.FatBlock is IMyTerminalBlock);
				foreach(var block in blocks)
				{
					var thrustBlock = block.FatBlock as IMyThrust;
					var reactorBlock = block.FatBlock as IMyReactor;
					var generatorBlock = block.FatBlock as IMyGasGenerator;
					var drillBlock = block.FatBlock as IMyShipDrill;

					if(thrustBlock != null)
					{
						if(stats.ContainsKey(23) && stats[23] != 0)
						{
							thrustBlock.ThrustMultiplier = stats[23];
						}
						else
						{	
							thrustBlock.ThrustMultiplier = 1.0f;
						}
						//Log.ChatError($"SetMultiplier to: {thrustBlock.ThrustMultiplier}");
					}
					
					if(reactorBlock != null)
					{
						if(stats.ContainsKey(22) && stats[22] != 0)
						{
							reactorBlock.PowerOutputMultiplier = stats[22];
						}
						else
						{	
							reactorBlock.PowerOutputMultiplier = 1.0f;
						}
						//Log.ChatError($"SetMultiplier to: {reactorBlock.PowerOutputMultiplier}");
					}
					
					if(generatorBlock != null)
					{
						if(stats.ContainsKey(21) && stats[21] != 0)
						{
							//This is the rate of ice consumption, NOT the rate of O2/H2 output
							generatorBlock.ProductionCapacityMultiplier = stats[21];
						}
						else
						{	
							//This is the rate of ice consumption, NOT the rate of O2/H2 output
							generatorBlock.ProductionCapacityMultiplier = 1.0f;
						}
						//Log.ChatError($"SetMultiplier to: {generatorBlock.ProductionCapacityMultiplier}");
					}
					
					if(drillBlock != null)
					{
						if(stats.ContainsKey(20) && stats[20] != 0)
						{
							drillBlock.DrillHarvestMultiplier = stats[20];
						}
						else
						{	
							drillBlock.DrillHarvestMultiplier = 1.0f;
						}
						//Log.ChatError($"SetMultiplier to: {drillBlock.DrillHarvestMultiplier}");
					}
				}
            }
        }
		
		//This doesnt seem to work when expected, not sure why, is this even needed?
		private static void OnLimitedBlockCreated(object limitedBlock)
        {
			//Log.ChatError($"OnLimitedBlockCreated");
			if (limitedBlock is IMyTerminalBlock)
			{
				var block = limitedBlock as IMyTerminalBlock;
				var core = SpecBlockHooks.GetSpecCoreBlock(block);
				var stats = new Dictionary<int, float>();
				SpecBlockHooks.GetSpecCoreLimits(core, stats, SpecBlockHooks.GetSpecCoreLimitsEnum.CurrentStaticOrDynamic);
				if(stats.ContainsKey(23) && stats[23]!=0)
				{
					if(((IMyTerminalBlock)(block)).DefinitionDisplayNameText.Contains("Thruster"))
					{
						(block as IMyThrust).ThrustMultiplier = stats[23];
						//Log.ChatError($"SetMultiplier to: {(block as IMyThrust).ThrustMultiplier}");
					}						
				}
			}
		}
		
		//This isnt working because OnSpecBlockDestroyed returns the block and not the grid
        /*private static void OnSpecBlockDestroyed(object limitedBlock)
        {
            List<IMySlimBlock> blocks = new List<IMySlimBlock>();
			foreach (var grid in grids)
            {                
				blocks.Clear();
                grid.GetBlocks(blocks, x=>x.FatBlock is IMyTerminalBlock);
				foreach(var block in blocks)
				{
					if(((IMyTerminalBlock)(block.FatBlock)).DefinitionDisplayNameText.Contains("Thruster"))
					{
						(block.FatBlock as IMyThrust).ThrustMultiplier = 1.0f;
						Log.ChatError($"SetMultiplier to: {(block.FatBlock as IMyThrust).ThrustMultiplier}");
					}                        
				}
            }
        }*/
    }
}