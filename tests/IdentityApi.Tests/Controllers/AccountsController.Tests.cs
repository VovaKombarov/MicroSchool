using Common.EventBus;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityApi.Controllers;
using IdentityApi.Services;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Common.ErrorResponse;
using IdentityApi.ViewModels;
using System.Net;
using IdentityApi.Utilities;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Common.Api;
using Common.TestsUtils;



namespace IdentityApi.Tests.Controllers
{
    public class AccountsControllerTests
    {
        #region Fields

        private AccountsController _accountsController;

        private Mock<IIdentityService> _identityservice;

        private Mock<IOptions<Dictionary<string, string>>> _options;

        private Mock<ILogger> _logger;

        #endregion Fields

        #region Setup 

        [SetUp]
        public void Setup()
        {
            _identityservice = new Mock<IIdentityService>();
            _options = new Mock<IOptions<Dictionary<string, string>>>();
            _logger = new Mock<ILogger>();

            _accountsController = new AccountsController(
                _identityservice.Object, _options.Object, _logger.Object);

            Invoker.InitLogger(_logger.Object);

            _SetupOptions();
        }

        private void _SetupOptions()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
            {
                { "TokenError", "Ошибка получения токена!"},
                { "UserExistsError", "Ошибка проверки пользователя!"},
                { "UserExists", "Пользователь с таким email уже существует!"},
                 {"UserNotExists", "Пользователь с таким email не существует!" },
                  {"EmptyValue", "Пустое значение!" },

            };

            _options.Setup(w => w.Value).Returns(keyValuePairs);
        }

        private void _SetupCheckUserViewModel(bool isCheck)
        {
            _identityservice.Setup(w => w.CheckUserViewModel(It.IsAny<UserViewModel>()))
                .Returns(isCheck);
        }

        private void _SetupUserExistsAsync(bool exists)
        {
            _identityservice.Setup(w => w.UserExistsAsync(It.IsAny<UserViewModel>()))
                .Returns(Task.FromResult(exists));
        }

        private void _SetupCreateUserAsync(
            SetupKey setupKey)
        {
            if(setupKey == SetupKey.InternalServerError)
            {
                _identityservice.Setup(w => w.CreateUserAsync(It.IsAny<UserViewModel>()))
                    .Throws(new HttpStatusException(
                        HttpStatusCode.InternalServerError));
            }
            else
            {
                _identityservice.Setup(w => w.CreateUserAsync(It.IsAny<UserViewModel>()))
                   .Returns(Task.CompletedTask);
            } 
        }

        private void _SetupGetTokenFromRequestHeaders(SetupKey setupKey)
        {
            if(setupKey == SetupKey.InternalServerError)
            {
                _identityservice.Setup(w => w.GetTokenFromRequestHeaders(It.IsAny<HttpContext>()))
                    .Throws(new HttpStatusException(
                        HttpStatusCode.InternalServerError));
            }
            else
            {
                _identityservice.Setup(w => w.GetTokenFromRequestHeaders(
                    It.IsAny<HttpContext>()))
                        .Returns("TestToken");

                 
            }
        }

        private void _SetupGetTokenAsync(SetupKey setupKey)
        {
            if (setupKey == SetupKey.InternalServerError)
            {
                _identityservice.Setup(w => w.GetTokenAsync(It.IsAny<UserViewModel>()))
                    .Throws(new HttpStatusException(
                        HttpStatusCode.InternalServerError));
            }
            else
            {
                _identityservice.Setup(w => w.GetTokenAsync(It.IsAny<UserViewModel>()))
                    .Returns(Task.FromResult(It.IsAny<TokenResponse>()));
                   
            }
        }

        #endregion Setup

