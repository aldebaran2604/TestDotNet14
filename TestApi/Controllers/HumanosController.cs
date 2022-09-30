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

    public HumanosController(ILogger<HumanosController> logger, TestApiContext context)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Humano> Get()
    {
        Mock<IGenericRepository<Humano>> mockRepository = new Mock<IGenericRepository<Humano>>();
        mockRepository.Setup(m => m.GetAll()).Returns(_context.humanos.ToList());
        return mockRepository.Object.GetAll();
    }

    [HttpGet("{id}")]
    public Humano GetById(int id)
    {
        Mock<IGenericRepository<Humano>> mockRepository = new Mock<IGenericRepository<Humano>>();
        mockRepository.Setup(m=> m.GetById(It.IsAny<int>())).Returns((int idValue)=> _context.humanos.Find(idValue));
        return mockRepository.Object.GetById(id);
    }
}