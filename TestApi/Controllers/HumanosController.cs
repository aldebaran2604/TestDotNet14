using Microsoft.AspNetCore.Mvc;
using Moq;
using TestApi.Interfaces;
using TestApi.Models;
using TestApi.OtherClasses;

namespace TestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HumanosController : ControllerBase
{
    private readonly ILogger<HumanosController> _logger;
    private readonly TestApiContext _context;
    private readonly Mock<IGenericRepository<Humano>> _mockRepository;

    // Constructor que inyecta las dependencias de logger y context(instancia para trabajar con la base de datos)
    public HumanosController(ILogger<HumanosController> logger, TestApiContext context)
    {
        _context = context;
        _logger = logger;

        //Se crea la instancia de mock para la interfaz del IGenericRepository para consultar, agregar y actualizar datos de humanos
        _mockRepository = new Mock<IGenericRepository<Humano>>();
        _mockRepository.Setup(m => m.GetAll()).Returns(_context.humanos.ToList());
        _mockRepository.Setup(m => m.GetById(It.IsAny<int>())).Returns((int idValue) => _context.humanos.Find(idValue));
        _mockRepository.Setup(m => m.Insert(It.IsAny<Humano>())).Callback((Humano h) =>
        {
            _context.humanos.Add(h);
            _mockRepository.Object.Save();
        });
        _mockRepository.Setup(m => m.Update(It.IsAny<int>(), It.IsAny<Humano>())).Callback((int id, Humano h) =>
        {
            Humano humanoUpdate = _mockRepository.Object.GetById(id);
            humanoUpdate.Nombre = h.Nombre;
            humanoUpdate.Sexo = h.Sexo;
            humanoUpdate.Edad = h.Edad;
            humanoUpdate.Altura = h.Altura;
            humanoUpdate.Peso = h.Peso;
            _mockRepository.Object.Save();
        });
        _mockRepository.Setup(m => m.Save()).Callback(() => { _context.SaveChanges(); });
    }

    //Método get que devuelve la consulta de la tabla humano de la base de datos
    [HttpGet]
    public IResponseInformation<IEnumerable<Humano>> Get()
    {
        ResponseInformation<IEnumerable<Humano>> responseInformation = new ResponseInformation<IEnumerable<Humano>>();
        responseInformation.ResultItem = _mockRepository.Object.GetAll();
        responseInformation.Message = $"Total de registros: {responseInformation.ResultItem?.Count()}";
        return responseInformation;
    }

    //Método get que devuelve un humano por su identificador y en caso de no se encontrado regresa null 
    [HttpGet("{id}")]
    public IResponseInformation<Humano> GetById(int id)
    {
        ResponseInformation<Humano> responseInformation = new ResponseInformation<Humano>();
        responseInformation.ResultItem = _mockRepository.Object.GetById(id);
        if (responseInformation.ResultItem is null)
        {
            responseInformation.Message = "Información no encontrada.";
        }
        return responseInformation;
    }

    //Método post para registrar un objeto humano en la base de datos
    [HttpPost]
    public IResponseInformation Create(Humano humano)
    {
        ResponseInformation responseInformation = new ResponseInformation();
        _mockRepository.Object.Insert(humano);
        responseInformation.Message = "Información guardada con éxito.";
        return responseInformation;
    }

    //Método put para actualizar los un registro de humano en la base de datos.
    [HttpPut("{id}")]
    public IResponseInformation Update(int id, Humano humano)
    {
        ResponseInformation responseInformation = new ResponseInformation();
        _mockRepository.Object.Update(id, humano);
        responseInformation.Message = "Información actualizada con éxito.";
        return responseInformation;
    }
}