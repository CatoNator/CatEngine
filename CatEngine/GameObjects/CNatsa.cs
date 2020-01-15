using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatEngine.Content;
using Microsoft.Xna.Framework;

namespace CatEngine
{
    class CNatsa : CGameObject
    {
        public float rotation = 0.0f;

        public override void Render()
        {
            //UpdateCollision();

            rotation += 0.042f;

            rotation %= (float)Math.PI * 2.0f;

            CRender.Instance.DrawSimpleModel("natsa", new Vector3(x, z + 2.5f*(float)Math.Sin((double)rotation), y), new Vector3(-(float)(Math.PI/2.0), 0, rotation), 1.0f);
        }
    }
}
