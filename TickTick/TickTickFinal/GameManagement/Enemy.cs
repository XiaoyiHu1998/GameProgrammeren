using System;
using Microsoft.Xna.Framework;

class Enemy : AnimatedGameObject
{
    protected bool alive;

    public Enemy(int layer = 0, string id = "")
    :base(layer, id)
    {
        alive = true;
    }

    public void Die()
    {
        alive = false;
        visible = false;
    }

    public override void Reset()
    {
        visible = true;
        alive = true;
    }
}
