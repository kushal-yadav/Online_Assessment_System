using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IAdminService
    {
        bool AddTest(List<question> questionList);
        test AddTest(test newTest);
        topic GetTopicByTopicName(string topicName);
        bool RemoveTest(int id);
        List<userTest> GetFailedStudents();
        bool UpdateFailedStudentList(List<int> studentsMarkedForRetest);
        question GetQuestion(int questionID);
        List<category> GetAllCategories();
        int AddCategory(category category);
        int AddSubCategory(subcategory subcategory);
        int AddTopic(topic topic);
        int EditQuestion(question editQuestion, int questionId);
        IEnumerable<subcategory> GetSubcategoryByCategory(int categoryId);
        IEnumerable<question> GetQuestionsBySubcategory(int subcategoryId);
    }
}
