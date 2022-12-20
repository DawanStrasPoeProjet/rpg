using Microsoft.EntityFrameworkCore;
using RpgAppDatabase.Context;
using RpgAppDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgAppDatabase.Dao
{
    internal class EntityDAO : IEntityDAO
    {
        private RpgContext context;
                
        public EntityDAO(RpgContext context)
        {
            this.context = context;
        }

        public async Task SaveOrUpdateEntity(Entity entity)
        {
            if (entity.EntityId == 0)  
            {
                context.Entities.Add(entity);  
            }
            else   
            {
                context.Entry(entity).State = EntityState.Modified;
            }

            await context.SaveChangesAsync(); 
        }

        public async Task DeleteEntity(Entity entity)
        {
            context.Entry(entity).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        public async Task<Entity> FindEntityById(int id)
        {            
            return await context.Entities.FindAsync(id);
        }

        public async Task<List<Entity>> GetEntities()
        {
            return await context.Entities.AsNoTracking().ToListAsync(); ;
        }
        
    }
}
