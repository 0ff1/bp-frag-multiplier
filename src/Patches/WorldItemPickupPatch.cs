using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace BPFragMultiplier.Patches;

[HarmonyPatch(typeof(WorldItem), nameof(WorldItem.Pickup))]
public static class WorldItemPickupPatch
{
    private static readonly HashSet<uint> BP_PREFAB_IDS = new()
    {
        2187047607, // ShortPrefabName: blueprintpage_singlepickup.entity
        484967355, // ShortPrefabName: blueprintpage_pickup.entity
        2242060430, // ShortPrefabName: blueprintbook_pickup.entity
    };

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var instructionsList = instructions.ToList();
        var giveItem = AccessTools.Method(typeof(BaseEntity), nameof(BaseEntity.GiveItem));

        for (var i = 0; i < instructionsList.Count; i++)
        {
            if (
                instructionsList[i].opcode != OpCodes.Callvirt
                || instructionsList[i].operand as MethodInfo != giveItem
            )
                continue;

            instructionsList.Insert(i++, new CodeInstruction(OpCodes.Ldarg_0));
            instructionsList.Insert(i++, new CodeInstruction(OpCodes.Ldloc_1));
            instructionsList.Insert(
                i++,
                new CodeInstruction(
                    OpCodes.Call,
                    AccessTools.Method(typeof(WorldItemPickupPatch), nameof(SetFragmentAmount))
                )
            );

            break;
        }

        return instructionsList.AsEnumerable();
    }

    public static void SetFragmentAmount(WorldItem worldItem, Item item)
    {
        if (!BP_PREFAB_IDS.Contains(worldItem.prefabID))
            return;

        item.amount = item.info.name switch
        {
            "blueprintpage" => item.amount
                * BPFragMultiplier.Config.Data.BlueprintPageMultiplier,
            "blueprintbook" => item.amount
                * BPFragMultiplier.Config.Data.BlueprintBookMultiplier,
            _ => item.amount,
        };
    }
}
