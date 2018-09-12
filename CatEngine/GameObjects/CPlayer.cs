using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CatEngine
{
    public class CPlayer : CGameObject
    {
        private bool bLanded = false;

        public bool bFlip;
        
        public override void InstanceSpawn()
        {
            vCollisionOrigin = new Vector2(17, 17);
            rCollisionRectangle = new Rectangle(0, 0, 34, 35);
            bFlip = false;
        }
        
        public override void Update()
        {
            UpdateCollision();

            //floor collision
            CGameObject collision = CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y + rCollisionRectangle.Height, rCollisionRectangle.Width, 1), typeof(CWall), true);

            if (collision != null && fVerSpeed > 0)
            {
                bLanded = true;
                y = collision.y - vCollisionOrigin.Y;
                fVerSpeed = 0;
            }
            else
                bLanded = false;

            //input
            KeyboardState keyboardState = Keyboard.GetState();

            int maxSpeed = 2;

            //moving
            if (keyboardState.IsKeyDown(Keys.Left) &&
                CollisionRectangle(new Rectangle(rCollisionRectangle.X - maxSpeed + 1, rCollisionRectangle.Y, maxSpeed + 1, rCollisionRectangle.Height - 3), typeof(CWall), true) == null)
            {
                fHorSpeed = -maxSpeed;
                bFlip = true;
            }  
            else if (keyboardState.IsKeyDown(Keys.Right) &&
                CollisionRectangle(new Rectangle(rCollisionRectangle.X + rCollisionRectangle.Width, rCollisionRectangle.Y, maxSpeed + 1, rCollisionRectangle.Height - 3), typeof(CWall), true) == null)
            {
                fHorSpeed = maxSpeed;
                bFlip = false;
            }
            else
                fHorSpeed = 0;

            //jumping
            if (keyboardState.IsKeyDown(Keys.Up) && bLanded)
                fVerSpeed = -6;

            if (!bLanded)
                fVerSpeed += 0.25f;

            x += fHorSpeed;
            y += fVerSpeed;
        }

        public override void Render()
        {
            //CSprite.Instance.DrawRect(rCollisionRectangle, Color.Green);

            CSprite.Instance.Render("sprTest", x, y, 0, bFlip, 0.0f, 1.0f, Color.White);
        }
    }
}
