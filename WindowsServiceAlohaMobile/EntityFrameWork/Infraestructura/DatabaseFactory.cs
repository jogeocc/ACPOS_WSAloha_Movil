using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceAlohaMobile.EntityFrameWork.Context;

namespace WindowsServiceAlohaMobile.EntityFrameWork.Infraestructura
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private ApplicationDbContext dataContext;

        ApplicationDbContext IDatabaseFactory.Get()
        {
            return dataContext = new ApplicationDbContext();
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
