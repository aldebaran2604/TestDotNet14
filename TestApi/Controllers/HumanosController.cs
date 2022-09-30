using Microsoft.AspNetCore.Mvc;
using Moq;
using TestApi.Interfaces;
using TestApi.Models;

namespace TestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HumanosController : ControllerBase
{
    private readonly ILogger<HumanosController> _logger;
    private readonly TestApiContext _context;
    private readonly Mock<IGenericRepository<Humano>> _mockRepository;

    public HumanosController(ILogger<HumanosController> logger, TestApiContext context)
    {
        _context = context;
        _logger = logger;
        _mockRepository = new Mock<IGenericRepository<Humano>>();
        _mockRepository.Setup(m => m.GetAll()).Returns(_context.humanos.ToList());
        _mockRepository.Setup(m=> m.GetById(It.IsAny<int>())).Returns((int idValue)=> _context.humanos.Find(idValue));
        _mockRepository.Setup(m=> m.Insert(It.IsAny<Humano>())).Callback((Humano h)=>{ _context.humanos.Add(h);});
        _mockRepository.Setup(m => m.Save()).Callback(()=>{_context.SaveChanges(); });
    }

    [HttpGet]
    public IEnumerable<Humano> Get()
    {
        return _mockRepository.Object.GetAll();
    }

    [HttpGet("{id}")]
    public Humano GetById(int id)
    {
        return _mockRepository.Object.GetById(id);
    }

    [HttpPost]
    public ActionResult Create(Humano humano)
    {
        _mockRepository.Object.Insert(humano);
        _mockRepository.Object.Save();
        return Ok();
    }
}