using RpgAppDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgAppDatabase.Dao
{
    internal interface IEntityDAO
    {
        Task<List<Entity>> GetEntities();
        Task<Entity> FindEntityById(int id);
        Task SaveOrUpdateEntity(Entity entity);
        Task DeleteEntity(Entity entity);
    }
}
