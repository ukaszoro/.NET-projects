using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D;

public class Brick : ITile
{
    public Texture2D _texture { get; set; }
    public TileCollision _collision { get; set; }
    public Vector2 Size { get; set; }
    public Vector2 Pos { get; set; }
    public bool Hit { get; set; }
    public bool Break { get; set; }
    public Rectangle[] Sourcerect { get; }
    public int Contains { get; set; }

    Vector2 Hit_anim = new(0, 0);
    float Anim_timer;
    int Anim_current;
    float Anim_X = 0;
    SoundEffect[] Sound;

    public Brick(Texture2D texture, TileCollision collision, ContentManager Content, Vector2 pos)
    {
        _texture = texture;
        _collision = collision;
        Anim_current = 0;
        Size = new(Tile.Width, Tile.Height);
        Pos = pos;

        Sourcerect = new Rectangle[4];
        Sourcerect[0] = new(0, 0, 20, 20);
        Sourcerect[1] = new(20, 0, 20, 20);
        Sourcerect[2] = new(0, 20, 20, 20);
        Sourcerect[3] = new(20, 20, 20, 20);

        Sound = new SoundEffect[2];
        Sound[0] = Content.Load<SoundEffect>("block_bump");
        Sound[1] = Content.Load<SoundEffect>("block_break");
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 Camera2D, GameTime gameTime)
    {
        if (Break == true)
        {
            if (Anim_X < 5)
                Sound[1].Play();

            for (int i = 0; i < 4; i++)
            {
                if (i < 2)
                    Hit_anim.X = -0.1f * 3 * (i + 1) * Anim_X;
                else
                    Hit_anim.X = 0.1f * 3 * (i - 1) * Anim_X;

                if (Anim_timer >= 7)
                {
                    Hit_anim.Y = (0.01f * -Anim_X * Anim_X + 1.2f * Anim_X);
                    Anim_X += 6;
                    Anim_timer = 0;
                }
                else
                    Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                spriteBatch.Draw(
                    _texture,
                    Pos * Size,
                    Sourcerect[i],
                    Color.White,
                    0f,
                    Camera2D + Hit_anim,
                    Vector2.One,
                    SpriteEffects.None,
                    0f
                );
            }
        }
        else
            spriteBatch.Draw(
                _texture,
                Pos * Size,
                null,
                Color.White,
                0f,
                Camera2D + Hit_anim,
                Vector2.One,
                SpriteEffects.None,
                0f
            );
    }

    public void Update(GameTime gameTime)
    {
        if (Break == true)
        {
            if (Anim_X > 20)
            {
                Hit = false;
                _collision = TileCollision.Passable;
            }
        }

        else if (Hit)
        {
            if (Anim_X < 6)
                Sound[0].Play();

            if (Anim_timer >= 6)
            {
                Hit_anim.Y = (0.01f * -Anim_X * Anim_X + 1.2f * Anim_X);
                Anim_X += 9;
                Anim_timer = 0;
            }
            else
                Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        if (Anim_X > 120 && Break == false)
        {
            Hit = false;
            Hit_anim.Y = 0;
            Anim_timer = 0;
            Anim_X = 0;
            Anim_current = 0;
        }
    }
}