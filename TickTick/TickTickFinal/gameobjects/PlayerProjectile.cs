using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class PlayerProjectile : AnimatedGameObject
{
    protected Vector2 startposition;
    
        
    public PlayerProjectile(Vector2 playerGlobalPosition)
    : base(2, "PlayerProjectile")
    {
        LoadAnimation("Sprites/Player/spr_idle", "idle", true);
        LoadAnimation("Sprites/Player/spr_explode@5x5", "explode", false, 0.04f);


    }

    public override void Reset()
    {
        visible = false;
    }

    public void Spawn(Vector2 spawnPosition)
    {
        position = spawnPosition;
        visible = true;
    }

    public void Update()
    {
        Rocket rocket = GameWorld.Find("Rocket") as Rocket;

        if (CollidesWith(rocket))
        {
            rocket.Reset();
            PlayAnimation("explode");
            Reset();
        }
        else
        {
            PlayAnimation("idle");
            this.Position =  new Vector2(this.Position.X + 2.0f, this.Position.Y);
        }

        //TODO: reset if out of bounds
        if(GlobalPosition.X > 1)
        {

        }

    }
}

