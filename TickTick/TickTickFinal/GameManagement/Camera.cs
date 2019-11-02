using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TickTick5.GameManagement
{
    public class Camera : GameObject
    {
        private Vector2 cameraPosition;
        private Vector2 worldSize;

        public Camera()
        {
            cameraPosition = new Vector2();
        }

        public Vector2 CameraPosition
        {
            get { return cameraPosition; }
        }

        public Vector2 WorldSize
        {
            set { worldSize = value; }
        }

        public void updatePosition(Vector2 position)
        {
            if (position.X > GameEnvironment.Screen.X / 2)
            {
                if (position.X < worldSize.X - GameEnvironment.Screen.X / 2)
                    cameraPosition.X = position.X - GameEnvironment.Screen.X / 2;
                else
                    cameraPosition.X = worldSize.X - GameEnvironment.Screen.X;
            }
            else
                cameraPosition.X = 0;


            if (position.Y > GameEnvironment.Screen.Y / 2)
            {
                if (position.Y < worldSize.Y - GameEnvironment.Screen.Y / 2)
                    cameraPosition.Y = position.Y - GameEnvironment.Screen.Y / 2;
                else
                    cameraPosition.Y = worldSize.Y - GameEnvironment.Screen.Y;
            }
            else
                cameraPosition.Y = 0;
        }
    }
}
