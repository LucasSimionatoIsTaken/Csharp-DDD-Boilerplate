using System.Net;
using System.Net.Http.Json;
using Application.SeedWork.Responses;
using FluentAssertions;
using IntegrationTests.config;
using Application.Services.Auth;

namespace IntegrationTests.Tests.Auth;

[Collection("Integration Tests")]
public class LoginTests : TestBase
{
    public LoginTests(TestServer server) : base(server)
    {
    }

    [Fact]
    public async Task Login_WithValidData_ShouldReturnToken()
    {
        var response = await _client.PostAsJsonAsync(TestData.AuthEndpoint, new
        {
            email = TestData.AdminEmail,
            password = TestData.AdminPassword
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<DataResponse<Login.Response>>();
        body.Should().NotBeNull();
        body.Data!.Token.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("commonsdkafjalkdsfa@email.com", "Abc123!@")]
    [InlineData("common", "Abc123!@")]
    [InlineData("common@email.com", "Abc123fasdfasadfs!@")]
    public async Task Login_WithInvalidEmail_ShouldReturnNotFound(string email, string password)
    {
        var response = await _client.PostAsJsonAsync(TestData.AuthEndpoint, new
        {
            email,
            password
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}