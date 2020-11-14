using TempateMicroservice.DAL.Models;
using TempateMicroservice.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TempateMicroservice.DAL.Repositories.Classes
{
    public class TempateRepository : BaseRepository<TemplateModel>, ITempateRepository
    {
        public TempateRepository(TemplateDBContext context) : base(context)
        {

        }

    }
}
