using System.Data;
using System.Diagnostics;
using Dapper;
using Demo.Data.Extensions;
using Demo.Data.Models;
using Demo.Data.Repositories.Abstract;
using Demo.Extensions;
using Demo.Views;

// ReSharper disable All
#pragma warning disable CS8603

namespace Demo.Repositories;

public class Repository: IRepository
{

    private readonly Serilog.ILogger _logger;
    private readonly Stopwatch _sw;

    public Repository(Serilog.ILogger logger)
    {
        _logger = logger;
        _sw = new Stopwatch();
    }
    
    public async Task<IEnumerable<PayTypeEntity>> GetPayTypes(IDbConnection connection, int? id)
    {
        var p = new DynamicParameters();
        p.Add("@id", id);
        var result = await connection.QueryAsync<PayTypeEntity>("constants.GetPayTypes", p, null, null, CommandType.StoredProcedure);
        return result;
    }

    public async Task<IEnumerable<PeriodEntity>> GetPeriods(IDbConnection connection, int? id)
    {
        var p = new DynamicParameters();
        p.Add("@id", id);
        var result = await connection.QueryAsync<PeriodEntity>("constants.GetPeriods", p, null, null, CommandType.StoredProcedure);
        return result;
    }
    
    public async Task<IEnumerable<PersonalEntity>> GetPersonals(IDbConnection connection, long? identityNumber)
    {
        var p = new DynamicParameters();
        p.Add("@identityNumber", identityNumber);
        var result = await connection.QueryAsync<PersonalEntity>("finances.GetPersonals", p, null, null, CommandType.StoredProcedure);
        return result;
    }

    public async Task<IEnumerable<PersonalPayTypeView>> GetPersonalPayTypes(IDbConnection connection, long? identityNumber, int? payTypeId)
    {
        var p = new DynamicParameters();
        p.Add("@identityNumber", identityNumber);
        p.Add("@payTypeId", payTypeId);
        var result = await connection.QueryAsync<PersonalPayTypeView>("finances.GetPersonalPayTypes", p, null, null, CommandType.StoredProcedure);
        return result;
    }

    public async Task<IEnumerable<PersonalPayrollEntity>> GetPersonalPayRoles(IDbConnection connection, long? identityNumber, int? payTypeId, int? period)
    {
        var p = new DynamicParameters();
        p.Add("@identityNumber", identityNumber);
        p.Add("@payTypeId", payTypeId);
        p.Add("@period", period);
        var result = await connection.QueryAsync<PersonalPayrollEntity>("finances.GetPersonalPayRolls", p, null, null, CommandType.StoredProcedure);
        return result;
    }

    public async Task<IEnumerable<PersonalPayrollSummaryView>> GetPersonalPayrollSummaries(IDbConnection connection, long? identityNumber, int? period)
    {
        var p = new DynamicParameters();
        p.Add("@identityNumber", identityNumber);
        p.Add("@period", period);
        var result = await connection.QueryAsync<PersonalPayrollSummaryView>("finances.GetPersonalPayrollSummaries", p, null, null, CommandType.StoredProcedure);
        return result;
    }

    public async Task<Payrolls> XmlPersonalPayRollsExpanded(IDbConnection connection, long? identityNumber)
    {
        var p = new DynamicParameters();
        p.Add("@identityNumber", identityNumber);
        var result = await connection.QueryAsync<string>("finances.XmlPersonalPayRollsExpanded", p,null,null,CommandType.StoredProcedure);
        if (string.IsNullOrEmpty(string.Join("", result).ToString()))
            return null;
        return GlobalExtensions.Deserialize<Payrolls>(string.Join("", result).ToString());
    }

    public async Task<CommandResponse> CreateOrUpdatePayTypes(IDbConnection connection, List<PayTypeEntity> entities)
    {
        var p = new DynamicParameters();
        p.Add("@entity", entities.AsTableValuedParameter());
        return await RunCommandAsync(connection, "constants.CreateOrUpdatePayTypes", p);
    }

    public async Task<CommandResponse> CreateOrUpdatePeriods(IDbConnection connection, List<PeriodEntity> entities)
    {
        var p = new DynamicParameters();
        p.Add("@entity", entities.AsTableValuedParameter());
        return await RunCommandAsync(connection, "constants.CreateOrUpdatePeriods", p);
    }

    public async Task<CommandResponse> CreateOrUpdatePersonals(IDbConnection connection, List<PersonalEntity> entities)
    {
        var p = new DynamicParameters();
        p.Add("@entity", entities.AsTableValuedParameter());
        return await RunCommandAsync(connection, "finances.CreateOrUpdatePersonals", p);
    }

