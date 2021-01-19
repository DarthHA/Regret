using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace Regret
{
	public class BrokenShaderData : ScreenShaderData
	{
		public BrokenShaderData(string passName)
			: base(passName)
		{
		}
		public BrokenShaderData(Ref<Effect> shader, string passName) : base(shader, passName)
		{

		}

        public override void Update(GameTime gameTime)
        {
            if(Regret.BrokenState >=0 && Regret.BrokenState <= 3)
            {
				UseImage(Regret.Instance.GetTexture("Images/Broken" + Regret.BrokenState.ToString()));
			}
            else
            {
				UseImage(Regret.Instance.GetTexture("Images/Broken0"));
			}
			Vector2 Pos = Regret.BrokenPos - Main.screenPosition - new Vector2(Main.screenWidth, Main.screenHeight) / 2;
			Pos.X /= Main.screenWidth;
			Pos.Y /= Main.screenHeight;
			UseColor(Pos.X, Pos.Y, 0);
		}
        public override void Apply()
		{
			if (Regret.BrokenState >= 0 && Regret.BrokenState <= 3)
			{
				UseImage(Regret.Instance.GetTexture("Images/Broken" + Regret.BrokenState.ToString()));
			}
			else
			{
				UseImage(Regret.Instance.GetTexture("Images/NoBroken"));
			}
			Vector2 Pos = Regret.BrokenPos - Main.screenPosition - new Vector2(Main.screenWidth, Main.screenHeight) / 2;
			Pos.X /= Main.screenWidth;
			Pos.Y /= Main.screenHeight;
			UseColor(Pos.X, Pos.Y, 0);
			base.Apply();
		}
	}
}