using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Setting.Commands.DeleteSetting;

public sealed record DeleteSettingCommand(int Id) : ICommand;
