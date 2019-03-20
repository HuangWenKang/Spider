using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VideoSpider.Models;

namespace VideoSpider
{
    public interface IVideoSpiderService
    {
        Task<VideoSpiderResult> SearchAsync(string keyword, int pageNumber, int pageSize);
    }    
}
