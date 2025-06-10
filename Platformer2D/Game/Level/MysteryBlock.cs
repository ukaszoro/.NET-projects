using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D;

public class MysteryBlock : ITile
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

    public MysteryBlock(Texture2D texture, TileCollision collision, ContentManager Content, Vector2 pos, int contains)
    {
        _texture = texture;
        _collision = collision;
        Anim_current = 0;
        Contains = contains;
        Size = new(Tile.Width, Tile.Height);
        Pos = pos;

        Sourcerect = new Rectangle[4];
        Sourcerect[0] = new(0, 0, 40, 40);
        Sourcerect[1] = new(40, 0, 40, 40);
        Sourcerect[2] = new(80, 0, 40, 40);
        Sourcerect[3] = new(120, 0, 40, 40);

        Sound = new SoundEffect[2];
        Sound[0] = Content.Load<SoundEffect>("block_bump");
        Sound[1] = Content.Load<SoundEffect>("coin_sound");
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 Camera2D, GameTime gameTime)
    {
        spriteBatch.Draw(
            _texture,
            Pos * Size,
            Sourcerect[Anim_current],
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
        if (_collision == TileCollision.Mysteryblock && Contains == 0)
            _collision = TileCollision.Mysteryblock_empty;

        if (_collision == TileCollision.Mysteryblock)
        {
            if (Anim_timer >= 400 / (Anim_current + 1))
            {
                Anim_current = (Anim_current + 1) % 3;
                Anim_timer = 0;
            }
            else
                Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        else
            Anim_current = 3;

        if (Hit)
        {
            if (Anim_X < 6 && _collision == TileCollision.Mysteryblock && Contains > 0)
                Sound[1].Play();
            if (_collision == TileCollision.Mysteryblock)
            {
                Anim_current = 3;
                if (Contains > 0)
                {
                    Contains--;
                }

                if (Contains < 0)
                {
                    Contains++;
                }
            }

            if (Anim_timer >= 6)
            {
                Hit_anim.Y = (0.01f * -Anim_X * Anim_X + 1.2f * Anim_X);
                Anim_X += 9;
                Anim_timer = 0;
            }
            else
                Anim_timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Anim_X > 120)
            {
                Hit = false;
                Hit_anim.Y = 0;
                Anim_timer = 0;
                Anim_X = 0;
            }
        }
    }
}