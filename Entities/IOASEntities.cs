using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entities
{
    public interface IOASEntities
    {
        int SaveChanges();
        IDbSet<category> categories { get; set; }
        IDbSet<question> questions { get; set; }
        IDbSet<role> roles { get; set; }
        IDbSet<subcategory> subcategories { get; set; }
        IDbSet<subcategoryTopic> subcategoryTopics { get; set; }
        IDbSet<sysdiagram> sysdiagrams { get; set; }
        IDbSet<test> tests { get; set; }
        IDbSet<topic> topics { get; set; }
        IDbSet<user> users { get; set; }
        IDbSet<userTest> userTests { get; set; }
        IDbSet<userTestAnswer> userTestAnswers { get; set; }
        IDbSet<testLog> testLogs { get; set; }
    }
}
