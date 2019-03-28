using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBS.Core.Entities;
using IBS.Service.Repositories;

namespace IBS.Service.Services
{
    public class CommonService : ICommonService
    {
        private readonly ICommonRepository _commonRepository;
        public CommonService(ICommonRepository commonRepository)
        {
            _commonRepository = commonRepository;
        }
        public IList<Coverage> GetAllCoverages()
        {
            return _commonRepository.GetAllCoverages();
        }

        public IList<Product> GetAllProducts()
        {
            return _commonRepository.GetAllProducts();
        }

        public Coverage GetCoverageById(int coverageId)
        {
            return _commonRepository.GetCoverageById(coverageId);
        }

        public Product GetProductById(int productId)
        {
            return _commonRepository.GetProductById(productId);
        }
    }
}
