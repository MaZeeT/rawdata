using System.Collections.Generic;

namespace DatabaseService.Interfaces.Repositories;

public interface IShared
{
    string GetPostType(int postId);
    SinglePost GetPost(int postId);
    IList<Posts> GetThread(int questionId);
    Questions GetQuestion(int questionId);
    Answers GetAnswer(int answerId);
    int NumberOfQuestions();
    int GetPagination(int matchcount, PagingAttributes pagingAttributes);
}

