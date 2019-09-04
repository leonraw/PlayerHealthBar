using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using System.Collections.Generic;
using PlayerHealthBar.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PlayerHealthBar
{
    public class PlayerHealthBar : Mod
    {
        public PlayerHealthBar()
        {
        }

        internal UserInterface face;
        internal TestBox tb;
        public override void Load()
        {
            if (!Main.dedServ)
            {
                face = new UserInterface();
                tb = new TestBox();
                // tb.visible = true;
                face.SetState(tb);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (tb.visible)
            {
                face?.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "PlayerHealthBar: Head Resource Bars",
                    delegate
                        {
                            if (tb.visible)
                            {
                                face.Draw(Main.spriteBatch, new GameTime());
                            }
                            DrawInterface_PlayerHealthBars();
                            return true;
                        },
                       InterfaceScaleType.Game));
            }
        }

        private void DrawInterface_PlayerHealthBars()
        {
            Player player = Main.player[Main.myPlayer];

            if (player.statLife == player.statLifeMax)
                return;

            Vector2 position = player.position;
            float positionX = position.X + (float)(player.width / 2);
            float positionY = position.Y + (float)(player.height) + 10f;
            float brightness = Lighting.Brightness(
                (int)(positionX / 16.0),
                (int)((positionY + (double)player.gfxOffY) / 16.0)
                );
            // float scale = 1f;

            this.DrawHealthBar(positionX, positionY, player.statLife, player.statLifeMax, brightness);
        }
        private void DrawHealthBar(float X, float Y, int Health, int MaxHealth, float alpha, float scale = 1f)
        {
            if (Health <= 0)
                return;

            // HP: 生命值百分比。
            float HP = (float)Health / (float)MaxHealth;
            if ((double)HP > 1.0)
                HP = 1f;

            // HP_36: 生命值上限为36时的生命值。
            int HP_36 = (int)(36.0 * (double)HP);

            // HBLeftX: X - 尺寸的18倍。
            float HBLeftX = X - 18f * scale;

            // Y_: Y。
            float Y_ = Y;

            // 如果玩家重力相反。
            if ((double)Main.player[Main.myPlayer].gravDir == -1.0)
            {
                float n1 = Y_ - Main.screenPosition.Y;
                Y_ = Main.screenPosition.Y + (float)Main.screenHeight - n1;
            }

            // blueColor: 0f
            float blueColor = 0.0f;

            // maxValue: 255
            float maxValue = (float)byte.MaxValue;

            // HP_: 生命百分比 - 10%
            float HP_ = HP - 0.1f;

            // greenColor, redColor: null.
            float greenColor;
            float redColor;

            // 如果 生命百分比 大于 60%。
            if ((double)HP_ > 0.5)
            {
                // greenColor: 255.
                greenColor = (float)byte.MaxValue;

                // redColor: 255 * 失去生命百分比+10% * 2
                redColor = (float)((double)byte.MaxValue * (1.0 - (double)HP_) * 2.0);
            }
            else
            {
                // greenColor: 255 * 生命百分比-10% * 2
                greenColor = (float)((double)byte.MaxValue * (double)HP_ * 2.0);

                // redColor: 255.
                redColor = (float)byte.MaxValue;
            }
            // 生命值越大，绿色光越大、红色光越小。

            float n2 = 0.95f;

            // redAlphaColor: redColor * 透明度 * 0.95
            float redAlphaColor = redColor * alpha * n2;

            // greenAlphaColor: greenColor * 透明度 * 0.95
            float greenAlphaColor = greenColor * alpha * n2;

            // alphaColor: 255 * 透明度 * 0.95
            float alphaColor = maxValue * alpha * n2;


            if ((double)redAlphaColor < 0.0)
                redAlphaColor = 0.0f;
            if ((double)redAlphaColor > (double)byte.MaxValue)
                redAlphaColor = (float)byte.MaxValue;
            if ((double)greenAlphaColor < 0.0)
                greenAlphaColor = 0.0f;
            if ((double)greenAlphaColor > (double)byte.MaxValue)
                greenAlphaColor = (float)byte.MaxValue;
            if ((double)alphaColor < 0.0)
                alphaColor = 0.0f;
            if ((double)alphaColor > (double)byte.MaxValue)
                alphaColor = (float)byte.MaxValue;

            Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color((int)(byte)redAlphaColor, (int)(byte)greenAlphaColor, (int)(byte)blueColor, (int)(byte)alphaColor);

            // HP_36: 生命值上限为36时的生命值。
            if (HP_36 < 3)
                HP_36 = 3;
            if (HP_36 < 34)
            {
                // HBLeftX:  X - 尺寸的18倍。

                if (HP_36 < 36)
                    // 绘制窄条的左端。
                    Main.spriteBatch.Draw(Main.hbTexture2, new Vector2((float)((double)HBLeftX - (double)Main.screenPosition.X + (double)HP_36 * (double)scale), Y_ - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(2, 0, 2, Main.hbTexture2.Height)), color, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
                if (HP_36 < 34)
                    // 绘制窄条。
                    Main.spriteBatch.Draw(Main.hbTexture2, new Vector2((float)((double)HBLeftX - (double)Main.screenPosition.X + (double)(HP_36 + 2) * (double)scale), Y_ - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(HP_36 + 2, 0, 36 - HP_36 - 2, Main.hbTexture2.Height)), color, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
                if (HP_36 > 2)
                    // 绘制血条。
                    Main.spriteBatch.Draw(Main.hbTexture1, new Vector2(HBLeftX - Main.screenPosition.X, Y_ - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, HP_36 - 2, Main.hbTexture1.Height)), color, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
                Main.spriteBatch.Draw(Main.hbTexture1, new Vector2((float)((double)HBLeftX - (double)Main.screenPosition.X + (double)(HP_36 - 2) * (double)scale), Y_ - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(32, 0, 2, Main.hbTexture1.Height)), color, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
            }
            else
            {
                if (HP_36 < 36)
                    Main.spriteBatch.Draw(Main.hbTexture2, new Vector2((float)((double)HBLeftX - (double)Main.screenPosition.X + (double)HP_36 * (double)scale), Y_ - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(HP_36, 0, 36 - HP_36, Main.hbTexture2.Height)), color, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
                Main.spriteBatch.Draw(Main.hbTexture1, new Vector2(HBLeftX - Main.screenPosition.X, Y_ - Main.screenPosition.Y), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, HP_36, Main.hbTexture1.Height)), color, 0.0f, new Vector2(0.0f, 0.0f), scale, SpriteEffects.None, 0.0f);
            }
        }
    }
}