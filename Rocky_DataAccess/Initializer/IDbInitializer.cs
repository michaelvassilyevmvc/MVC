using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky_DataAccess.Initializer
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}