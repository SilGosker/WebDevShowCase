using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ShowCase.Services.Account;
using ShowCase.Services.Plants;

namespace ShowCase.Backend;

public class TestApplicationFactory : WebApplicationFactory<Program>
{
    private bool TryRemoveService(IServiceCollection services, Type type)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == type);
        if (descriptor != null)
            services.Remove(descriptor);
        return descriptor != null;
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            if (TryRemoveService(services, typeof(IAccountService)))
            {
                var mockAccountService = new Mock<IAccountService>();
                mockAccountService.Setup(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((string email, string password, CancellationToken ct) =>
                    {
                        if (email == "existing@example.com" && password == "correct-password")
                        {
                            return new DbAccount()
                            {
                                Email = email,
                                Role = Role.PlantHolder
                            };
                        }

                        return null;
                    });

                mockAccountService.Setup(s => s.CreateAccountAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((string email, string password, CancellationToken ct) =>
                    {
                        if (email == "existing@example.com")
                        {
                            return null;
                        }

                        return new DbAccount()
                        {
                            Email = email,
                            Role = Role.PlantHolder
                        };
                    });

                services.AddSingleton(mockAccountService.Object);
            }

            if (TryRemoveService(services, typeof(IPlantService)))
            {
                var mockPlantService = new Mock<IPlantService>();
                mockPlantService.Setup(s =>
                        s.GetPlantAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((int accountId, int plantId, CancellationToken ct) =>
                    {
                        if (plantId == ConnectedPlantId)
                        {
                            return new Plant()
                            {
                                Id = plantId,
                                Name = "Connected Plant"
                            };
                        }

                        if (plantId == DisconnectedPlantId)
                        {
                            return new Plant()
                            {
                                Id = plantId,
                                Name = "Disconnected Plant"
                            };
                        }

                        return null;
                    });

                mockPlantService.Setup(s => s.IsConnectedAsync(It.IsAny<int>(), It.IsAny<int>()))
                    .ReturnsAsync((int _, int plantId) => plantId == -1
                        ? null
                        : plantId == ConnectedPlantId
                            ? "Connected"
                            : "Disconnected");

                mockPlantService.Setup(x => x.CreatePlantAsync(It.IsAny<Plant>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Plant plant, CancellationToken _) => "password");

            services.AddSingleton(mockPlantService.Object);
            }
        });
    }

    public const int ConnectedPlantId = 1;
    public const int DisconnectedPlantId = 2;
}
