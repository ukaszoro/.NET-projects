using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D;

public class Tile : ITile
{
    public Texture2D _texture { get; set; }
    public TileCollision _collision { get; set; }
    public const int Width = 40;
    public const int Height = 40;

    public Vector2 Size { get; set; }
    public Vector2 Pos { get; set; }
    public bool Hit { get; set; }
    public bool Break { get; set; }
    public Rectangle[] Sourcerect { get; }
    public int Contains { get; set; }

    public Tile(Texture2D texture, TileCollision collision, Vector2 pos)
    {
        Pos = pos;
        Size = ITile.TSize;
        _texture = texture;
        _collision = collision;
        Sourcerect = new Rectangle[4];
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 Camera2D, GameTime gameTime)
    {
        spriteBatch.Draw(
            _texture,
            Pos * Size,
            null,
            Color.White,
            0f,
            Camera2D,
            Vector2.One,
            SpriteEffects.None,
            0f
        );
    }

    public void Update(GameTime gameTime)
    {
    }
}