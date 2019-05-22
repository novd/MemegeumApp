using System;
using System.Collections;
using System.Collections.Generic;

namespace memegeumApp.Models
{
    public interface IMemeRespository
    {
        IEnumerable<Meme> GetMemesByTags(IEnumerable<string> tags);
        IEnumerable<Meme> GetAllMemesByNewest();
        IEnumerable<Meme> GetMemesByPage(int numberOfPage);
        IEnumerable<string> GetAllTags();
        IEnumerable<int> GetPageNumberRange(int currentPage);
        int GetMaxPageNumber();
        int AddMemes(IEnumerable<Meme> memes);
    }
}
