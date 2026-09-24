using AerationSterilize.API.Controllers.V1;
using AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanBox;
using AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanBoxingPosition;
using AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanTag;
using AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
using Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AerationSterilize.Architecture.Tests.Controllers;

public class BoxingJobControllerTests
{
    private readonly Mock<ISender> _senderMock = new();
    private readonly BoxingJobController _sut;

    // (D@02)=SeqNo, (D@12)=ItemCD, (D@30)=LotNo, (D@06)=Qty
    private const string ValidQrQty5 = "(D@02)SEQ001(D@12)ITEM001(D@30)LOT001(D@06)5";
    private const string ValidQrQty15 = "(D@02)SEQ001(D@12)ITEM001(D@30)LOT001(D@06)15";

    public BoxingJobControllerTests()
    {
        _sut = new BoxingJobController(_senderMock.Object);
    }

    #region ScanBoxingPosition

    [Fact]
    public async Task ScanBoxingPosition_WhenSuccess_ReturnsOk()
    {
        var dto = new ScanBoxingPositionDto { PositionId = 1, PositionCode = "P001" };
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanBoxingPositionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(dto));

        var result = await _sut.ScanBoxingPosition("P001", CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task ScanBoxingPosition_WhenFailure_ReturnsNotFound()
    {
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanBoxingPositionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<ScanBoxingPositionDto>(new Error("404", "Boxing position not found")));

        var result = await _sut.ScanBoxingPosition("INVALID", CancellationToken.None);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ScanBoxingPosition_SendsCommandWithCorrectPositionCode()
    {
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanBoxingPositionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new ScanBoxingPositionDto()));

        await _sut.ScanBoxingPosition("P001", CancellationToken.None);

        _senderMock.Verify(
            s => s.Send(
                It.Is<ScanBoxingPositionCommand>(c => c.PositionCode == "P001"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region ScanBox

    [Fact]
    public async Task ScanBox_WhenSuccess_ReturnsOk()
    {
        var dto = new ScanBoxDto { Id = 1, BoxCode = "BOX001", Capacity = 10 };
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanBoxCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(dto));

        var result = await _sut.ScanBox("BOX001", CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task ScanBox_WhenFailure_ReturnsNotFound()
    {
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanBoxCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<ScanBoxDto>(new Error("404", "Box not found")));

        var result = await _sut.ScanBox("INVALID", CancellationToken.None);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ScanBox_SendsCommandWithCorrectBoxCode()
    {
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanBoxCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new ScanBoxDto()));

        await _sut.ScanBox("BOX001", CancellationToken.None);

        _senderMock.Verify(
            s => s.Send(
                It.Is<ScanBoxCommand>(c => c.BoxCode == "BOX001"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region ScanTag

    [Fact]
    public async Task ScanTag_WhenInvalidQrCode_ReturnsBadRequest_WithoutSendingCommand()
    {
        var result = await _sut.ScanTag("not-a-valid-qr", CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
        _senderMock.Verify(s => s.Send(It.IsAny<ScanTagCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        _senderMock.Verify(s => s.Send(It.IsAny<ScanBoxCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ScanTag_WhenQtyLessOrEqualTo10_SendsScanTagCommand()
    {
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanTagCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        await _sut.ScanTag(ValidQrQty5, CancellationToken.None);

        _senderMock.Verify(s => s.Send(It.IsAny<ScanTagCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        _senderMock.Verify(s => s.Send(It.IsAny<ScanBoxCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ScanTag_WhenQtyLessOrEqualTo10AndSuccess_ReturnsOk()
    {
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanTagCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await _sut.ScanTag(ValidQrQty5, CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task ScanTag_WhenQtyLessOrEqualTo10AndFailure_ReturnsBadRequest()
    {
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanTagCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(new Error("400", "Tag already scanned")));

        var result = await _sut.ScanTag(ValidQrQty5, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ScanTag_WhenQtyGreaterThan10_SendsScanBoxCommand()
    {
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanBoxCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new ScanBoxDto()));

        await _sut.ScanTag(ValidQrQty15, CancellationToken.None);

        _senderMock.Verify(s => s.Send(It.IsAny<ScanBoxCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        _senderMock.Verify(s => s.Send(It.IsAny<ScanTagCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ScanTag_WhenQtyGreaterThan10AndSuccess_ReturnsOk()
    {
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanBoxCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new ScanBoxDto()));

        var result = await _sut.ScanTag(ValidQrQty15, CancellationToken.None);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task ScanTag_WhenQtyGreaterThan10AndFailure_ReturnsBadRequest()
    {
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanBoxCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<ScanBoxDto>(new Error("400", "Box scan failed")));

        var result = await _sut.ScanTag(ValidQrQty15, CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ScanTag_WhenQtyGreaterThan10_PassesOriginalQrCodeToBoxCommand()
    {
        _senderMock
            .Setup(s => s.Send(It.IsAny<ScanBoxCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(new ScanBoxDto()));

        await _sut.ScanTag(ValidQrQty15, CancellationToken.None);

        _senderMock.Verify(
            s => s.Send(
                It.Is<ScanBoxCommand>(c => c.BoxCode == ValidQrQty15),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion
}
