﻿using Microsoft.IdentityModel.Tokens;
using SeatsSelector.Application.Models;
using SeatsSelector.Shared.Models;
using SeatsSelector.Shared.Models.Rooms;
using SeatsSelector.Shared.Models.Seats;
using SeatsSelector.Shared.Models.Users;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SeatsSelector.Application.Services
{
    public interface IWebAPI
    {
        Task<string> Authenticate(string username, string password);

        Task<Room> GetRoom(int roomId);
        Task<List<User>> GetUsers();
        Task AssignUserToSeat(int seatId, int userId);
    }

    public class WebAPI : IWebAPI
    {
        private readonly HttpClient _httpClient;

        public WebAPI(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        // ---------------------------------------------------------------

        public async Task<string> Authenticate(string username, string password)
        {
            using StringContent jsonContent = new(
                    JsonSerializer.Serialize(new Authenticate() 
                    { 
                        UserName = username, 
                        Password = password 
                    }),
                    Encoding.UTF8,
                    "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync($"account/authenticate", jsonContent);

            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadAsStringAsync();

            return token;
        }


        // ---------------------------------------------------------------

        public async Task<Room> GetRoom(int roomId)
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"rooms/{roomId}");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var room = JsonSerializer.Deserialize<Room>(jsonResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return room;
        }



        // -------------------------------------------------------------

        public async Task AssignUserToSeat(int seatId, int userId)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new AssignUser()
                {
                    SeatId = seatId,
                    UserId = userId
                }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _httpClient.PostAsync($"seats/assign-user", jsonContent);

            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadAsStringAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            using HttpResponseMessage response = await _httpClient.GetAsync($"users");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var users = JsonSerializer.Deserialize<List<User>>(jsonResponse, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return users;
        }


        //public async Task<Seat> GetSeat(int seatId)
        //{
        //    await Task.CompletedTask;

        //    Seat seat = _rooms.First().Seats.FirstOrDefault(seats => seat.);
        //}

    }
}
