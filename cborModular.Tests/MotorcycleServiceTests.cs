using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cborModular.Application;
using cborModular.Domain;
using cborModular.Infrastructure;

namespace cborModular.Tests
{
    public class MotorcycleServiceTests
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly Mock<IMotorcycleRepository> _mockRepository;

        public MotorcycleServiceTests()
        {
            // Nastavení DI kontejneru s mocky pro testování
            var services = new ServiceCollection();

            // Mock IBluetoothClient
            var mockBluetoothClient = new Mock<IBluetoothClient>();
            mockBluetoothClient
                .Setup(b => b.ConnectToDeviceAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);
            mockBluetoothClient
                .Setup(b => b.ReadDataAsync())
                .ReturnsAsync(GetCborResponseForTest());

            services.AddSingleton(mockBluetoothClient.Object);

            // Mock IMotorcycleRepository
            var mockRepository = new Mock<IMotorcycleRepository>();
            services.AddSingleton(mockRepository.Object);

            // Přidání služby, která má být testována
            services.AddTransient<IMotorcycleService, MotorcycleService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task LoadDataAsync_ShouldSaveData()
        {
            var service = _serviceProvider.GetRequiredService<IMotorcycleService>();

            // Act
            await service.LoadDataAsync(Guid.NewGuid());

            // Assert
            _mockRepository.Verify(r => r.SaveAsync(It.IsAny<MotorcycleData>()), Times.Once);
        }

        private byte[] GetCborResponseForTest()
        {
            var writer = new System.Formats.Cbor.CborWriter();

            writer.WriteStartMap(3); // Tři klíč-hodnota dvojice, podle struktury očekávané dekodérem

            // Přidání "Timestamp"
            writer.WriteTextString("Timestamp");
            writer.WriteInt64(DateTime.Now.Ticks);

            // Přidání "Speed"
            writer.WriteTextString("Speed");
            writer.WriteDouble(120.5);  // Testovací hodnota pro rychlost

            // Přidání "Throttle"
            writer.WriteTextString("Throttle");
            writer.WriteSingle(0.75f);  // Testovací hodnota pro plyn

            // Uzavření mapy
            writer.WriteEndMap();

            return writer.Encode();

        }
    }
}
