using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceAlohaMobile.EntityFrameWork.Context;

namespace WindowsServiceAlohaMobile.EntityFrameWork.Infraestructura
{
    public interface IDatabaseFactory : IDisposable
    {
        ApplicationDbContext Get();
    }
}
