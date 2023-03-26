using System.Data;
using AutoMapper;
using Demo.Data.Configurations.Abstract;
using Demo.Data.Models;
using Demo.Data.Repositories.Abstract;
using Demo.Dtos;
using Demo.Views;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

#pragma warning disable CS8625
#pragma warning disable CS8604

// ReSharper disable All

namespace Demo.Controllers;

[Route("api/constant")]
public class ConstantController: ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConnectionFactory _factory;
    private readonly IRepository _repo;
    private readonly IMapper _mapper;

    public ConstantController(ILogger logger, IConnectionFactory factory, IRepository repo, IMapper mapper)
    {
        _logger = logger;
        _factory = factory;
        _repo = repo;
        _mapper = mapper;
    }
    
    [HttpGet("paytype/{id}",Name = "GetPayType")]
    [ProducesResponseType(200, Type = typeof(GenericResponse))]
    public async Task<IActionResult> GetPayType(int id)
    {
        GenericResponse response = new GenericResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var data = await _repo.GetPayTypes(conn, id);
            if (!data.Any())
                throw new Exception("Data not found");
            var mapped = _mapper.Map<ConstantView>(data.First());
            response.Entity = mapped;
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.Error($"ConstantController {nameof(GetPayType)} => Error: {ex.Message}");
            response.Success = false;
            response.Message = ex.Message;
            response.Entity = null;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        
        return Ok(response);
    }

    [HttpGet("paytype",Name = "GetPayTypes")]
    [ProducesResponseType(200, Type = typeof(GenericResponse))]
    public async Task<IActionResult> GetPayTypes()
    {
        GenericResponse response = new GenericResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var data = await _repo.GetPayTypes(conn, null);
            if (!data.Any())
                throw new Exception("Data not found");
            var mapped = _mapper.Map<List<ConstantView>>(data);
            response.Entity = mapped;
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.Error($"ConstantController {nameof(GetPayTypes)} => Error: {ex.Message}");
            response.Success = false;
            response.Message = ex.Message;
            response.Entity = null;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        
        return Ok(response);
    }
    
    [HttpPost("addpaytype",Name = "CreatePayType")]
    [ProducesResponseType(200, Type = typeof(CommandResponse))]
    public async Task<IActionResult> CreatePayType([FromBody] PayTypeAddDto request)
    {
        CommandResponse response = new CommandResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var entities = new List<PayTypeEntity>();
            entities.Add(_mapper.Map<PayTypeEntity>(request));
            response = await _repo.CreateOrUpdatePayTypes(conn, entities);
        }
        catch (Exception ex)
        {
            _logger.Error($"FinanceController {nameof(CreatePayType)} => Error: {ex.Message}");
            response.Status = false;
            response.Message = ex.Message;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        
        return Ok(response);
    }
    
    [HttpPut("updatepaytype",Name = "UpdatePayType")]
    [ProducesResponseType(200, Type = typeof(CommandResponse))]
    public async Task<IActionResult> UpdatePayType([FromBody] PayTypeUpdateDto request)
    {
        CommandResponse response = new CommandResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var entities = new List<PayTypeEntity>();
            entities.Add(_mapper.Map<PayTypeEntity>(request));
            response = await _repo.CreateOrUpdatePayTypes(conn, entities);
        }
        catch (Exception ex)
        {
            _logger.Error($"FinanceController {nameof(CreatePayType)} => Error: {ex.Message}");
            response.Status = false;
            response.Message = ex.Message;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        
        return Ok(response);
    }
    
    [HttpGet("period/{id}",Name = "GetPeriod")]
    [ProducesResponseType(200, Type = typeof(GenericResponse))]
    public async Task<IActionResult> GetPeriod(int id)
    {
        GenericResponse response = new GenericResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var data = await _repo.GetPeriods(conn, id);
            if (!data.Any())
                throw new Exception("Data not found");
            var mapped = _mapper.Map<ConstantView>(data.First());
            response.Entity = mapped;
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.Error($"ConstantController {nameof(GetPeriod)} => Error: {ex.Message}");
            response.Success = false;
            response.Message = ex.Message;
            response.Entity = null;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        
        return Ok(response);
    }
    
    [HttpGet("period",Name = "GetPeriods")]
    [ProducesResponseType(200, Type = typeof(GenericResponse))]
    public async Task<IActionResult> GetPeriods()
    {
        GenericResponse response = new GenericResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var data = await _repo.GetPeriods(conn, null);
            if (!data.Any())
                throw new Exception("Data not found");
            var mapped = _mapper.Map<List<ConstantView>>(data);
            response.Entity = mapped;
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.Error($"ConstantController {nameof(GetPeriods)} => Error: {ex.Message}");
            response.Success = false;
            response.Message = ex.Message;
            response.Entity = null;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        
        return Ok(response);
    }

    [HttpPost("addperiod",Name = "CreatePeriod")]
    [ProducesResponseType(200, Type = typeof(CommandResponse))]
    public async Task<IActionResult> CreatePeriod([FromBody] PeriodAddDto request)
    {
        CommandResponse response = new CommandResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var entities = new List<PeriodEntity>();
            entities.Add(_mapper.Map<PeriodEntity>(request));
            response = await _repo.CreateOrUpdatePeriods(conn, entities);
        }
        catch (Exception ex)
        {
            _logger.Error($"FinanceController {nameof(CreatePeriod)} => Error: {ex.Message}");
            response.Status = false;
            response.Message = ex.Message;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        
        return Ok(response);
    }
    
    [HttpPut("updateperiod",Name = "UpdatePeriod")]
    [ProducesResponseType(200, Type = typeof(CommandResponse))]
    public async Task<IActionResult> UpdatePeriod([FromBody] PeriodUpdateDto request)
    {
        CommandResponse response = new CommandResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var entities = new List<PeriodEntity>();
            entities.Add(_mapper.Map<PeriodEntity>(request));
            response = await _repo.CreateOrUpdatePeriods(conn, entities);
        }
        catch (Exception ex)
        {
            _logger.Error($"FinanceController {nameof(UpdatePeriod)} => Error: {ex.Message}");
            response.Status = false;
            response.Message = ex.Message;
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        
        return Ok(response);
    }
}