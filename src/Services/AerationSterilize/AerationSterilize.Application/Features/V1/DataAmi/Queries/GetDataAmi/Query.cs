using AerationSterilize.Application.Features.V1.DataAmi.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.DataAmi.Queries.GetDataAmi;
public sealed record GetDataAmiQuery(string filter) : IQuery<IEnumerable<DataAmiDto>>;
 
