﻿
// Stores all the data associated with an actor's identity (but not their physical location)

using System.Collections.Generic;
using ContentLibraries;

public class ActorData
{
	public ActorData(
		string actorId,
		string actorName,
		string personality,
		string RaceId,
		Gender gender,
		string hair,
		ActorPhysicalCondition physicalCondition,
		ActorInventory.InvContents inventory,
		int walletMoney,
		FactionStatus factionStatus,
		string profession)
	{
		this.actorId = actorId;
		ActorName = actorName;
		Personality = personality;
		this.RaceId = RaceId;
		Gender = gender;
		Hair = hair;
		Inventory = new ActorInventory();
		Inventory.SetInventory(inventory ?? new ActorInventory.InvContents());
		Wallet = new ActorWallet(walletMoney);
		FactionStatus = factionStatus ?? new FactionStatus(null);
		Profession = profession;
		
		ActorRace race = ContentLibrary.Instance.Races.Get(this.RaceId);
		PhysicalCondition = physicalCondition ?? new ActorPhysicalCondition(race.MaxHealth, race.MaxHealth);
	}

	public readonly string actorId;
	public string ActorName { get; set; }
	public string Personality { get; set; }
	public string RaceId { get; set; }
	public Gender Gender { get; set; }
	public string Hair { get; set; }
	public ActorPhysicalCondition PhysicalCondition { get; }
	public ActorInventory Inventory { get; set; }
	public ActorWallet Wallet { get; set; }
	public FactionStatus FactionStatus { get; }
	public ActorLocationMemories Memories { get; }
	public List<Relationship> Relationships { get; }
	public string Profession { get; set; }

	[System.Serializable]
	public struct Relationship
	{
		public string id;
		public float value;
		public Relationship(string id, float val)
		{
			this.id = id;
			this.value = val;
		}
	}
}
