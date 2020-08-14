using Microsoft.AspNetCore.Mvc;

namespace HSEApiTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyController : Controller
    {
        private readonly IDummyService _dummyService;

        //В конструкторе контроллера происходит инъекция сервисов через их интерфейсы
        public DummyController(IDummyService dummyService)
        {
            _dummyService = dummyService;
        }

        /*private readonly Random rand;

        public DummyController()
        {
            rand = new Random();
        }*/

        [HttpGet("generate/{number}")]
        public string DummyGenerator(int number)
        {
            return _dummyService.DummyServiceGenerator(number);
        }
    }
}
