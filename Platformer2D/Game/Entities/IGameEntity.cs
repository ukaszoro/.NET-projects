using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer2D;

public interface IGameEntity
{
    string Type { get; }
    bool[] Collision { get; }
    int DrawOrder { get; }

    int UpdateOrder { get; }

    //Vector2 Pos { get; set; }
    float Pos_X { get; set; }
    float Pos_Y { get; set; }
    float walk_speed { get; set; }
    Texture2D Texture { get; }
    int Height { get; set; }
    int Width { get; set; }
    bool Hurt { get; set; }
    bool Hurt_up { get; set; }
    float jump_speed { get; set; }
    float gravity_accel { get; set; }
    bool remove { get; set; }
    Health Health { get; set; }
    void Update(GameTime gameTime, EntityManager e_manager);
    void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D);
}