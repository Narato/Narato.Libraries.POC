using Moq;
using Narato.Libraries.POC.Domain.Contracts.DataProviders;
using AutoMapper;
using Xunit;
using Narato.Libraries.POC.Domain.Managers;
using Narato.Libraries.POC.APIContracts.DTO;
using System;
using Narato.ResponseMiddleware.Models.Exceptions;
using System.Linq;

namespace Narato.Libraries.POC.Test.Managers
{
    public class BookManagerTest
    {
        [Fact]
        public async void TestUpdateBook()
        {
            var bookDataProviderMock = new Mock<IBookDataProvider>();
            var mapperMock = new Mock<IMapper>();

            var manager = new BookManager(bookDataProviderMock.Object, mapperMock.Object);

            var book = new BookDto()
            {
                Id = Guid.NewGuid()
            };

            var ex = await Assert.ThrowsAsync<ValidationException<string>>(() => manager.UpdateBookAsync(Guid.NewGuid(), book));

            Assert.Equal("Id in book is not the same as the id in url.", ex.ValidationMessages.Values.First().First());
        }
    }
}
