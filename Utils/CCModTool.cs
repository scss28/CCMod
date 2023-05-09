using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace CCMod.Tool
{
	static partial class CCModTool
	{
		public static string GetTheSameTextureAsEntity<T>() where T : class
		{
			var type = typeof(T);
			string NameSpace = type.Namespace;
			if (NameSpace == null)
				return GetVanillaTexture<Item>(ItemID.Acorn);
			return NameSpace.Replace(".", "/") + "/" + type.Name;
		}
		public static string GetTheSameTextureAs<T>(string altName = "") where T : class
		{
			var type = typeof(T);
			if (string.IsNullOrEmpty(altName))
				altName = type.Name;
			string NameSpace = type.Namespace;
			if (NameSpace == null)
				return GetVanillaTexture<Item>(ItemID.Acorn);
			return NameSpace.Replace(".", "/") + "/" + altName;
		}
		public static string GetVanillaTexture<T>(int EntityType) where T : class
		{
			var type = typeof(T);
			if (type == typeof(NPC))
				return "Terraria/Images/NPC_" + EntityType;
			if (type == typeof(Item))
				return "Terraria/Images/Item_" + EntityType;
			if (type == typeof(Projectile))
				return "Terraria/Images/Projectile_" + EntityType;
			return GetVanillaTexture<Item>(ItemID.Acorn);
		}
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
