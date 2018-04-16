using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace MyDataService {
    public class CustomBasicAuth {

        class UserInfo {
            public string Name { get; set; }
            public string PasswordHash { get; set; }
            public string[] Roles { get; set; }
        }

        static UserInfo[] Users = new[] {
            new UserInfo {
                Name = "John",
                PasswordHash = "F3U89ry4+MMXyqbbT90tcs18J5Y=",
                Roles = new[] { "User", "Admin" }
            }
        };

        static HashAlgorithm Hasher = new SHA1CryptoServiceProvider();

        public static void Authenticate(HttpContext context) {
            // NOTE in production, use basic authentication over SSL only!
            //if(!context.Request.IsSecureConnection)
            //    return;
            context.User = Authenticate(context.Request.Headers);
        }

        public static IPrincipal Authenticate(NameValueCollection requestHeaders) {
            var credentials = ParseAuthHeader(requestHeaders["Authorization"]);
            if (credentials == null)
                return null;

            return GetPrincipalFromCredentials(credentials[0], credentials[1]);
        }

        static string[] ParseAuthHeader(string header) {
            const string headerPrefix = "Basic ";

            if (String.IsNullOrEmpty(header) || !header.StartsWith(headerPrefix))
                return null;

            var cred = Encoding.ASCII.GetString(Convert.FromBase64String(header.Substring(headerPrefix.Length))).Split(':');
            if (cred.Length != 2)
                return null;

            return cred;
        }

        static IPrincipal GetPrincipalFromCredentials(string login, string password) {
            var passwordHash = GetSaltedHash(password);
            var user = Users.FirstOrDefault(u => u.Name == login && u.PasswordHash == passwordHash);
            if (user == null)
                return null;

            return new GenericPrincipal(new GenericIdentity(user.Name), user.Roles);
        }

        static string GetSaltedHash(string password) {
            const string salt = "WLKvHTeV4RGv"; // any random string
            return Convert.ToBase64String(Hasher.ComputeHash(Encoding.UTF8.GetBytes(password + salt)));
        }
    }
}
