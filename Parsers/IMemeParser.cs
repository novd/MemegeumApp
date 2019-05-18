using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using memegeumApp.Models;

namespace memegeumApp.Parsers
{
    public interface IMemeParser
    {
        Task<List<Meme>> GetMemesByNewest(int amount);
    }
}
