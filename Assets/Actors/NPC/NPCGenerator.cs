﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGenerator : MonoBehaviour
{
	static System.Random random;
    public static NPCData Generate ()
    {
        List<Hair> hairPool = HairLibrary.GetHairs();
        List<Hat> hatPool = ItemLibrary.GetHats();
        List<Shirt> shirtPool = ItemLibrary.GetShirts();
        List<Pants> pantsPool = ItemLibrary.GetPants();

        Hair hair = hairPool.PickRandom();
        Hat hat = hatPool.PickRandom();
        Shirt shirt = shirtPool.PickRandom();
        Pants pants = pantsPool.PickRandom();

        string hatId = hat.ItemId;

		// 50% chance of no hat 
		if (random == null)
			random = new System.Random();
        if (random.Next(2) == 0)
            hatId = null;


        Gender gender = GenderHelper.RandomGender();
        string name = NameGenerator.Generate(gender);
		ActorInventory.InvContents inv = new ActorInventory.InvContents();
		inv.equippedHat = hat;
		inv.equippedShirt = shirt;
		inv.equippedPants = pants;

        return new NPCData(NPCDataMaster.GetUnusedId(name), name, "human_base", hair.hairId, gender, inv);
    }

}
