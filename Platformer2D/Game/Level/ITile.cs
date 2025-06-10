using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D;

public interface ITile
{
    public Texture2D _texture { get; set; }
    public TileCollision _collision { get; set; }
    public static int Width = 40;
    public static int Height = 40;
    public static Vector2 TSize = new(Width, Height);

    public Vector2 Size { get; set; }
    public Vector2 Pos { get; set; }
    public bool Hit { get; set; }
    public bool Break { get; set; }
    public Rectangle[] Sourcerect { get; }
    public int Contains { get; set; }


    public void Draw(SpriteBatch spriteBatch, Vector2 Camera2D, GameTime gameTime);
    public void Update(GameTime gameTime);
}