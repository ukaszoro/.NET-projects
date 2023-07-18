using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Platformer2D;

public interface IGameEntity
{
    int DrawOrder { get; }
    int UpdateOrder { get; }
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch, GameTime gameTime);    
}

public class EntityManager
{
    private readonly List<IGameEntity> _entities = new List<IGameEntity>();
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
        if (HasEntity(entity))
            return false;
        _entities.Remove(entity);
        return true;
    }
    public bool HasEntity(IGameEntity entity) => _entities.Contains(entity);

    public void Update(GameTime gameTime)
    {
        foreach(IGameEntity entity in _entities.OrderBy(e => e.UpdateOrder))
            entity.Update(gameTime);
    }
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {

        foreach(IGameEntity entity in _entities.OrderBy(e => e.DrawOrder))
            entity.Draw(spriteBatch, gameTime);
    }
}

