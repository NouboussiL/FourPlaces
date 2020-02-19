using System;
using System.Collections.Generic;
using System.Text;
using FourPlaces.Models;

namespace FourPlaces.Services
{
    public interface ISessionService
    {
        void Connect(LoginResult _login);
        LoginResult GetLogin();
    }
    class SessionService : ISessionService
    {
        private LoginResult _login;
        public void Connect(LoginResult log)
        {
            _login = log;
        }

        public LoginResult GetLogin()
        {
            return _login;
        }
    }
}
