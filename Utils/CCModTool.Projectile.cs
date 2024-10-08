using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace CCMod.Utils
{
	static partial class CCModTool
	{
		/// <summary>
		/// This method should be put in PreDraw hook, it is strictly work with projectile<br/>
		/// Example :
		/// <code>
		/// public override bool PreDraw(Color lightcolor)
		/// {
		///		Projectile.DrawTrail(lightcolor, 0.02f);
		///		return true;
		/// }
		/// </code>
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="color"></param>
		/// <param name="ScaleAccordinglyToLength"></param>
		public static void DrawTrail(this Projectile projectile, Color color, float ScaleAccordinglyToLength = 0)
		{
			Main.instance.LoadProjectile(projectile.type);
			Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
			var origin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
				Color ColorAlpha = projectile.GetAlpha(color) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, ColorAlpha, projectile.rotation, origin, projectile.scale - k * ScaleAccordinglyToLength, SpriteEffects.None, 0);
			}
		}
		public static void ProjectileDefaultDrawInfo(this Projectile projectile, out Texture2D texture, out Vector2 origin)
		{
			Main.instance.LoadProjectile(projectile.type);
			texture = TextureAssets.Projectile[projectile.type].Value;
			origin = texture.Size() * .5f;
		}
		/// <summary>
		/// This method should be put in PreDraw hook, it is strictly work with projectile<br/>
		/// Example :
		/// <code>
		/// public override bool PreDraw(Color lightcolor)
		/// {
		///		Projectile.DrawTrail(lightcolor, 0.02f);
		///		return true;
		/// }
		/// </code>
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="color"></param>
		/// <param name="ScaleAccordinglyToLength"></param>
		public static void DrawTrailWithoutColorAdjustment(this Projectile projectile, Color lightColor, float ManualScaleAccordinglyToLength = 0)
		{
			projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
			if (ProjectileID.Sets.TrailingMode[projectile.type] != 2)
			{
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
					Main.EntitySpriteDraw(texture, drawPos, null, lightColor, projectile.rotation, origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
				}
			}
			else
			{
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
					Main.EntitySpriteDraw(texture, drawPos, null, lightColor, projectile.oldRot[k], origin, projectile.scale - k * ManualScaleAccordinglyToLength, SpriteEffects.None, 0);
				}
			}
		}
		/// <summary>
		/// This method will ensure that you will spawn a projectile like how starfury sword do<br/>
		/// Use <see cref="SpawnProjectileLikeStarFuryItem(Player, float, int, int, float, Vector2, bool)"/> if the projectile going to be spawn from a item
		/// </summary>
		/// <param name="source">Source of the spawn</param>
		/// <param name="player">The player</param>
		/// <param name="speed">The speed of the projectile</param>
		/// <param name="ProjectileType">The type of projectile that going to be spawn</param>
		/// <param name="damage">The damage of that projectile</param>
		/// <param name="knockback">The knockback of that projectile</param>
		/// <param name="Range">Spawn position on X and Y axis, you only need to set positive if you gonna randomize it, otherwise no need to set it</param>
		/// <param name="randomizePosition">Make your range position randomize instead of fixed</param>
		public static int SpawnProjectileLikeStarFury(IEntitySource source, Player player, float speed, int ProjectileType, int damage, float knockback, Vector2 Range, bool randomizePosition = true)
		{
			float RandomizeX = randomizePosition ? Main.rand.NextFloat(-Range.X, Range.X) : Range.X;
			float RandomizeY = randomizePosition ? Main.rand.NextFloat(-Range.Y, Range.Y) : Range.Y;
			Vector2 spawn = new Vector2(player.Center.X + RandomizeX, player.Center.Y - 1000 + RandomizeY);
			Vector2 velocity = (Main.MouseWorld - spawn).SafeNormalize(Vector2.Zero) * speed;
			return Projectile.NewProjectile(source, spawn, velocity, ProjectileType, damage, knockback, player.whoAmI);
		}
		/// <summary>
		/// This method will ensure that you will spawn a projectile like how starfury sword do<br />
		/// Use <see cref="SpawnProjectileLikeStarFury(IEntitySource, Player, float, int, int, float, Vector2, bool)"/> if the projectile not spawning from a item
		/// </summary>
		/// <param name="player">The player</param>
		/// <param name="speed">The speed of the projectile</param>
		/// <param name="ProjectileType">The type of projectile that going to be spawn</param>
		/// <param name="damage">The damage of that projectile</param>
		/// <param name="knockback">The knockback of that projectile</param>
		/// <param name="Range">Spawn position on X and Y axis, you only need to set positive if you gonna randomize it, otherwise no need to set it</param>
		/// <param name="randomizePosition">Make your range position randomize instead of fixed</param>
		public static int SpawnProjectileLikeStarFuryItem(Player player, float speed, int ProjectileType, int damage, float knockback, Vector2 Range, bool randomizePosition)
		{
			float RandomizeX = randomizePosition ? Main.rand.NextFloat(-Range.X, Range.X) : Range.X;
			float RandomizeY = randomizePosition ? Main.rand.NextFloat(-Range.Y, Range.Y) : Range.Y;
			Vector2 spawn = new Vector2(player.Center.X + RandomizeX, player.Center.Y - 1000 + RandomizeY);
			Vector2 velocity = (Main.MouseWorld - spawn).SafeNormalize(Vector2.Zero) * speed;
			return Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), spawn, velocity, ProjectileType, damage, knockback, player.whoAmI);
		}
		/// <summary>
		/// Go through all projectiles in Main.projectile[] <br/>
		/// Check if the projectile is the type that you looking for<br/>
		/// and if it's position to parameter position is smaller than parameter distance
		/// </summary>
		/// <param name="position">position want to check</param>
		/// <param name="type">type of projectile</param>
		/// <param name="distance">distance to check</param>
		/// <returns>Return true if it's type equal type you look for and its distance to the position is smaller than distance<br/>
		/// otherwise return false</returns>
		public static bool LookForProjectile(Vector2 position, int type, float distance)
		{
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i] is { active: true } proj && proj.type == type && proj.WithinRange(position, distance))
				{
					return true;
				}
			}
			return false;
		}
	}
}
