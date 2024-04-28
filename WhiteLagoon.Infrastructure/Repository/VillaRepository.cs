using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interface;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository

    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext context) :base(context)
        {
            _db = context;
        }

        public void Update(Villa villa)
        {
            _db.Update(villa);
        }
    }
}
