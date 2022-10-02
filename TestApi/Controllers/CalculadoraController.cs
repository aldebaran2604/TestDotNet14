using Microsoft.AspNetCore.Mvc;
using TestApi.Interfaces;
using TestApi.OtherClasses;

namespace TestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculadoraController : ControllerBase
{
    //Se configura un diccionario con las operaciones básicas y callback para realizar el calculo correspondiente
    private static readonly Lazy<Dictionary<string, Func<double, double, double>>> _diccionarioOperadores = new Lazy<Dictionary<string, Func<double, double, double>>>(() =>
    {
        var operadoresAccion = new Dictionary<string, Func<double, double, double>>();
        operadoresAccion.Add("suma", (double numeroIzquierda, double numeroDerecha) => { return numeroIzquierda + numeroDerecha; });
        operadoresAccion.Add("resta", (double numeroIzquierda, double numeroDerecha) => { return numeroIzquierda - numeroDerecha; });
        operadoresAccion.Add("multiplicacion", (double numeroIzquierda, double numeroDerecha) => { return numeroIzquierda * numeroDerecha; });
        operadoresAccion.Add("division", (double numeroIzquierda, double numeroDerecha) => { return numeroIzquierda / numeroDerecha; });
        return operadoresAccion;
    });

    private readonly ILogger<CalculadoraController> _logger;
    public CalculadoraController(ILogger<CalculadoraController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IResponseInformation<double> Get()
    {
        _logger.LogInformation("Inicio OperacionesGet");
        double numeroIzquierda = double.Parse(Request.Headers["numeroIzquierda"]);
        double numeroDerecha = double.Parse(Request.Headers["numeroDerecha"]);
        string operador = Request.Headers["operador"];
        if (!_diccionarioOperadores.Value.Keys.Contains(operador)) throw new InvalidOperationException($"El operador '{operador}' no es valido, se esperaba ({string.Join(", ", _diccionarioOperadores.Value.Keys)}).");

        ResponseInformation<double> responseInformation = new ResponseInformation<double>();
        double resultadoOperador = _diccionarioOperadores.Value[operador](numeroIzquierda, numeroDerecha);
        responseInformation.ResultItem = resultadoOperador;
        responseInformation.Message = $"El resultado de la operación es: '{resultadoOperador}'.";
        _logger.LogInformation("Fin OperacionesGet");
        return responseInformation;
    }

    [HttpPost("{numeroIzquierda}/{operador}/{numeroDerecha}")]
    public IResponseInformation<double> OperacionesPost(double numeroIzquierda, string operador, double numeroDerecha)
    {
        _logger.LogInformation("Inicio OperacionesPost");
        if (!_diccionarioOperadores.Value.Keys.Contains(operador)) throw new InvalidOperationException($"El operador '{operador}' no es valido, se esperaba ({string.Join(", ", _diccionarioOperadores.Value.Keys)}).");

        ResponseInformation<double> responseInformation = new ResponseInformation<double>();
        double resultadoOperador = _diccionarioOperadores.Value[operador](numeroIzquierda, numeroDerecha);
        responseInformation.ResultItem = resultadoOperador;
        responseInformation.Message = $"El resultado de la operación es: '{resultadoOperador}'.";
        _logger.LogInformation("Fin OperacionesPost");
        return responseInformation;
    }
}