using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D;

public class Coin : IGameEntity
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

    Rectangle[] Sourcerect;
    int Anim_current;
    float Anim_timer;
    SoundEffect Sound;

    public Coin(int x, int y, Texture2D texture, SoundEffect sound)
    {
        Collision = new bool[4];
        Type = "coin";
        Height = 40;
        Width = 40;

        Texture = texture;
        Sound = sound;

        Pos_X = x * Width;
        Pos_Y = y * Height;

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
        float Elapsed_time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if (remove == true)
        {
            Sound.Play();
        }

        if (Anim_timer > 120)
        {
            Anim_timer = 0;
            Anim_current = (Anim_current + 1) % 3;
        }
        else
        {
            Anim_timer += Elapsed_time;
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        spriteBatch.Draw(
            Texture,
            new Vector2(Pos_X, Pos_Y),
            Sourcerect[Anim_current],
            Color.White,
            0f,
            Camera2D,
            new Vector2(1, 1),
            SpriteEffects.None,
            0f
        );
    }
}