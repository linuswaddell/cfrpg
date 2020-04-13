﻿
public class ContentLibrary
{
	private static ContentLibrary instance;
	public static ContentLibrary Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new ContentLibrary();
			}
			return instance;
		}
	}

	public BiotopeLibrary Biotopes { get; private set; }
	public EntityLibrary Entities { get; private set; }
	public GroundMaterialLibrary GroundMaterials { get; private set; }
	public HairLibrary Hairs { get; private set; }
	public ItemLibrary Items { get; private set; }
	public PersonalityLibrary Personalities { get; private set; }
	public RaceLibrary Races { get; private set; }

	public void LoadAllLibraries ()
	{
		Biotopes = new BiotopeLibrary();
		Entities = new EntityLibrary();
		GroundMaterials = new GroundMaterialLibrary();
		Hairs = new HairLibrary();
		Items = new ItemLibrary();
		Personalities = new PersonalityLibrary();
		Races = new RaceLibrary();

		Biotopes.LoadLibrary();
		Entities.LoadLibrary();
		GroundMaterials.LoadLibrary();
		Hairs.LoadLibrary();
		Items.LoadLibrary();
		Personalities.LoadLibrary();
		Races.LoadLibrary();
	}
}