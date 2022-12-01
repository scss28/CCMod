using CCMod.Common;
using Terraria.ModLoader;

namespace CCMod
{
	public class CCMod : Mod
	{
        public static Mod Instance { get; private set; }
        public CCMod()
        {
            Instance = this;
        }

        public override void Load()
        {
            SoundManager.Load(Instance);
        }

        public override void Unload()
        {
            Instance = null;
        }
    }
}