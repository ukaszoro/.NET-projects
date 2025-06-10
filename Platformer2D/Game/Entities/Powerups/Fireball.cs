using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D;

public class Fireball : IGameEntity
{
    public string Type { get; set; }
    public bool[] Collision { get; }
    public int DrawOrder { get; }
    public int UpdateOrder { get; }
    public float Pos_X { get; set; }
    public float Pos_Y { get; set; }
    public float walk_speed { get; set; }
    public Texture2D Texture { get; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool Hurt { get; set; }
    public bool Hurt_up { get; set; }
    public float jump_speed { get; set; }
    public float gravity_accel { get; set; }
    public bool remove { get; set; }
    public Health Health { get; set; }

    float max_fall;
    float _world_gravity;
    Rectangle[] Sourcerect;
    float Anim_timer;
    int Anim_current;
    float Explosion_timer;
    float Time_alive;

    public Fireball(Vector2 Pos, Texture2D texture, int rotation)
    {
        Type = "fireball";
        Collision = new bool[4];

        Height = 40;
        Width = 40;

        Texture = texture;
        Sourcerect = new Rectangle[4];
        Sourcerect[0] = new(0, 0, 20, 20);
        Sourcerect[1] = new(20, 0, 20, 20);
        Sourcerect[2] = new(40, 0, 20, 20);
        Sourcerect[3] = new(60, 0, 20, 20);

        Pos_X = Pos.X;
        Pos_Y = Pos.Y;
        max_fall = 400;
        _world_gravity = 6;
        Explosion_timer = 1.2f;
        if (rotation == 1)
        {
            walk_speed = 200;
            Pos_X += 20;
        }
        else if (rotation == 0)
        {
            walk_speed = -200;
            Pos_X -= 20;
        }
    }

    public void Update(GameTime gameTime, EntityManager e_manager)
    {
        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Time_alive += Elapsed_time;
        if (Time_alive > 3)
            Hurt = true;
        Pos_X += walk_speed * Elapsed_time;
        if (Hurt == true || Hurt_up == true)
        {
            Type = "ded";
            walk_speed = 0;
            jump_speed = 0;
            Sourcerect[0] = new(80, 0, 40, 40);
            Sourcerect[1] = new(120, 0, 40, 40);
            Sourcerect[2] = new(160, 0, 40, 40);
            Sourcerect[3] = new(160, 0, 40, 40);

            Explosion_timer -= Elapsed_time;
        }

        if (Explosion_timer < 0)
            remove = true;
        if (Anim_timer > 100)
        {
            Anim_timer = 0;
            Anim_current = (Anim_current + 1) % 4;
        }
        else
            Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        if (!Collision[0])
        {
            gravity_accel += (0.05f * gravity_accel) + _world_gravity;
            if (gravity_accel > max_fall * 1.86f)
                gravity_accel = max_fall * 1.86f;
        }

        if (Collision[0])
        {
            jump_speed = 300;
            gravity_accel = 0;
        }

        if (Collision[3])
        {
            gravity_accel = 0;
            jump_speed = 0;
        }

        Pos_Y += gravity_accel * Elapsed_time;

        if (Collision[1] || Collision[2])
        {
            walk_speed = -walk_speed;
        }

        Pos_X += walk_speed * Elapsed_time;

        if (!Collision[0])
        {
            gravity_accel += (0.05f * gravity_accel) + _world_gravity;
            if (gravity_accel > max_fall * 1.86f)
                gravity_accel = max_fall * 1.86f;
        }

        Pos_Y -= (jump_speed - gravity_accel) * Elapsed_time;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        spriteBatch.Draw(
            Texture,
            new Vector2(Pos_X, Pos_Y),
            Sourcerect[Anim_current],
            Color.White,
            0f,
            Camera2D + new Vector2(0, -10),
            new Vector2(1, 1),
            SpriteEffects.None,
            0f
        );
    }
}