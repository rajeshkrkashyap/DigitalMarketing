using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.SeoScore
{
    public interface ISeoScore<T, U>
    {
        Task<bool> Create(T t, U l);
        Task<bool> CreateList(T t, U l);
        Task<bool> UpdateAnalysisStatus(T t, U l);
        Task<bool> Update(T t, U l);
    }
}
