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

[Route("api/finance")]
public class FinanceController: ControllerBase
{
    private readonly ILogger _logger;
    private readonly IConnectionFactory _factory;
    private readonly IRepository _repo;
    private readonly IMapper _mapper;

    public FinanceController(ILogger logger, IConnectionFactory factory, IRepository repo, IMapper mapper)
    {
        _logger = logger;
        _factory = factory;
        _repo = repo;
        _mapper = mapper;
    }
    
    [HttpGet("personal/{id}",Name = "GetPersonal")]
    [ProducesResponseType(200, Type = typeof(GenericResponse))]
    public async Task<IActionResult> GetPersonal(long id)
    {
        GenericResponse response = new GenericResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var data = await _repo.GetPersonals(conn, id);
            if (!data.Any())
                throw new Exception("Data not found");
            var mapped = _mapper.Map<PersonalView>(data.First());
            response.Entity = mapped;
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.Error($"ConstantController {nameof(GetPersonal)} => Error: {ex.Message}");
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

    [HttpGet("personal",Name = "GetPersonals")]
    [ProducesResponseType(200, Type = typeof(GenericResponse))]
    public async Task<IActionResult> GetPersonals()
    {
        GenericResponse response = new GenericResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var data = await _repo.GetPersonals(conn, null);
            if (!data.Any())
                throw new Exception("Data not found");
            var mapped = _mapper.Map<List<PersonalView>>(data);
            response.Entity = mapped;
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.Error($"ConstantController {nameof(GetPersonals)} => Error: {ex.Message}");
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

    [HttpPost("payrollsummary",Name = "GetPayrollSummary")]
    [ProducesResponseType(200, Type = typeof(GenericResponse))]
    public async Task<IActionResult> GetPayrollSummary([FromBody] PayrollSummaryQueryDto request)
    {
        GenericResponse response = new GenericResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var data = await _repo.GetPersonalPayrollSummaries(conn, request.IdentityNumber, request.Period);
            if (!data.Any())
                throw new Exception("Data not found");
            response.Entity = data.ToList();
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.Error($"FinanceController {nameof(GetPayrollSummary)} => Error: {ex.Message}");
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
    
    [HttpPost("payrollexpanded",Name = "GetPayrollExpanded")]
    [ProducesResponseType(200, Type = typeof(GenericResponse))]
    public async Task<IActionResult> GetPayrollExpanded([FromBody] PayrollExpandedQueryDto request)
    {
        GenericResponse response = new GenericResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var data = await _repo.XmlPersonalPayRollsExpanded(conn, request.IdentityNumber);
            response.Entity = data;
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.Error($"FinanceController {nameof(GetPayrollExpanded)} => Error: {ex.Message}");
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
    
    [HttpPost("addorupdatepersonal",Name = "CreateOrUpdatePersonal")]
    [ProducesResponseType(200, Type = typeof(CommandResponse))]
    public async Task<IActionResult> CreateOrUpdatePersonal([FromBody] PersonalAddUpdateDto request)
    {
        CommandResponse response = new CommandResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var entities = new List<PersonalEntity>();
            entities.Add(_mapper.Map<PersonalEntity>(request));
            response = await _repo.CreateOrUpdatePersonals(conn, entities);
        }
        catch (Exception ex)
        {
            _logger.Error($"FinanceController {nameof(CreateOrUpdatePersonal)} => Error: {ex.Message}");
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
    
    [HttpPost("addpersonalpaytype",Name = "CreatePersonalPayType")]
    [ProducesResponseType(200, Type = typeof(CommandResponse))]
    public async Task<IActionResult> CreatePersonalPayType([FromBody] PersonalPayTypeAddDto request)
    {
        CommandResponse response = new CommandResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var entities = new List<PersonalPayTypeEntity>();
            entities.Add(_mapper.Map<PersonalPayTypeEntity>(request));
            response = await _repo.CreatePersonalPayTypes(conn, entities);
        }
        catch (Exception ex)
        {
            _logger.Error($"FinanceController {nameof(CreatePersonalPayType)} => Error: {ex.Message}");
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
    
    [HttpPost("addpayroll",Name = "CreatePayroll")]
    [ProducesResponseType(200, Type = typeof(CommandResponse))]
    public async Task<IActionResult> CreatePayroll([FromBody] PayrollAddDto request)
    {
        CommandResponse response = new CommandResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            conn.Open();
            var existPayRoll = await _repo.GetPersonalPayRoles(conn, request.PersonalId, request.PayTypeId, request.Period);
            if (existPayRoll.Any())
                throw new Exception("Exist PayRoll data");
            var existPersonal = await _repo.GetPersonals(conn, request.PersonalId);
            if(!existPersonal.Any())
                throw new Exception($"Personal data not found => Personal: {request.PersonalId}");
            var existPersonalPayType = await _repo.GetPersonalPayTypes(conn, request.PersonalId, request.PayTypeId);
            if(!existPersonalPayType.Any())
                throw new Exception($"Personal PayType data not found => PayTypeId: {request.PayTypeId}");
            var existPeriod = await _repo.GetPeriods(conn, request.Period);
            if(!existPeriod.Any())
                throw new Exception($"Period data not found => Period: {request.Period}");
            var entities = new List<PersonalPayrollEntity>();
            entities.Add(_mapper.Map<PersonalPayrollEntity>(request));
            response = await _repo.CreateOrUpdatePersonalPayRolls(conn, entities);
        }
        catch (Exception ex)
        {
            _logger.Error($"FinanceController {nameof(CreatePayroll)} => Error: {ex.Message}");
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
    
    [HttpPut("updatepayroll",Name = "UpdatePayroll")]
    [ProducesResponseType(200, Type = typeof(CommandResponse))]
    public async Task<IActionResult> UpdatePayroll([FromBody] PayrollUpdateDto request)
    {
        CommandResponse response = new CommandResponse();
        await using var conn = _factory.CreateConnection();
        try
        {
            var existPersonal = await _repo.GetPersonals(conn, request.PersonalId);
            if(!existPersonal.Any())
                throw new Exception($"Personal data not found => Personal: {request.PersonalId}");
            var existPersonalPayType = await _repo.GetPersonalPayTypes(conn, request.PersonalId, request.PayTypeId);
            if(!existPersonalPayType.Any())
                throw new Exception($"Personal PayType data not found => PayTypeId: {request.PayTypeId}");
            var existPeriod = await _repo.GetPeriods(conn, request.Period);
            if(!existPeriod.Any())
                throw new Exception($"Period data not found => Period: {request.Period}");
            conn.Open();
            var entities = new List<PersonalPayrollEntity>();
            entities.Add(_mapper.Map<PersonalPayrollEntity>(request));
            response = await _repo.CreateOrUpdatePersonalPayRolls(conn, entities);
        }
        catch (Exception ex)
        {
            _logger.Error($"FinanceController {nameof(UpdatePayroll)} => Error: {ex.Message}");
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