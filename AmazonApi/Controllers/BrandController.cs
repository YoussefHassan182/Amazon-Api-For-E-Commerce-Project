using AmazonApi.Models;
using ITI.ElectroDev.Models;
using ITI.Library.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmazonApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BrandController : ControllerBase
    {
        private Context dbcontext;

        public BrandController (Context _dbcontext)
        {
            dbcontext = _dbcontext;
        }

        [HttpGet]
        public BrandViewModel Get()
        {
            var brands = dbcontext.Brands.ToList();
            return new BrandViewModel()
            {
                Success = true,
                Message = " List Of Brands",
                Data = new { Brands = brands }
            };
        }

        [HttpGet("{id}")]

        public BrandViewModel Get(int id)
        {
            var brands = dbcontext.Brands.FirstOrDefault(i => i.Id == id);
            if (brands != null)
            {
                
                return new BrandViewModel
                {
                    Success = true,
                    Data = new
                    {
                        Brand = brands
                         
                    }
                };
            }
            return new BrandViewModel()
            {
                Success = false,
                Message = "Brands Not Found"
            };
        }
    }
    
}