    public async Task<CommandResponse> CreatePersonalPayTypes(IDbConnection connection, List<PersonalPayTypeEntity> entities)
    {
        var p = new DynamicParameters();
        p.Add("@entity", entities.AsTableValuedParameter());
        return await RunCommandAsync(connection, "finances.CreatePersonalPayTypes", p);
    }

    public async Task<CommandResponse> CreateOrUpdatePersonalPayRolls(IDbConnection connection, List<PersonalPayrollEntity> entities)
    {
        var p = new DynamicParameters();
        p.Add("@entity", entities.AsTableValuedParameter());
        return await RunCommandAsync(connection, "finances.CreateOrUpdatePersonalPayRolls", p);
    }

    

    

    

    private void RunCommand(IDbConnection connection, string procedureName, SqlMapper.IDynamicParameters parameter)
    {
        try
        {
            _sw.Start();
            connection.Execute(procedureName, parameter, commandType: CommandType.StoredProcedure);
            _sw.Stop();
            _logger.Debug("Procedure : {0} - Elapsed Time : {1} ms", procedureName, _sw.ElapsedMilliseconds);
        }
        catch (Exception e)
        {
            _logger.Debug("Procedure : {0} - Elapsed Time : {1}ms - Error : {2}", procedureName,
                _sw.ElapsedMilliseconds, e.Message);
            if (e.InnerException != null)
                _logger.Debug("Procedure : {0} - Elapsed Time : {1}ms - Error : {2}", procedureName,
                    _sw.ElapsedMilliseconds, e.InnerException.Message);
        }
        finally
        {
            _sw.Reset();
        }
    }

    private async Task<CommandResponse> RunCommandAsync(IDbConnection connection, string procedureName, SqlMapper.IDynamicParameters parameter)
    {
        var result = new CommandResponse();
        try {
            _sw.Start();
            await connection.ExecuteAsync(procedureName, parameter, commandType: CommandType.StoredProcedure);
            _sw.Stop();
            _logger.Information("Procedure : {0} - Elapsed Time : {1} ms", procedureName, _sw.ElapsedMilliseconds);
            result.Status = true;
            return result;
        }
        catch (Exception e) {
            _logger.Error("Procedure : {0} - Elapsed Time : {1}ms - Error : {2}", procedureName, _sw.ElapsedMilliseconds, e.Message);
            if (e.InnerException != null)
                _logger.Error("Procedure : {0} - Elapsed Time : {1}ms - Error : {2}", procedureName, _sw.ElapsedMilliseconds, e.InnerException.Message);
            result.Status = false;
            result.Message = e.Message;
        }
        finally {
            _sw.Reset();
        }

        return result;
    }

    private T RunQuery<T>(IDbConnection connection, string procedureName, object param)
    {
        var result = default(T);
        try
        {
            _sw.Start();
            result = connection.QuerySingle<T>(procedureName, param, commandType: CommandType.StoredProcedure);
            _sw.Stop();
            _logger.Debug("Procedure : {0} - Elapsed Time : {1}ms", procedureName, _sw.ElapsedMilliseconds);
        }
        catch (Exception e)
        {
            _logger.Debug("Procedure : {0} - Elapsed Time : {1}ms - Error : {2}", procedureName,
                _sw.ElapsedMilliseconds, e.Message);
            if (e.InnerException != null)
            {
                _logger.Error(e.InnerException.Message);
                _logger.Debug("Procedure : {0} - Elapsed Time : {1}ms - Error : {2}", procedureName,
                    _sw.ElapsedMilliseconds, e.InnerException.Message);
            }
        }
        finally
        {
            _sw.Reset();
        }

        return result;
    }

    private async Task<T> RunQueryFirst<T>(IDbConnection connection, string procedureName, object param)
    {
        var result = default(T);
        try
        {
            _sw.Start();
            result = await connection.QuerySingleAsync<T>(procedureName, param,
                commandType: CommandType.StoredProcedure);
            _sw.Stop();
            _logger.Debug("Procedure : {0} - Elapsed Time : {1}ms", procedureName, _sw.ElapsedMilliseconds);
        }
        catch (Exception e)
        {
            _logger.Debug("Procedure : {0} - Elapsed Time : {1}ms - Error : {2}", procedureName,
                _sw.ElapsedMilliseconds, e.Message);
            if (e.InnerException != null)
                _logger.Debug("Procedure : {0} - Elapsed Time : {1}ms - Error : {2}", procedureName,
                    _sw.ElapsedMilliseconds, e.InnerException.Message);
        }
        finally
        {
            _sw.Reset();
        }

        return result;
    }
}