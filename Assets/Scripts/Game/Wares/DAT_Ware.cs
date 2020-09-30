using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Rivenort
/// </summary>
[System.Serializable]
public class DAT_Ware
{
    
    public enum Type
    {
        NOTHING,
        TORCH,
        FEATHERS,
        BEDSHEETS,
        CHICKEN_NUGGETS,
        WOOD,
        BOARDS,
        TOYS,
        GRASS,
        WOOL,
        VEGETABLES,
        WHEELS,
        BIKE,
        CLOTH,
        SUITS,
        JEANS,
        MUSHROOMS,
        STONE,
        TOOLS,
        COAL,
        BRONZE_LUMP,
        BRONZE_BAR,
        RIDING_DINOS,
        GEMS,
        COINS,
        CARAVANS,
        IRON_ORE,
        IRON_BARS,
        JEWELRY,
        PAPER,
        STELL_BAR,
        FISH,
        SUSHI,
        CLAY,
        POTTERY,
        BOATS,
        PLUSHIES,
        GLASS_DISHES,
        CRYSTALS,
        MILK,
        CHEESE,
        PIZZA,
        LEATHER,
        BELTS_AND_BAGS,
        CHECKERED_BELTS_AND_BAGS,
        STONE_WHEELS,
        GRAIN,
        RICE,
        REAGENTS,
        SCROLLS,
        DEVICES,
        PRINTED_BOOKS,
        PILES_OF_PAPER,
        ENERGETICS,
        NEWSPAPER,
        COFFEE,
        KITCHEN_UTENSILS,
        ALLOYS_BAR,
        ICE_CREAM,
        PLASTIC_STUFF,
        STEAKS,
        BURGERS,
        BBQ,
        PHONES,
        JOURNAL,
        FLOUR,
        BREAD,
        GROCERY_CART,
        PASTRY,
        CARS
    }

    public Type type;
    public Sprite gfx;
    
    public static Type ToType(string typeName)
    {
        typeName = typeName.Replace(" ", "_");
        typeName = typeName.ToUpper();
        foreach (Type wareType in System.Enum.GetValues(typeof(Type)))
        {
            if (wareType.ToString().Equals(typeName))
                return wareType;
        }
        return Type.NOTHING;
    }

    public static DAT_Ware.Type GetRandWare()
    {
        System.Array wares = System.Enum.GetValues(typeof(Type));
        return (Type) wares.GetValue(UnityEngine.Random.Range(1, wares.Length));
    }
}
