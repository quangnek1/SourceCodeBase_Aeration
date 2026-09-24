using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Setting.Commands.CreateSetting;

public sealed record CreateSettingCommand(
    string PlanCAG,
    string PlanPTCA,
    string DataAmiQ411
) : ICommand;
