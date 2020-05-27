using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatEngine.Content;
using Microsoft.Xna.Framework;

namespace CatEngine.SkeletalAnimation
{
    public class Bone
    {
        String boneName = "";

        Vector3 Position;
        Vector2 Size;
        Vector2 Origin;
        float[] Rotations;
        int[] Images;
        int currentImage;
        string Texture = "empty";

        Vector3 additionalPosition = new Vector3(0, 0, 0);
        float additionalRotation;

        Bone parentBone = null;
        List<Bone> childBones = new List<Bone>();

        private Matrix transformMatrix;

        public Bone(string name, Bone parent, Vector3 pos, string tex, Vector2 size, Vector2 orig)
        {
            boneName = name;
            Position = pos;

            Size = size;
            Origin = orig;

            Texture = tex;

            parentBone = parent;
        }

        public void SetRotations(float[] rot)
        {
            Rotations = rot;
        }

        public void SetImages(int[] img)
        {
            Images = img;
        }

        public string GetName()
        {
            return boneName;
        }

        public string ParentName()
        {
            string name = "";

            if (parentBone != null)
                name = parentBone.GetName();

            return name;
        }

        public void AddChild(Bone bone)
        {
            childBones.Add(bone);
        }

        public Bone GetChild(string name)
        {
            Bone childBone = null;
            
            if (childBones.Count > 0)
            {
                foreach (Bone b in childBones)
                {
                    if (childBone == null)
                    {
                        if (b.GetName() == name)
                        {
                            childBone = b;
                            Console.WriteLine("found child");
                        }
                        else
                        {
                            Console.WriteLine("child was not the one; " + boneName);
                            childBone = b.GetChild(name);
                        }
                    }
                }
            }

            return childBone;
        }

        private float Lerp(float a, float b, float amount)
        {
            return a * (1 - amount) + b * amount;
        }

        public void UpdateBone(float frame, int maxFrames)
        {
            //Console.WriteLine("updating bone " + boneName);

            Matrix positionMatrix = Matrix.CreateTranslation(Position + additionalPosition);

            float rot = 0;

            if (Rotations != null)
            {
                if (frame >= Rotations.Length)
                    rot = 0f;
                else
                {
                    int currentFrame = (int)frame;
                    int nextFrame = (int)(frame + 1) % maxFrames;
                    float lerpAmount = frame % 1;

                    rot = Lerp(Rotations[currentFrame], Rotations[nextFrame], lerpAmount);
                }
            }
            else
                rot = 0f;

            Matrix rotationMatrix = Matrix.CreateRotationY(rot+additionalRotation);

            if (Images != null)
                currentImage = Images[(int)frame];
            else
                currentImage = 0;

            Matrix localTransformMatrix = rotationMatrix * positionMatrix;

            if (parentBone != null)
                transformMatrix = localTransformMatrix * parentBone.GetTransformMatrix();
            else
                transformMatrix = localTransformMatrix;

            if (childBones.Count > 0)
            {
                foreach (Bone b in childBones)
                {
                    b.UpdateBone(frame, maxFrames);
                }
            }
        }

        public void AddRotation(float additionalRot)
        {
            additionalRotation = additionalRot;
        }

        public void AddPosition(Vector3 addPos)
        {
            additionalPosition = addPos;
        }

        public void RenderBone(float scale)
        {
            transformMatrix *= Matrix.CreateScale(scale);

            CRender.Instance.RenderBone("BasicColorDrawing", Texture, currentImage, transformMatrix, Size, Origin);

            if (childBones.Count > 0)
            {
                foreach (Bone b in childBones)
                {
                    b.RenderBone(scale);
                }
            }
        }

        public Matrix GetTransformMatrix()
        {
            return transformMatrix;
        }
    }
}
