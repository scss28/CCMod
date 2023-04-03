using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Common
{
	public class SoundManager
	{
		public static Dictionary<string, SoundStyle> Sounds { get; private set; }

		static readonly string[] loadFolders = new string[]
		{
			"SFX"
		};

		static readonly string[] soundTypes = new string[]
		{
			"wav",
			"ogg",
			"mp3"
		};

		public static void Load(Mod mod)
		{
			Sounds = new Dictionary<string, SoundStyle>();
			foreach (string file in mod.GetFileNames())
			{
				foreach (string folder in loadFolders)
				{
					string startsWith = $"Assets/Audio/{folder}/";

					if (file.StartsWith(startsWith))
					{
						foreach (string type in soundTypes)
						{
							string endsWith = "." + type;
							if (file.EndsWith(endsWith))
							{
								string path = file.Replace(endsWith, string.Empty);
								string name = path.Replace(startsWith, string.Empty);

								Sounds[name] = new SoundStyle("CCMod/" + path);
							}
						}
					}
				}
			}
		}
	}
}