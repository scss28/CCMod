using Terraria;
using Microsoft.Xna.Framework;

namespace CCMod.Content.Items.Weapons.Ranged
{
    internal static class XinimLazyUtils
    {
        /// <summary>
        /// This return a offset Vector2, useful to make bullet appear out of muzzle
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity">velocity</param>
        /// <param name="offSetBy">amount offset</param>
        /// <returns></returns>
        public static Vector2 PositionOFFSET(this Vector2 position, Vector2 velocity, float offSetBy)
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
