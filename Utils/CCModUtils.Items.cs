using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Utils
{
	partial class CCModUtils
	{
		/// <summary>
		/// Sets default stats for the given item to a sword.
		/// </summary>
		/// <remarks>If you're seeing this, this is an extension, not a method inside <see cref="Item"/>.</remarks>
		/// <param name="item"></param>
		/// <param name="damage"></param>
		/// <param name="swingTime"></param>
		/// <param name="knockback"></param>
		/// <param name="autoReuse"></param>
		public static void DefaultToSword(this Item item, int damage, int swingTime, float knockback = 0, bool autoReuse = true)
		{
			item.useStyle = ItemUseStyleID.Swing;
			item.DamageType = DamageClass.Melee;
			item.damage = damage;
			item.useTime = item.useAnimation = swingTime;
			item.knockBack = knockback;
			item.autoReuse = autoReuse;
			item.UseSound = SoundID.Item1;
		}

		/// <summary>
		/// This return a offset Vector2, useful to make bullet appear out of muzzle.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="velocity">velocity</param>
		/// <param name="offSetBy">amount offset</param>
		/// <returns></returns>
		/// <example>
		/// public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		/// {
		///	position = position.OffsetPosition(velocity, 75);
		/// }
		/// </example>
		public static Vector2 OffsetPosition(this Vector2 position, Vector2 velocity, float offSetBy)
		{
			Vector2 OFFSET = velocity.SafeNormalize(Vector2.UnitX) * offSetBy;
			if (Collision.CanHitLine(position, 0, 0, position + OFFSET, 0, 0))
			{
				return position += OFFSET;
			}

			return position;
		}
	}
}