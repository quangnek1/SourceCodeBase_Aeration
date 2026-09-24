using AerationSterilize.Application.Features.V1.Setting.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Setting.Commands.UpdateSetting;

public sealed class UpdateSettingCommand : ICommand<SettingDto>
{
    public int Id { get; private set; }
    public string PlanCAG { get; set; }
    public string PlanPTCA { get; set; }
    public string DataAmiQ411 { get; set; }

    public void SetId(int id)
    {
        Id = id;
    }
}
