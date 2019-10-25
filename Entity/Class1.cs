using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Class1
    {
        topic t;
        OasEntities e;
       
           public topic subct()
        {
            e = new OasEntities();
            return e.topics.First();
        }
           
       
    }
}
