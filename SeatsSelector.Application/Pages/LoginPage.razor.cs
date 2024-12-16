﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using SeatsSelector.Application.Models;
using SeatsSelector.Application.Services;

namespace SeatsSelector.Application.Pages
{
    public class LoginPageBase : ComponentBase
    {
        [Inject] protected IAccountService Session { get; set; }
        [Inject] protected NavigationManager Nav { get; set; }

        private Login _data;
        protected Login Data 
        { 
            get
            {
                if (_data == null)
                    _data = new Login();

                return _data;
            }

            set => _data = value;
        }

        protected string Error;

        private string _returnURL;


        protected override void OnInitialized()
        {
            var uri = Nav.ToAbsoluteUri(Nav.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnURL", out var returnURL))
            {
                _returnURL = Uri.UnescapeDataString(returnURL);
            }
            else
                _returnURL = "/";
        }

        protected void LoginForm_OnSubmit(EditContext editContext)
        {
            Error = string.Empty;

            bool isValid = editContext.Validate();

            if (isValid)
                LoginForm_OnValidSubmit(editContext);
        }

        protected async void LoginForm_OnValidSubmit(EditContext editContext)
        {
            try
            {
                await Session.Login(Data.Username, Data.Password);

                Nav.NavigateTo(_returnURL);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    Error = $"Incorrect password for {Data.Username}";
                else
                    Error = $"Failed to reach the server. Please try again.";

                StateHasChanged();
            }
        }
    }
}