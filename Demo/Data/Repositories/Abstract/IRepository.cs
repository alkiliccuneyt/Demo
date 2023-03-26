using System.Data;
using Demo.Data.Models;
using Demo.Views;

namespace Demo.Data.Repositories.Abstract;

public interface IRepository
{
    Task<IEnumerable<PayTypeEntity>> GetPayTypes(IDbConnection connection, int? id);
    Task<IEnumerable<PeriodEntity>> GetPeriods(IDbConnection connection, int? id);
    Task<IEnumerable<PersonalEntity>> GetPersonals(IDbConnection connection, long? identityNumber);
    Task<IEnumerable<PersonalPayTypeView>> GetPersonalPayTypes(IDbConnection connection, long? identityNumber, int? payTypeId);
    Task<IEnumerable<PersonalPayrollEntity>> GetPersonalPayRoles(IDbConnection connection, long? identityNumber, int? payTypeId, int? period);
    Task<IEnumerable<PersonalPayrollSummaryView>> GetPersonalPayrollSummaries(IDbConnection connection, long? identityNumber, int? period);
    Task<Payrolls> XmlPersonalPayRollsExpanded(IDbConnection connection, long? identityNumber);
    Task<CommandResponse> CreateOrUpdatePayTypes(IDbConnection connection, List<PayTypeEntity> entities);
    Task<CommandResponse> CreateOrUpdatePeriods(IDbConnection connection, List<PeriodEntity> entities);
    Task<CommandResponse> CreateOrUpdatePersonals(IDbConnection connection, List<PersonalEntity> entities);
    Task<CommandResponse> CreatePersonalPayTypes(IDbConnection connection, List<PersonalPayTypeEntity> entities);
    Task<CommandResponse> CreateOrUpdatePersonalPayRolls(IDbConnection connection, List<PersonalPayrollEntity> entities);
}