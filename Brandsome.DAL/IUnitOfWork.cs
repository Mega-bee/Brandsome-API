using Brandsome.DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brandsome.DAL
{
    public interface IUnitOfWork
    {



        void Save();
    }
}
