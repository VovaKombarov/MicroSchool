using IdentityServer4.Models;

namespace IdentityApi
{
    public class IdentityConfiguration
    {
        private readonly IConfiguration _configuration;
        public IdentityConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// ApiScopes — это то, что вы запрашиваете как клиент и как пользователь, на что вы даете согласие.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope( _configuration["AllowedScopes:Teacher"]),
                new ApiScope(_configuration["AllowedScopes:Parent"])
            };
        }

        public IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
              new ApiResource("TeacherApi")
              {
                ApiSecrets = new List<Secret>
                {
                  new Secret(_configuration["Secret:secret"].Sha256())
                }, 
                Scopes = new List<string>()
                {
                    _configuration["AllowedScopes:Teacher"]
                }
              },

              new ApiResource("ParentApi")
              {
                ApiSecrets = new List<Secret>
                {
                  new Secret(_configuration["Secret:secret"].Sha256())
                },
                Scopes = new List<string>()
                {
                    _configuration["AllowedScopes:Parent"]
                }
              },


            };
        }

        /// <summary>
        /// Получаем клиентов, у которых будет доступ к IdentityServer.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = _configuration["ClientIds:Teacher"],
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                     AllowedScopes = {_configuration["AllowedScopes:Teacher"] },
                     //AccessTokenType = AccessTokenType.Reference,
                },

                new Client
                {
                    ClientId = _configuration["ClientIds:Parent"],
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { _configuration["AllowedScopes:Parent"] }
                }
            };
        }

        /// <summary>
        /// IdentityResource - это области, которые будут включены в токен идентификатора
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                //OpenId и Profile это области, вещи,
                //которые клиентское приложение может запрашивать у IdentityServer
                //openid — это специальная область, которая всегда должна быть включена и
                //которая будет запрашивать у пользователей (суб) уникальный идентификатор (userid).
                //а область профиля запросит данные профиля
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
}
