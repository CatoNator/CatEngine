using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CatEd
{
    class CLevelView : MonoGame.Forms.Controls.MonoGameControl
    {
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw()
        {
            base.Draw();

            GraphicsDevice.Clear(Color.Black);

            Editor.spriteBatch.Begin();

            Editor.spriteBatch.End();
        }
    }
}
