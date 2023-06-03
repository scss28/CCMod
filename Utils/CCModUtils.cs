using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

#nullable enable
namespace CCMod.Utils
{
	public static partial class CCModUtils
	{
		public static Vector2 Bezier(float ammount, Vector2 a, Vector2 b, Vector2 c)
		{
			Vector2 q1 = Vector2.Lerp(a, b, ammount);
			Vector2 q2 = Vector2.Lerp(b, c, ammount);
			return Vector2.Lerp(q1, q2, ammount);
		}

		public static Vector2 Bezier(float ammount, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
		{
			Vector2 p1 = Vector2.Lerp(a, b, ammount);
			Vector2 p2 = Vector2.Lerp(b, c, ammount);
			Vector2 p3 = Vector2.Lerp(c, d, ammount);

			Vector2 q1 = Vector2.Lerp(p1, p2, ammount);
			Vector2 q2 = Vector2.Lerp(p2, p3, ammount);
			return Vector2.Lerp(q1, q2, ammount);
		}

		#region Normalization and Finite Checks

		/// <summary>Normalizes the given <paramref name="vector"/> with fast inverse square root.</summary>
		/// <param name="vector">The vector to normalize</param>
		/// <returns>The normalized <paramref name="vector"/>.</returns>
		/// <remarks>This method modifies the instance that called this method, unlike <see cref="FastNormalized(Vector2)"/>.</remarks>
		public static Vector2 FastNormalize(this ref Vector2 vector)
		{
			vector *= MathF.ReciprocalSqrtEstimate(vector.LengthSquared());
			return vector;
		}

		/// <summary>Normalizes the given <paramref name="vector"/> with fast inverse square root.</summary>
		/// <param name="vector">The vector to normalize</param>
		/// <returns>The normalized <paramref name="vector"/>.</returns>
		/// <remarks>This method <strong>doesn't</strong> modify the instance that called this method, unlike <see cref="FastNormalize(ref Vector2)"/>.</remarks>
		public static Vector2 FastNormalized(this Vector2 vector)
		{
			return vector * MathF.ReciprocalSqrtEstimate(vector.LengthSquared());
		}

		/// <summary>Returns the given <paramref name="vector"/> normalized (with length of 1).</summary>
		/// <param name="vector">The vector to normalize</param>
		/// <returns>The vector normalized.</returns>
		public static Vector2 Normalized(this Vector2 vector)
		{
			vector.Normalize();
			return vector;
		}

		/// <summary>Attempts to normalize the given <paramref name="vector"/>, if the normalized result has non finite components it returns the given <paramref name="fallback"/>. </summary>
		/// <param name="vector"></param>
		/// <param name="fallback"></param>
		/// <returns>The normalized <paramref name="vector"/>, or <paramref name="fallback"/> if one of the normalized vector components is not finite.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 NormalizedOr(this Vector2 vector, Vector2 fallback)
		{
			vector.Normalize();
			return !(float.IsFinite(vector.X) || float.IsFinite(vector.Y)) ? fallback : vector;
		}

		/// <summary>Attempts to normalize the given <paramref name="vector"/>, if the normalized result has non finite components it returns <see cref="Vector2.Zero"/> </summary>
		/// <param name="vector"></param>
		/// <param name="fallback"></param>
		/// <returns>The normalized <paramref name="vector"/>, or <see cref="Vector2.Zero"/> if one of the normalized vector components is not finite.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 NormalizedOrZero(this Vector2 vector)
		{
			return NormalizedOr(vector, Vector2.Zero);
		}

		/// <summary>Attempts to normalize the given <paramref name="vector"/>, if the normalized result has non finite components it returns the given <paramref name="fallback"/>. </summary>
		/// <param name="vector"></param>
		/// <param name="fallback"></param>
		/// <returns>The normalized <paramref name="vector"/>, or <paramref name="fallback"/> if one of the normalized vector components is not finite.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 NormalizedOrOne(this Vector2 vector)
		{
			return NormalizedOr(vector, Vector2.One);
		}

		/// <summary>Checks if the given <paramref name="vector"/> has non finite components (NaN, Infinity, ...).</summary>
		/// <param name="vector">The vector.</param>
		/// <returns><see langword="true"/> if any component of <paramref name="vector"/> is not finite, <see langword="false"/> otherwise.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasNonFinite(this Vector2 vector)
		{
			return !float.IsFinite(vector.X) || !float.IsFinite(vector.Y);
		}

		/// <summary>
		/// If the given vector has non finite components, it returns the fallback.
		/// </summary>
		/// <param name="vector">The vector.</param>
		/// <param name="fallback">The fallback.</param>
		/// <returns>The given <paramref name="vector"/>, or <paramref name="fallback"/> if <paramref name="vector"/> has a non finite component.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 FiniteOr(this Vector2 vector, Vector2 fallback)
		{
			return float.IsFinite(vector.X) && float.IsFinite(vector.Y) ? vector : fallback;
		}

		/// <summary>If the given vector has a non finite component it returns <see cref="Vector2.Zero"/>.</summary>
		/// <param name="vector">The vector.</param>
		/// <returns>The given <paramref name="vector"/>, or <see cref="Vector2.Zero"/> if any component is not finite.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 FiniteOrZero(this Vector2 vector)
		{
			return float.IsFinite(vector.X) && float.IsFinite(vector.Y) ? vector : Vector2.Zero;
		}

		/// <summary>If the given vector has non finite components it returns <see cref="Vector2.One"/></summary>
		/// <param name="vector">The vector.</param>
		/// <returns>The given <paramref name="vector"/>, or <see cref="Vector2.One"/> if any component is not finite.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 FiniteOrOne(this Vector2 vector)
		{
			return float.IsFinite(vector.X) && float.IsFinite(vector.Y) ? vector : Vector2.One;
		}

		#endregion // Normalization and finite checks

		public static Vector2[] GenerateCircularPositions(this Vector2 center, float radius, int amount = 8, float rotation = 0)
		{
			if (amount <= 0)
				return Array.Empty<Vector2>();

			Vector2[] postitions = new Vector2[amount];

			GenerateCircularPositions(center, postitions.AsSpan(), radius, rotation);

			return postitions;
		}

		public static Span<Vector2> GenerateCircularPositions(Vector2 center, Span<Vector2> postitions, float radius, float rotation)
		{
			if (postitions.IsEmpty)
				return Span<Vector2>.Empty;
			float angle = MathHelper.Pi * 2f / postitions.Length;
			angle += rotation;

			for (int i = 0; i < postitions.Length; i++)
			{
				Vector2 position = (angle * i).ToRotationVector2() * radius;
				position += center;

				postitions[i] = position;
			}

			return postitions;

		}

		//lowqualitytrash-xinim blizzard vector2 code
		/// <summary>
		/// use with combination of <see cref="IsVelocityLimitReached(Vector2, float)"/>
		/// Not recommend to use as it is very broken
		/// </summary>
		/// <param name="velocity"></param>
		/// <param name="limited"></param>
		/// <returns></returns>
		public static Vector2 LimitingVelocity(this Vector2 velocity, float limited)
		{
			velocity.X = Math.Clamp(velocity.X, -limited, limited);
			velocity.Y = Math.Clamp(velocity.Y, -limited, limited);
			return velocity;
		}
		/// <summary>
		/// Check if the velocity of object reach a certain limited 
		/// </summary>
		/// <param name="velocity"></param>
		/// <param name="limited"></param>
		/// <returns></returns>
		public static bool IsVelocityLimitReached(this Vector2 velocity, float limited)
		{
			if (Math.Abs(Math.Clamp(velocity.X, -limited, limited)) >= limited)
				return true;
			if (Math.Abs(Math.Clamp(velocity.Y, -limited, limited)) >= limited)
				return true;
			return false;
		}
		public static float InExpo(float t) => (float)Math.Pow(2, 5 * (t - 1));
		public static float OutExpo(float t) => 1 - InExpo(1 - t);
		public static float InOutExpo(float t)
		{
			if (t < 0.5) return InExpo(t * 2) * .5f;
			return 1 - InExpo((1 - t) * 2) * .5f;
		}
		/// <summary>
		/// Use to order 2 values from smallest to biggest
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns></returns>
		public static (int, int) Order(float v1, float v2) => v1 < v2 ? ((int)v1, (int)v2) : ((int)v2, (int)v1);
		/// <summary>
		/// Use to order 2 values from smallest to biggest
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns></returns>
		public static (float, float) OrderFloat(float v1, float v2) => v1 < v2 ? (v1, v2) : (v2, v1);
	}
}