﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using CatEngine.Content;

namespace CatEngine
{
    public class CWall : CGameObject
    {
        private int iXScale = 1;
        private int iYScale = 1;

        public override void InstanceSpawn()
        {
            rCollisionRectangle = new Rectangle(0, 0, 16*iXScale, 16*iYScale);
        }

        public void SetScale(int iXScale, int iYScale)
        {
            this.iXScale = iXScale;
            this.iYScale = iYScale;

            rCollisionRectangle = new Rectangle(0, 0, 16*iXScale, 16*iYScale);
        }

        public override void Render(string technique)
        {
            UpdateCollision();

            //CRender.Instance.DrawModel("board", new Vector3(x, z, y), 0.0f);
        }

        public override void Render2D()
        {
            CSprite.Instance.DrawRect(rCollisionRectangle, Color.Gray);
        }
    }
}
