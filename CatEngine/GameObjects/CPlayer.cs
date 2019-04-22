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
        public float fAimDirection = 0.0f;

        private int iSpeed = 0;
        
        public override void InstanceSpawn()
        {
            vCollisionOrigin = new Vector2(16, 4);
            rCollisionRectangle = new Rectangle(0, 0, 48, 8);
        }
        
        public override void Update()
        {
            UpdateCollision();

            //floor collision
            CGameObject collision = CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y + rCollisionRectangle.Height, rCollisionRectangle.Width, 1), typeof(CWall), true);

            //input
            KeyboardState keyboardState = Keyboard.GetState();

            MovementKeyboard(keyboardState);

            fHorSpeed = (float)iSpeed;

            //note! current collision model only supports recantular collisions, no pixel perfect shapes
            //collision always gets stuck, needs adjusting

            if (x < 0)
            {
                x = 1;
                fHorSpeed = 0;
            }

            else if (x > 416)
            {
                x = 415;
                fHorSpeed = 0;
            }

            x += fHorSpeed;
        }

        public override void Render()
        {
            CSprite.Instance.DrawRect(rCollisionRectangle, Color.Red);

            //CSprite.Instance.Render("sprTest", x, y, 0, false, 0, 1.0f, Color.White);
        }

        public void MovementKeyboard(KeyboardState keyboardState)
        {
            //rename this var
            int maxSpeed = 2;
            
            //moving forward/backward
            if (keyboardState.IsKeyDown(CSettings.Instance.kPMoveRight))
                iSpeed = maxSpeed;
            else if (keyboardState.IsKeyDown(CSettings.Instance.kPMoveLeft))
                iSpeed = -maxSpeed;
            else
                iSpeed = 0;
        }
    }
}
