﻿using UnityEngine;

namespace Items
{
	public interface IGun
	{
		GameObject Projectile { get; }
		Vector2 ProjectileOffset { get; }
		float Damage { get; }
		float Velocity { get; }
		float Range { get; }
		float FireRate { get; }
		float Spread { get; }
		float ProjectileRadius { get; }
		bool Automatic { get; }
	}
}