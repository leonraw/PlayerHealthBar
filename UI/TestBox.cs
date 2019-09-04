using Terraria;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using PlayerHealthBar.UI;

namespace PlayerHealthBar.UI
{
    public class TestBox : UIState
    {

        public UIText text = new UIText("nothing..");
        public bool visible = false;
        public override void OnInitialize()
        {
            UIPanel panel = new UIPanel();
            panel.Width.Set(300f, 0f);
            panel.Height.Set(300f, 0f);
            panel.VAlign = 0.3f;
            panel.HAlign = 0.3f;
            Append(panel);

            UIText header = new UIText("Test Box");
            header.HAlign = 0.5f;
            header.Top.Set(15, 0);
            panel.Append(header);

            text.HAlign = text.VAlign = 0.5f;
            panel.Append(text);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];

            text.SetText(
                "player position: (" + player.position.X + ", " + player.position.Y.ToString() + ")" + "\n" +
                "screen position: (" + Main.screenPosition.X + ", " + Main.screenPosition.Y + ")" + "\n" +
                "player size: " + player.Size + "\n" +
                "player statLifeMax: " + player.statLifeMax
                );

            Recalculate();
            base.Draw(spriteBatch);
        }
    }
}