﻿using ContentLibraries;
using Items;
using UnityEngine;

// Literally just loads the sprites for the actor
public class HumanSpriteLoader : MonoBehaviour
{
	public void LoadSprites(string raceId, string hairId, string hatId, string shirtId, string pantsId)
	{
		Sprite[] bodySprites = new Sprite[0];
		Sprite[] swooshSprites = new Sprite[0];
		Sprite[] hairSprites = new Sprite[4];
		Sprite[] hatSprites = new Sprite[4];
		Sprite[] shirtSprites = new Sprite[12];
		Sprite[] pantsSprites = new Sprite[12];

		ActorRace race = ContentLibrary.Instance.Races.Get(raceId);
		if (race != null)
		{
			bodySprites = ContentLibrary.Instance.Races.Get(raceId).BodySprites.ToArray();
			swooshSprites = ContentLibrary.Instance.Races.Get(raceId).SwooshSprites.ToArray();
		}
		else {
			Debug.LogWarning("No race found for race ID " + raceId);
		}

		if (hairId != null)
		{
			Hair hair = ContentLibrary.Instance.Hairs.Get(hairId);
			if (hair != null)
			{
				hairSprites = hair.sprites;
			}
		}
		if (hatId != null)
		{
			if (ContentLibrary.Instance.Items.Get(hatId) is IHat)
			{
				IHat hat = (IHat)ContentLibrary.Instance.Items.Get(hatId);
				if (hat != null)
					hatSprites = hat.GetHatSprites();
			}
		}
		if (shirtId != null)
		{
			if (ContentLibrary.Instance.Items.Get(shirtId) is Shirt)
			{
				Shirt shirt = (Shirt)ContentLibrary.Instance.Items.Get(shirtId);
				if (shirt != null)
					shirtSprites = shirt.GetShirtSprites();
			}
		}
		if (pantsId != null)
		{
			if (ContentLibrary.Instance.Items.Get(pantsId) is Pants)
			{
				Pants pants = (Pants)ContentLibrary.Instance.Items.Get(pantsId);
				if (pants != null)
					pantsSprites = pants.GetPantsSprites();
			}
		}

		GetComponent<ActorSpriteController>().SetSpriteArrays(bodySprites, swooshSprites, hairSprites, hatSprites,
			shirtSprites, pantsSprites, race.BounceUpperSprites);
	}
}
