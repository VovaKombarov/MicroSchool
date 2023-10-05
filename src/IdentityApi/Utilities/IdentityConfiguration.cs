using IdentityServer4.Models;

namespace IdentityApi.Utilities
{
    /// <summary>
    /// Конфигуратор настройки IdentityServer.
    /// </summary>
    public class IdentityConfiguration
    {
        #region Fields

        /// <summary>
        /// Объект конфигураций приложения.
        /// </summary>
        private readonly IConfiguration _configuration;

        #endregion Fields

        #region Static 

        /// <summary>
        /// IdentityResource - это области, 
        /// которые будут включены в токен идентификатора.
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                //OpenId и Profile это области,
                //которые клиентское приложение может запрашивать у IdentityServer
                //openid — это специальная область, которая всегда должна быть включена и
                //которая будет запрашивать у пользователей (суб) уникальный идентификатор (userid).
                //а область профиля запросит данные профиля
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        #endregion Static 

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="configuration">Объект конфигураций приложения.</param>
        public IdentityConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Получить клиентов у которых будет доступ к IdentityServer.
        /// </summary>
        /// <returns>Коллекцию клиентов.</returns>
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
                     AccessTokenType = AccessTokenType.Reference
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
        /// Области Api которые необходимо защищать, информация войдет в token доступа.
        /// </summary>
        /// <returns>Коллекцию областей.</returns>
        public IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope( _configuration["AllowedScopes:Teacher"]),
                new ApiScope(_configuration["AllowedScopes:Parent"])
            };
        }

        /// <summary>
        /// Дополнительная информация, которая войдет в token доступа, 
        /// позволяет сделать token доступа более целевым. 
        /// </summary>
        /// <returns>Коллекцию ApiResource.</returns>
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

        #endregion Methods
    }
}
