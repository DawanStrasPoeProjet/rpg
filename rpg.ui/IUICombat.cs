using RPG.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.UI
{
    public interface IUICombat
    {
        public void Initialize(IEntity player,
            IEnumerable<IEntity> turnOrder,
            IEnumerable<IEntity> aliveEntities,
            IEnumerable<IEntity> enemies,
            string description = "",
            int turn = -1);
        public void WaitEnterKeyPress();
        public void Display();
        public void Update(IEntity? player = null,
            IEnumerable<IEntity>? turnOrder = null,
            IEnumerable<IEntity>? aliveEntities = null,
            IEnumerable<IEntity>? enemies = null,
            string? description = null,
            int turn = -1);
        public dynamic? GetPrompt(PromptType prompt);
    }
}
