using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Class1
    {
        OASEntities e = new OASEntities();
        public List<testLog> mymethod()
        {
            
            return e.users.First(x=>x.userID == 1).userTests.First().testLogs.ToList();
        }
       
    }
}
