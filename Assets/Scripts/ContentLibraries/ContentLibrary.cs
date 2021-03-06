﻿
// Loads and stores references to all the content libraries in the game.
namespace ContentLibraries
{
	public class ContentLibrary
	{
		private static ContentLibrary instance;
		public static ContentLibrary Instance
		{
			get { return instance ??= new ContentLibrary(); }
		}

		public static bool Loaded { get; private set; }

		public GenericContentLibrary<Biome> Biomes { get; private set; }
		public GenericContentLibrary<Biotope> Biotopes { get; private set; }
		public GenericContentLibrary<ActorTemplate> ActorTemplates { get; private set; }
		public ContainerLayoutElementPrefabLibrary ContainerLayoutElementPrefabs { get; private set; }
		public GenericContentLibrary<EntityData> Entities { get; private set; }
		public GenericContentLibrary<GroundMaterial> GroundMaterials { get; private set; }
		public GenericContentLibrary<Hair> Hairs { get; private set; }
		public ItemLibrary Items { get; private set; }
		public NameLibrary Names { get; private set; }
		public PersonalityLibrary Personalities { get; private set; }
		public RaceLibrary Races { get; private set; }
		public RegionFeatureLibrary RegionFeatures { get; private set; }

		public void LoadAllLibraries ()
		{
			Biomes = new GenericContentLibrary<Biome>("BiomeLibrary");
			Biotopes = new GenericContentLibrary<Biotope>("BiotopeLibrary");
			ActorTemplates = new GenericContentLibrary<ActorTemplate>("CharacterGenTemplateLibrary");
			ContainerLayoutElementPrefabs = new ContainerLayoutElementPrefabLibrary();
			Entities = new GenericContentLibrary<EntityData>("EntityLibrary");
			GroundMaterials = new GenericContentLibrary<GroundMaterial>("GroundMaterialLibrary");
			Hairs = new GenericContentLibrary<Hair>("HairLibrary");
			Items = new ItemLibrary();
			Names = new NameLibrary();
			Personalities = new PersonalityLibrary();
			Races = new RaceLibrary();
			RegionFeatures = new RegionFeatureLibrary();

			Biomes.LoadLibrary();
			Biotopes.LoadLibrary();
			ActorTemplates.LoadLibrary();
			ContainerLayoutElementPrefabs.LoadLibrary();
			Entities.LoadLibrary();
			GroundMaterials.LoadLibrary();
			Hairs.LoadLibrary();
			Items.LoadLibrary();
			Names.LoadLibrary();
			Personalities.LoadLibrary();
			Races.LoadLibrary();
			RegionFeatures.LoadLibrary();

			Loaded = true;
		}
	}
}