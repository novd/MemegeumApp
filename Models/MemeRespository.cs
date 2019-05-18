using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace memegeumApp.Models
{
    public class MemeRespository : IMemeRespository
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        List<Meme> _memes;

        readonly int _MEMES_PER_PAGE = 8;
        readonly int _DEFAULT_PAGE_NUMBERS_PER_PAGE = 10;
        public MemeRespository(IHttpContextAccessor httpContextAccessor)
        {
            _memes = new List<Meme>();
            _httpContextAccessor = httpContextAccessor;
        }


        public int AddMemes(IEnumerable<Meme> newMemes)
        {
            var oldMemesAmount = _memes.Count();

            _memes.AddRange(
                newMemes.Where(newMeme => !InCollection(_memes, newMeme))
                );

            return _memes.Count() - oldMemesAmount;
        }


        public IEnumerable<Meme> GetAllMemesByNewest()
        {
            var filteredMemes = _memes.ToList();
            filteredMemes.Reverse();

            filteredMemes = FilterMemesByBlackList(filteredMemes);
            filteredMemes = FilterMemesByWhiteList(filteredMemes);

            return filteredMemes;
        }

        public IEnumerable<Meme> GetMemesByPage(int numberOfPage)
        {
            var index = _MEMES_PER_PAGE * (numberOfPage-1);
            if ((index + _MEMES_PER_PAGE) < GetAllMemesByNewest().Count())
            {
                return GetAllMemesByNewest().ToList().GetRange(index, _MEMES_PER_PAGE);
            }

            throw new ArgumentOutOfRangeException($"There is no {numberOfPage} page in respository");
        }

        public int GetMaxPageNumber()
        {
            return ((GetAllMemesByNewest().Count() / _MEMES_PER_PAGE));
        }

        public IEnumerable<int> GetPageNumberRange(int currentNumber)
        {
            var maxNumber = GetMaxPageNumber();

            var firstInRange = GetFirstInRange(currentNumber);
            var lastInRange = GetLastInRange(currentNumber, maxNumber);
            var range = Enumerable.Range(
                            firstInRange,
                            (lastInRange - firstInRange + 1));

            range = TryToCompletePageRange(range.ToList(), maxNumber);

            return range.Reverse();
        }

        public IEnumerable<Meme> GetMemesByTags(IEnumerable<string> tags)
        {
            return _memes.Where(meme => tags.Any(tag => meme.Tags.Contains(tag)));
        }

        #region Additional private Methods
        bool InCollection(IEnumerable<Meme> collection, Meme meme)
        {
            return collection.Select(item => item.ImagePath).Contains(meme.ImagePath);
        }

        #region GetPageNumberRange help methods
        int GetFirstInRange(int currentNumber)
        {
            return currentNumber - (_DEFAULT_PAGE_NUMBERS_PER_PAGE / 2) > 0 ?
                   currentNumber - (_DEFAULT_PAGE_NUMBERS_PER_PAGE / 2) :
                    1;
        }

        int GetLastInRange(int currentNumber, int maxNumber)
        {
            return currentNumber + (_DEFAULT_PAGE_NUMBERS_PER_PAGE / 2) < maxNumber ?
                   currentNumber + (_DEFAULT_PAGE_NUMBERS_PER_PAGE / 2) :
                   maxNumber;
        }

        List<int> TryToCompletePageRange(List<int> range, int maxNumber)
        {
            var completeRange = new List<int>(range);
            while ((completeRange.Count() < _DEFAULT_PAGE_NUMBERS_PER_PAGE) && completeRange.LastOrDefault() < maxNumber)
            {
                completeRange.Add(completeRange.LastOrDefault() + 1);
            }

            while ((completeRange.Count() < _DEFAULT_PAGE_NUMBERS_PER_PAGE) && completeRange.FirstOrDefault() > 1)
            {
                completeRange.Insert(0, completeRange.FirstOrDefault() - 1);
            }

            return completeRange.Take(_DEFAULT_PAGE_NUMBERS_PER_PAGE).ToList();
        }

        #endregion

        #region GetAllMemesByNewest help methods
        List<Meme> FilterMemesByBlackList(IEnumerable<Meme> memesToFilter)
        {
            var blackListTagsAsText = _httpContextAccessor.HttpContext.Session.GetString("_blackListTags");

            if (!string.IsNullOrEmpty(blackListTagsAsText))
            {
                var blackListTagsList = blackListTagsAsText.Split("#").Where(tag => tag.Length > 0);

                //removing memes containing tags from black list
                return memesToFilter.Where(meme =>
                                           !meme.Tags.Any(tag => blackListTagsList.Contains(tag))
                                           ).ToList();
            }

            return memesToFilter.ToList();
        }

        List<Meme> FilterMemesByWhiteList(IEnumerable<Meme> memesToFilter)
        {
            var whiteListTagsAsText = _httpContextAccessor.HttpContext.Session.GetString("_whiteListTags");

            if (!string.IsNullOrEmpty(whiteListTagsAsText))
            {
                var whiteListTagsList = whiteListTagsAsText.Split("#").Where(tag => tag.Length > 0);

                //removing memes which do not contain any tag from white list
                return memesToFilter.Where(meme =>
                                            meme.Tags.Any(tag => whiteListTagsList.Contains(tag))
                                            ).ToList();
            }

            return memesToFilter.ToList();
        }
        #endregion

        #endregion
    }
}
