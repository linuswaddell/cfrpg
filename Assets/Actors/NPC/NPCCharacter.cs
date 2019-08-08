using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCharacter
{
	public string NPCName { get; private set;}
	public Gender NPCGender {get; private set;}


	public void Init (NPCData data) {
		NPCName = data.NpcName;
		NPCGender = data.Gender;
	}
}
