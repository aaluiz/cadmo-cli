using Contracts;
using Contracts.Repository;
using Entities.Data;
using Entities.Models;
using Entities.Models.Enums;
using Microsoft.Extensions.Configuration;
using Repositories.Abstract;
using System.Linq;

namespace Repositories
{
    public class ModelExampleRepository : RepositoryAbstract<ModelExample>, IModelExampleRepository<ModelExample>
    {
        public ModelExampleRepository(ApiDbContext context, ILoggerManager Logger, IConfiguration configuration) : base(context, Logger, configuration)
        {
            ColllectionFromDataBase = "ModelExamples";
        }
    }
}