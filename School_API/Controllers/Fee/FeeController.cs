using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Services.Interfaces;
using School_API.Common.Interface;

namespace School_API.Controllers.Fee
{

    public partial class FeeController : BaseController
    {
        private readonly IFeeTypeService _feetype;
        public FeeController(IFeeTypeService feetype, ICurrentUserService currentUser):base(currentUser)
        {
            _feetype = feetype;
        }
    }
    
}
