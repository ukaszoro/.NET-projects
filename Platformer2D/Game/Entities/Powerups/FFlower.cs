using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D;

public class FFlower : IGameEntity
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
    Rectangle[] Sourcerect;
    int Anim_current;
    float Anim_timer;
    SoundEffect[] Sound;

    public FFlower(Vector2 Pos, Texture2D texture, SoundEffect sound_appear, SoundEffect sound_take)
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

        Sourcerect = new Rectangle[4];
        Sourcerect[0] = new(0, 0, 40, 40);
        Sourcerect[1] = new(40, 0, 40, 40);
        Sourcerect[2] = new(80, 0, 40, 40);
        Sourcerect[3] = new(120, 0, 40, 40);
        Anim_current = 0;
        Anim_timer = 0;
    }

    public void Update(GameTime gameTime, EntityManager e_manager)
    {
        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (remove == true)
            Sound[1].Play();
        if (Spawn_timer == 1000)
            Sound[0].Play();
        if (Spawn_timer > 0 && Spawn_anim.Y < 0)
        {
            Spawn_anim.Y += 2.5f;
        }
        else
        {
            Spawn_timer = 0;
            if (Type == "ded")
                walk_speed = 60;
            Type = "fflower";
        }

        Spawn_timer -= Elapsed_time;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        if (Anim_timer > 80)
        {
            Anim_timer = 0;
            Anim_current = (Anim_current + 1) % 4;
        }
        else
            Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

        spriteBatch.Draw(
            Texture,
            new Vector2(Pos_X, Pos_Y),
            Sourcerect[Anim_current],
            Color.White,
            0f,
            Camera2D + Spawn_anim,
            new Vector2(1, 1),
            SpriteEffects.None,
            0f
        );
    }
}