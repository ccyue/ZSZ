using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.IService;
using ZSZ.Service.Entities;

namespace ZSZ.Service
{
    class CommonService<T> where T : BaseEntity
    {
        private ZSZDbContext ctx;
        public CommonService(ZSZDbContext ctx)
        {
            this.ctx = ctx;
        }
        /// <summary>
        /// Get all active data
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return ctx.Set<T>().Where(p => !p.IsDeleted);
        }
        /// <summary>
        /// Get total count
        /// </summary>
        /// <returns></returns>
        public long GetTotalCount()
        {
            return GetAll().LongCount();
        }
        public IQueryable<T> GetPageData(int startIndex,int count)
        {
            return GetAll().OrderBy(p=>p.CreateDateTime)
                .Skip(startIndex).Take(count);
        }
        public T GetById(long id)
        {
            return GetAll().Where(p => p.Id == id).SingleOrDefault();
        }
        public void MarkDeleted(long id)
        {
            var data = GetById(id);
            data.IsDeleted = true;
            ctx.SaveChanges();
        }
    }
}
