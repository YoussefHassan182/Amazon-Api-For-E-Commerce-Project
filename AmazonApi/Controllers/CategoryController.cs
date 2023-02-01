using ITI.ElectroDev.Models;
using ITI.Library.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace AmazonApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {

        private Context db;
        public CategoryController(Context _db)
        {
            db = _db;
        }
        [HttpGet]
        public ResultViewModel Get()
        {
            var categories = db.Category.ToList();
            return new ResultViewModel()
            {
                Success = true,
                Message = "Categories List",
                Data = new {Categories = categories}
            };

        }
        [HttpGet("{id}")]
        public ResultViewModel Get(int id)
        {
            var category = db.Category.FirstOrDefault(i => i.Id == id);
            return new ResultViewModel()
            {
                Success = true,
                Message = "",
                Data = new { category = category }
            };

        
        }
      
    }
}
