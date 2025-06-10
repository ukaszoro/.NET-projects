using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Platformer2D;

public class EntityManager
{
    public readonly List<IGameEntity> _entities = new List<IGameEntity>();

    public EntityManager()
    {
    }

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

    public void Update(GameTime gameTime, Level Level)
    {
        foreach (IGameEntity entity in _entities.OrderBy(e => e.UpdateOrder))
            entity.Update(gameTime, this);
        foreach (IGameEntity entity in _entities.OrderBy(e => e.UpdateOrder))
        {
            if (entity.remove == true)
            {
                if (entity.Type != "coin" && entity.Type != "fireball")
                    Level.Score += 100;
                RemoveEntity(entity);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime, ref Vector2 Camera2D)
    {
        foreach (IGameEntity entity in _entities.OrderBy(e => e.DrawOrder))
            entity.Draw(spriteBatch, gameTime, ref Camera2D);
    }
}