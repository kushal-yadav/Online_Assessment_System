using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AdminService :IAdminService
    {
        IOASEntities context;

        public AdminService(IOASEntities user)
        {
            context = user;
        }

        public AdminService()
        {
            context = new OASEntities();
        }

        public int AddCategory(category category)
        {

            try
            {
                int id = context.categories.Count() + 1;
                category.categoryID = id;
                category.created_on = DateTime.Now;
                int result = 0;
                int flag = 0;
                List<category> categoryList =  context.categories.ToList();
                
                foreach (var item in categoryList)
                {
                    if(item.categoryName == category.categoryName)
                    {
                        flag = 1;
                        return result;
                    }
                }
                if (flag == 0)
                {
                    context.categories.Add(category);
                    result = context.SaveChanges();
                }


                return result;
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddSubCategory(subcategory subcategory)
        {
            try
            {
                int id = context.subcategories.Count() + 1;
                subcategory.subcategoryID = id;
                subcategory.created_on = DateTime.Now;
                int result = 0;
                int flag = 0;
                List<subcategory> subcategoryList = context.subcategories.ToList();
                foreach (var item in subcategoryList)
                {
                    if (item.subcategoryName == subcategory.subcategoryName)
                    {
                        flag = 1;
                        return result;
                    }
                }

                if(flag == 0)
                {
                    context.subcategories.Add(subcategory);
                    result = context.SaveChanges();
                }
                return result;
            }
            catch (SqlException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public int AddTopic(topic topic)
        {
            try
            {
                int result = 0;
                context.topics.Add(topic);
                result = context.SaveChanges();
                return result;
            }
            catch(SqlException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public int EditQuestion(question editQuestion,int questionId)
        {
            try
            {
                int result = 0;
                question existingQuestion = context.questions.FirstOrDefault(que => que.questionID == questionId);
                if(existingQuestion !=null)
                {
					existingQuestion.questionText = editQuestion.questionText;
                    existingQuestion.option1 = editQuestion.option1;
                    existingQuestion.option2 = editQuestion.option2;
                    existingQuestion.option3 = editQuestion.option3;
                    existingQuestion.option4 = editQuestion.option4;
                    existingQuestion.answer = editQuestion.answer;


                    //existingQuestion.questionText = editQuestion.questionText;
                    result = context.SaveChanges();
                }
                return result;
            }
            catch(SqlException)
            {
                throw;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IEnumerable<subcategory> GetSubcategoryByCategory(int categoryId)
        {
            var list = context.subcategories.Where(x => x.categoryID == categoryId).ToList();
            
            return list;
        }

        public IEnumerable<question> GetQuestionsBySubcategory(int subcategoryId)
        {
            test test = context.tests.First(x=>x.subcategoryID==subcategoryId);
            return context.questions.Where(x=>x.testid==test.testID).ToList();
        }
        
        public bool AddTest(List<question> questionList)
        {
            try
            {
                int i = 0;
                int testID = 0;
                foreach (var question in questionList)
                {
                    testID = question.testid.Value;
                    question.questionID = context.questions.Count() + ++i;
                    context.questions.Add(question);
                }
                int totalMarks = questionList.Sum(ques => ques.marks);
                context.tests.FirstOrDefault(x => x.testID == testID).totalMarks = totalMarks;
                context.tests.FirstOrDefault(x => x.testID == testID).passingMarks = Convert.ToInt32(totalMarks * 0.35); 
                return context.SaveChanges() > 0;
            }
            catch (SqlException e)
            {
                throw;
            }
        }

        public topic GetTopicByTopicName(string topicName)
        {
            try
            {
                topic topicFromDB = context.topics.FirstOrDefault(x => x.topicName.ToLower() == topicName.ToLower());
                if (topicFromDB != null)
                {
                    return topicFromDB;
                }
                else
                {
                    topicFromDB = new topic();
                    topicFromDB.topicName = topicName;
                    topicFromDB.topicID = context.topics.Count() + 1;
                    topicFromDB.is_Active = true;
                    topicFromDB.created_on = DateTime.Now;
                    context.topics.Add(topicFromDB);
                    if (context.SaveChanges() > 0)
                    {
                        return topicFromDB;
                    }
                    else
                    {
                        throw new Exception("Topic Not Created");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public test AddTest(test newTest)
        {
            try
            {
                newTest.testID = context.tests.Count() + 1;
                newTest.created_on = DateTime.Now;
                newTest.is_Active = newTest.available;
                context.tests.Add(newTest);
                if(context.SaveChanges() > 0)
                {
                    return newTest;
                }
                else
                {
                    return null;
                }
            }
            catch (SqlException e)
            {
                throw;
            }
        }

        public bool RemoveTest(int id)
        {
            try
            {
                test testToRemove = context.tests.FirstOrDefault(x => x.testID == id);
                if(testToRemove != null)
                {
                    context.tests.Remove(testToRemove);
                    return context.SaveChanges() > 0;
                }
                else
                {
                    return true;
                }
                

            }

            catch (Exception)
            {

                throw;
            }
        }
        
        public List<userTest> GetFailedStudents()
        {
            try
            {
                List<userTest> failedStudentList = new List<userTest>();
                foreach (var userTest in context.userTests.ToList())
                {
                    if(userTest.statusOfTest == "fail" && userTest.is_Active == true)
                    {
                        failedStudentList.Add(userTest);
                    }   
                }
                return failedStudentList;
            }
            catch (SqlException e)
            {

                throw;
            }
        }
        
        public bool UpdateFailedStudentList(List<int> studendsMarkedForRetest)
        {
            try
            {
                foreach (var student in studendsMarkedForRetest)
                {
                    context.userTests.First(x => x.userTestID == student && x.statusOfTest == "fail").is_Active = false;
                }
                return context.SaveChanges() > 0;
            }
            catch (SqlException e)
            {

                throw;
            }
        }
        
        public question GetQuestion(int questionID)
        {
            try
            {
                return context.questions.First(x => x.questionID == questionID);
            }
            catch (SqlException e)
            {
                throw;
            }
        }
        
        public List<category> GetAllCategories()
        {
            try
            {
                return context.categories.ToList();
            }
            catch (SqlException e)
            {

                throw;
            }
        }
    }
}
