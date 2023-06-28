using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CCMod.Utils
{
	static partial class CCModTool
	{
		public static string GetSameTextureAs<T>() where T : class
		{
			Type type = typeof(T);

			if (type.IsSubclassOf(typeof(ModTexturedType)) && ModContent.GetInstance<T>() is ModTexturedType instance)
				return instance.Texture;

			string Namespace = type.Namespace;
			if (Namespace == null)
				return GetVanillaTexture<Item>(ItemID.Acorn);
			return Namespace.Replace(".", "/") + "/" + type.Name;
		}
		public static string GetTheSameTextureAs<T>(string altName = "") where T : class
		{
			Type type = typeof(T);
			if (string.IsNullOrEmpty(altName))
				altName = type.Name;
			string NameSpace = type.Namespace;
			if (NameSpace == null)
				return GetVanillaTexture<Item>(ItemID.Acorn);
			return NameSpace.Replace(".", "/") + "/" + altName;
		}
		public static string GetVanillaTexture<T>(int EntityType) where T : class => $"Terraria/Images/{typeof(T).Name}_{EntityType}";
		public static bool CompareSquareFloatValue(Vector2 pos1, Vector2 pos2, float maxDistance)
		{
			double value1X = pos1.X,
				value1Y = pos1.Y,
				value2X = pos2.X,
				value2Y = pos2.Y,
				DistanceX = value1X - value2X,
				DistanceY = value1Y - value2Y,
				maxDistanceDouble = maxDistance * maxDistance;
			return DistanceX * DistanceX + DistanceY * DistanceY < maxDistanceDouble;
		}
	}
}
