﻿using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.Api.Clients;
using Xunit;

namespace Weather.UnitTests.TestClasses.Services
{
    public class ApiClientTestsIntegration
    {
        private readonly HttpClient _httpClient = new HttpClient();

        [Theory]
        [InlineData("London", "imperial")]
        [InlineData("Norwich", "metric")]
        [InlineData("Oxford", "")]
        public async Task Calling_api_with_valid_parameters_should_succeed(string query, string units)
        {
            // Arrange
            var apiClient = new ApiClient(_httpClient);

            // Act
            var result = await apiClient.GetCurrentWeather(query, units);

            // Assert
            result.Name.Should().Be(query);
            result.Main.Should().NotBeNull();
            result.Weather.Should().NotBeNull().And.HaveCountGreaterThan(0);
        }

        [Fact]
        public void Calling_api_with_non_existing_city_should_fail()
        {
            // Arrange
            var apiClient = new ApiClient(_httpClient);

            // Act
            Func<Task> act = async () =>
            {
                var result = await apiClient.GetCurrentWeather("NonExitingCity", "metric");
            };

            // Assert
            act.Should().Throw<ApiException>().Where(x =>
                x.ErrorCode == (int)HttpStatusCode.NotFound &&
                x.Message == "Weather for city NonExitingCity not found.");
        }
    }
}