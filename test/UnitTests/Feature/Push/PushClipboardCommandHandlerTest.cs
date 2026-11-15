using FluentAssertions;
using Moq;
using Orchid.Contract.Payload;
using Orchid.Service.Clipboard.Application.Command;
using Orchid.Service.Clipboard.Application.Common;
using Orchid.Service.Clipboard.Application.Handler;
using Xunit;

namespace Orchid.UnitTest.Service;

public sealed class PushClipboardCommandHandlerTest
{
    [Fact]
    public async Task HandleShouldBroadcastEncryptedPayloadToConnectedClients()
    {
        // Given
        Mock<IRealtimeBroadcastService> broadcastMock = new();
        PushClipboardCommandHandler handler = new(broadcastMock.Object);
        EncryptedPayload payload = new(Guid.NewGuid(), [1, 2], [3], [4], "AES-256-GCM", DateTimeOffset.UtcNow);
        PushClipboardCommand command = new(Guid.NewGuid(), payload);

        // When
        bool result = await handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().BeTrue();
        broadcastMock.Verify(
            x => x.BroadcastAsync(payload, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
