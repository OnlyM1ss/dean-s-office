using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Contoso.Repository.Sql
{
    public class SqlContosoRepository : IContosoRepository
    {
        private readonly DbContextOptions<ContosoContext> _dbOptions;
        public SqlContosoRepository(DbContextOptionsBuilder<ContosoContext> dbOptionsBuilder)
        {
            _dbOptions = dbOptionsBuilder.Options;
            using (var db = new ContosoContext(_dbOptions))
            {
                db.Database.EnsureCreated();
                try
                {
                    db.Database.Migrate();
                }
                catch
                {

                }
            }
        }

        public IDisciplineRepository Disciplines => new SqlDisciplineRepository(new ContosoContext(_dbOptions));
        public IGroupRepository Groups => new SqlGroupRepository(new ContosoContext(_dbOptions));
        public IPositionRepository Positions => new SqlPositionRepository(new ContosoContext(_dbOptions));
        public ITeacherRepository Teachers => new SqlTeacherRepository(new ContosoContext(_dbOptions));
        public IUserRepository Users => new SqlUserRepository(new ContosoContext(_dbOptions));
    }
}