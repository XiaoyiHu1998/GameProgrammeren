using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class PlayerProjectile : SpriteGameObject
{
    bool moveLeft;
    int timeToLive;
    bool alive;
        
    public PlayerProjectile(Vector2 position, bool moveLeftDirection)
    : base("Sprites/spr_bomb", 2, "PlayerProjectile")
    {
        Spawn(position);
        moveLeft = moveLeftDirection;
        timeToLive = 50;
    }

    public override void Reset()
    {
        visible = false;
    }

    public void Spawn(Vector2 spawnPosition)
    {
        position = spawnPosition;
        visible = true;
        alive = true;
    }


    public override void Update(GameTime gameTime)
    {
        if (alive)
        {
            GameObjectList enemies = GameWorld.Find("enemies") as GameObjectList;
            List<GameObject> enemyList = enemies.Children;

            if (enemies != null)
            {
                foreach (Enemy enemy in enemyList)
                {
                    if (CollidesWith(enemy))
                    {
                        enemy.Die();
                        timeToLive = 0;
                    }
                }
            }

            if (moveLeft)
            {
                position.X -= 6.0f;
            }
            else
            {
                position.X += 6.0f;
            }

            timeToLive -= 1;

            if(timeToLive <= 0)
            {
                alive = false;
                visible = false;
            }
        }
    }
}

