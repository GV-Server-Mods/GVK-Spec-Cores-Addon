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
using VRage.Game.ModAPI;

namespace ServerMod
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class CustomSpecCoreFunction : MySessionComponentBase
    {
        static CustomSpecCoreFunction()
        {
            SpecBlockHooks.OnReady += HooksOnOnReady;
        }

        private static void HooksOnOnReady()
        {
            SpecBlockHooks.RegisterSpecCorePointCustomFx(-101, GetShipMass);
        }

        static float GetShipMass(object specCore, List<IMyCubeGrid> grids)
        {
            var mass = 0f;
            foreach (var g in grids)
            {
                mass += g.Physics?.Mass ?? 0;
            }
            return (int)(mass / 1000);
        }
    }
}