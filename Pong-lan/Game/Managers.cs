using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace Pong_lan;
public class IGameEntity
{
    public Texture2D Texture;
    public float Horizontal_speed;
    public float Vertical_speed;
    public Vector2 Pos;
    public GameWindow Window;
    public Rectangle Window_bounds = new(1,1,1,1);    
    public int Hight;
    public int Width;
    public string Type;
    public virtual void Update(GameTime gameTime){}
    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch){}
}

public class EntityManager
{
    public List<IGameEntity> _entities = new();
    public EntityManager() {}

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
        if (!HasEntity(entity))
            return false;
        _entities.Remove(entity);
        return true;
    }
    public bool HasEntity(IGameEntity entity)
    {
        if (_entities.Contains(entity)) return true;
        else return false;
    }
    public void Update(GameTime gameTime)
    {
        foreach (IGameEntity entity in _entities)
            entity.Update(gameTime);
    }
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (IGameEntity entity in _entities)
            entity.Draw(gameTime, spriteBatch);
    }
}
public class CollisionManager
{
    private readonly List<IGameEntity> Entities = new();
    public CollisionManager(List<IGameEntity> entities)
    {
        Entities = entities;
    }
    public void check_collision()
    {
        foreach(IGameEntity entity1 in Entities)
        {
            if (entity1.Type == "Player")
                continue;
            foreach(IGameEntity entity2 in Entities)
            {
                if (entity1 == entity2)
                    continue;
                Rectangle entity1_rect = new((int)entity1.Pos.X, (int)entity1.Pos.Y, entity1.Width, entity1.Hight);
                Rectangle entity2_rect = new((int)entity2.Pos.X, (int)entity2.Pos.Y, entity2.Width, entity2.Hight);
                Vector2 depth = RectangleExtensions.GetIntersectionDepth(entity1_rect, entity2_rect);
                if (depth == Vector2.Zero)
                    continue;
                entity1.Horizontal_speed = -entity1.Horizontal_speed * 1.05f;
                Random rnd = new();
                entity1.Vertical_speed += 0.05f * entity1.Vertical_speed;
            }
        }
    }
}