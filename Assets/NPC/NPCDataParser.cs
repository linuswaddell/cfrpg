﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NPCDataParser {

	public static List<NPCData> Parse (string jsonString) {
		List<NPCData> list = new List<NPCData> ();
		SimpleJSON.JSONNode json = SimpleJSON.JSON.Parse (jsonString);
		for (int i = 0; i < JSONHelper.GetElementCount(json); i++) {
			string npcId;
			string spriteName;
			string npcName;
			string npcGender;
			List<NPCData.Relationship> relationships;
			List<NPCData.ScheduleEvent> schedule;
			npcName = json [i] ["name"];
			npcId = json [i] ["id"];
			spriteName = json [i] ["sprite"];
			npcGender = json [i] ["gender"];

			relationships = new List<NPCData.Relationship> ();
			for (int j = 0; j < JSONHelper.GetElementCount(json[i]["relationships"]); j++) {
				SimpleJSON.JSONNode relJson = json [i] ["relationships"] [j];
				relationships.Add (new NPCData.Relationship(relJson["id"], relJson["value"]));
			}

			schedule = new List<NPCData.ScheduleEvent> ();
			for (int j = 0; j < JSONHelper.GetElementCount(json[i]["schedule"]); j++) {
				SimpleJSON.JSONNode itemJson = json [i] ["schedule"] [j];
				int startTime = itemJson ["startTime"];
				string eventId = itemJson ["action"];
				List<WeekDay> days = new List<WeekDay> ();
				for (int k = 0; k < JSONHelper.GetElementCount(itemJson["days"]); k++) {
					days.Add (WeekDayMethods.WeekdayFromString (itemJson ["days"] [k]));
				}
				schedule.Add (new NPCData.ScheduleEvent(startTime, days, eventId));
			}
			NPCData npc = new NPCData (npcId, npcName, spriteName, npcGender, schedule, relationships);
			list.Add (npc);
		}
		return list;
	}
}
