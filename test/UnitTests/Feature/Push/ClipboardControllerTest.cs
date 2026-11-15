using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Orchid.Contract.Model;
using Orchid.Contract.Payload;
using Orchid.Contract.Request;
using Orchid.Service.Clipboard.Api.Controller;
using Orchid.Service.Clipboard.Application.Command;
using Xunit;

namespace Orchid.UnitTest.Service;

/// <summary>
/// <see cref="ClipboardController"/>의 HTTP 응답 매핑(200 Ok 및 400 BadRequest) 동작을 검증하는 단위 테스트입니다.
/// </summary>
public sealed class ClipboardControllerTest
{
    [Fact]
    public async Task PushWhenSenderSucceedsShouldReturnOk()
    {
        // Given
        Mock<ISender> senderMock = new();
        senderMock.Setup(s => s.Send(It.IsAny<PushClipboardCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        ClipboardController controller = new(senderMock.Object);
        EncryptedPayload payload = new(Guid.NewGuid(), [1], [2], [3], "AES", DateTimeOffset.UtcNow);
        PushClipboardRequest request = new(Guid.NewGuid(), DevicePlatform.Windows, ContentType.Text, payload);

        // When
        IActionResult response = await controller.Push(request, CancellationToken.None);

        // Then
        response.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task PushWhenSenderFailsShouldReturnBadRequest()
    {
        // Given
        Mock<ISender> senderMock = new();
        senderMock.Setup(s => s.Send(It.IsAny<PushClipboardCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        ClipboardController controller = new(senderMock.Object);
        EncryptedPayload payload = new(Guid.NewGuid(), [1], [2], [3], "AES", DateTimeOffset.UtcNow);
        PushClipboardRequest request = new(Guid.NewGuid(), DevicePlatform.Windows, ContentType.Text, payload);

        // When
        IActionResult response = await controller.Push(request, CancellationToken.None);

        // Then
        response.Should().BeOfType<BadRequestResult>();
    }
}
