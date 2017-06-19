using Moq;
using Narato.Libraries.POC.Domain.Contracts.DataProviders;
using Xunit;
using Narato.Libraries.POC.Domain.Managers;
using System;
using Narato.ResponseMiddleware.Models.Exceptions;
using System.Linq;
using Narato.Libraries.POC.Domain.Models;

namespace Narato.Libraries.POC.Test.Managers
{
    public class BookManagerTest
    {
        [Fact]
        public async void TestUpdateBook()
        {
            var bookDataProviderMock = new Mock<IBookDataProvider>();

            var manager = new BookManager(bookDataProviderMock.Object);

            var book = new Book()
            {
                Id = Guid.NewGuid()
            };

            var ex = await Assert.ThrowsAsync<ValidationException<string>>(() => manager.UpdateBookAsync(Guid.NewGuid(), book));

            Assert.Equal("Id in book is not the same as the id in url.", ex.ValidationMessages.Values.First().First());
        }
    }
}
