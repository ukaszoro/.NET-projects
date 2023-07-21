using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;

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
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D);
}

public class EntityManager
{
    public readonly List<IGameEntity> _entities = new List<IGameEntity>();
    public EntityManager() { }

    public bool AddEntity(IGameEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
        if (HasEntity(entity))
            return false;
        _entities.Add(entity);
        return true;
    }

    public bool RemoveEntity(IGameEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));
        if (HasEntity(entity))
            return false;
        _entities.Remove(entity);
        return true;
    }
    public bool HasEntity(IGameEntity entity) => _entities.Contains(entity);

    public void Update(GameTime gameTime)
    {
        foreach (IGameEntity entity in _entities.OrderBy(e => e.UpdateOrder))
            entity.Update(gameTime);
    }
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {

        foreach (IGameEntity entity in _entities.OrderBy(e => e.DrawOrder))
            entity.Draw(spriteBatch, gameTime, ref Camera2D);
    }
}
public class CollisionManager
{
    private readonly List<IGameEntity> _entities = new List<IGameEntity>();
    Tile[,] Tiles;
    public CollisionManager(EntityManager e_manager, Tile[,] tiles)
    {
        _entities = e_manager._entities;
        Tiles = tiles;
    }
    public void check_collision()
    {
        foreach (IGameEntity entity1 in _entities.OrderBy(e => e.UpdateOrder))
        {
            Rectangle entity1_rect = new((int)entity1.Pos_X, (int)entity1.Pos_Y, entity1.Texture.Width, entity1.Texture.Height);
            entity1.Collision[0] = false; // Collision[0] - collision with the ground
            entity1.Collision[1] = false;
            entity1.Collision[2] = false;
            entity1.Collision[3] = false;
            for (int y = 0; y < Tiles.GetLength(1); y++)
            {
                for (int x = 0; x < Tiles.GetLength(0); x++)
                {
                    if (Tiles[x, y] == null)
                        continue;

                    Rectangle Tile = new(x * 40, y * 40, 40, 40);
                    Vector2 depth = RectangleExtensions.GetIntersectionDepth(entity1_rect, Tile);
                    if (Tiles[x, y]._collision == TileCollision.Impassable)
                    {
                        if (depth == Vector2.Zero)
                            continue;
                        Vector2 absDepth = new(Math.Abs(depth.X), Math.Abs(depth.Y));
                        if (absDepth.Y < absDepth.X && !entity1.Collision[0])
                        {
                            if (entity1_rect.Top > Tile.Top)
                            {
                                entity1.Collision[3] = true;
                                entity1.Pos_Y = Tile.Bottom;
                            }
                            else if (depth.Y < 0)
                            {
                                entity1.Collision[0] = true;
                                entity1.Pos_Y = Tile.Top - entity1.Texture.Height + 1;
                            }
                        }

                        else if (absDepth.Y > absDepth.X && !(entity1.Collision[1] || entity1.Collision[2]))
                        {
                            // entity1.Pos_X += depth.X;
                            if (entity1.Type == "player")
                                entity1.walk_speed = 0;
                                
                            if (depth.X < 0) // Right wall collision
                            {
                                entity1.Collision[1] = true;
                                entity1.Pos_X = Tile.Left - entity1.Texture.Width+ 0.2f; // weird value, but without it player vibrates when walking into a wall?
                            }
                            if (depth.X > 0) // Left wall collision
                            {
                                entity1.Collision[2] = true;
                                entity1.Pos_X = Tile.Right;
                            }
                        }
                    }
                }
            }
        }
    }
}
