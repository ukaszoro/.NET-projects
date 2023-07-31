using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace Pong_lan;
public class IGameEntity
{
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
    private readonly List<IGameEntity> _entities = new();
    public CollisionManager(EntityManager e_manager)
    {
        _entities = e_manager._entities;
    }
    public void check_collision()
    {
        foreach(IGameEntity entity in _entities)
        {
            
        }
    }
}