        #region Tests 

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("", null)]
        [TestCase(null, "")]
        public void SignUp_EmailIsEmpty_ReturnsBadRequest(string name, string email)
        {
            UserViewModel userViewModel = new UserViewModel()
            {
                Name = email,
                Email = email
            };
            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _accountsController.SignUp(userViewModel));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(_options.Object.Value[MessageKey.EmptyValue.ToString()], 
                    Is.EqualTo(exception.Message));
            });
        }

        [Category(TestCategory.BAD_REQUEST)]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("", null)]
        [TestCase(null, "")]
        public void SignUp_NameIsEmpty_ReturnsBadRequest(string name, string email)
        {
            UserViewModel userViewModel = new UserViewModel()
            {
                Name = name,
                Email = email
            };
            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _accountsController.SignUp(userViewModel));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(_options.Object.Value[MessageKey.EmptyValue.ToString()],
                    Is.EqualTo(exception.Message));
            });
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test]
        public void SignUp_UserExistsReturnTrue_ReturnsBadRequest()
        {
            _SetupCheckUserViewModel(true);
            _SetupUserExistsAsync(true);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
              await _accountsController.SignUp(It.IsAny<UserViewModel>()));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(_options.Object.Value[MessageKey.UserExists.ToString()],
                    Is.EqualTo(exception.Message));
            });
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void SignUp_CreateUserAsync_ReturnsInternalServerError()
        {
            _SetupCheckUserViewModel(true);
            _SetupUserExistsAsync(false);
            _SetupCreateUserAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
              await _accountsController.SignUp(It.IsAny<UserViewModel>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));

        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
       public void SignUp_GetTokenAsync_ReturnsInternalServerError()
       {
            _SetupCheckUserViewModel(true);
            _SetupUserExistsAsync(false);
            _SetupCreateUserAsync(SetupKey.Ok);
            _SetupGetTokenAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _accountsController.SignUp(It.IsAny<UserViewModel>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task SignUp_GetTokenAsync_ReturnsOK()
        {
            _SetupCheckUserViewModel(true);
            _SetupUserExistsAsync(false);
            _SetupCreateUserAsync(SetupKey.Ok);
            _SetupGetTokenAsync(SetupKey.ReturnsValue);

            var result = await _accountsController.SignUp(It.IsAny<UserViewModel>());

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Category(TestCategory.BAD_REQUEST)]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("", null)]
        [TestCase(null, "")]
        public void SignIp_EmailIsEmpty_ReturnsBadRequest(string name, string email)
        {
            UserViewModel userViewModel = new UserViewModel()
            {
                Name = email,
                Email = email
            };
            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _accountsController.SignIn(userViewModel));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(_options.Object.Value[MessageKey.EmptyValue.ToString()],
                    Is.EqualTo(exception.Message));
            });
        }

        [Category(TestCategory.BAD_REQUEST)]
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("", null)]
        [TestCase(null, "")]
        public void SignIn_NameIsEmpty_ReturnsBadRequest(string name, string email)
        {
            UserViewModel userViewModel = new UserViewModel()
            {
                Name = name,
                Email = email
            };
            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _accountsController.SignIn(userViewModel));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(_options.Object.Value[MessageKey.EmptyValue.ToString()],
                    Is.EqualTo(exception.Message));
            });
        }

        [Category(TestCategory.BAD_REQUEST)]
        [Test]
        public void SignIn_UserExistsReturnFalse_ReturnsBadRequest()
        {
            _SetupCheckUserViewModel(true);
            _SetupUserExistsAsync(false);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
              await _accountsController.SignIn(It.IsAny<UserViewModel>()));

            Assert.Multiple(() =>
            {
                Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(_options.Object.Value[MessageKey.UserNotExists.ToString()],
                    Is.EqualTo(exception.Message));
            });
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void SignIn_GetTokenAsync_ReturnsInternalServerError()
        {
            _SetupCheckUserViewModel(true);
            _SetupUserExistsAsync(true);
            _SetupGetTokenAsync(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
                await _accountsController.SignIn(It.IsAny<UserViewModel>()));

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task SignIn_GetTokenAsync_ReturnsOK()
        {
            _SetupCheckUserViewModel(true);
            _SetupUserExistsAsync(true);
            _SetupGetTokenAsync(SetupKey.ReturnsValue);

            var result = await _accountsController.SignIn(It.IsAny<UserViewModel>());

            var statusCode = ActionResultConverter.GetStatusCode(result);

            Assert.That(statusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Category(TestCategory.INTERNAL_SERVER_ERROR)]
        [Test]
        public void SignOut_GetTokenFromRequestHeaders_ReturnsInternalServerError()
        {
            _SetupGetTokenFromRequestHeaders(SetupKey.InternalServerError);

            var exception = Assert.ThrowsAsync<HttpStatusException>(async () =>
               await _accountsController.SignOut());

            Assert.That(exception.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        #endregion Tests
    }
}
