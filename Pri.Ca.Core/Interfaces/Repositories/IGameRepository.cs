﻿using Pri.Ca.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Core.Interfaces.Repositories
{
    public interface IGameRepository : IRepository<Game>
    {
        Task<IEnumerable<Game>> SearchByTitle(string title);
    }
}
