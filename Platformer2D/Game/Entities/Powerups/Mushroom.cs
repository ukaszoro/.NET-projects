using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D;

public class Mushroom : IGameEntity
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

    float Spawn_timer;
    Vector2 Spawn_anim;
    float max_fall;
    float _world_gravity;
    SoundEffect[] Sound;

    public Mushroom(Vector2 Pos, Texture2D texture, SoundEffect sound_appear, SoundEffect sound_take)
    {
        Type = "ded";
        Collision = new bool[4];
        walk_speed = 0;
        Height = 40;
        Width = 40;
        Spawn_anim.Y = -Height;

        Texture = texture;
        Sound = new SoundEffect[2];
        Sound[0] = sound_appear;
        Sound[1] = sound_take;
        Pos_X = Pos.X;
        Pos_Y = Pos.Y - Height;
        Spawn_timer = 1000;
        max_fall = 400;
        _world_gravity = 10;
    }

    public void Update(GameTime gameTime, EntityManager e_manager)
    {
        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Pos_X += walk_speed * Elapsed_time;

        if (Spawn_timer == 1000)
            Sound[0].Play();
        if (remove == true)
            Sound[1].Play();
        if (!Collision[0] && Spawn_timer < 0)
        {
            gravity_accel += (0.05f * gravity_accel) + _world_gravity;
            if (gravity_accel > max_fall * 1.86f)
                gravity_accel = max_fall * 1.86f;
        }

        if (Collision[0] || Collision[3])
        {
            gravity_accel = 0;
        }

        Pos_Y += gravity_accel * Elapsed_time;

        if (Spawn_timer > 0 && Spawn_anim.Y < 0)
        {
            Spawn_anim.Y += 2.5f;
        }
        else
        {
            Spawn_timer = 0;
            if (Type == "ded")
                walk_speed = 60;
            Type = "mushroom";
        }

        Spawn_timer -= Elapsed_time;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        spriteBatch.Draw(
            Texture,
            new Vector2(Pos_X, Pos_Y),
            null,
            Color.White,
            0f,
            Camera2D + Spawn_anim,
            new Vector2(1, 1),
            SpriteEffects.None,
            0f
        );
    }
}