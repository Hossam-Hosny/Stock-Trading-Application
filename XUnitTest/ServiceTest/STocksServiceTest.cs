using AutoFixture;
using Entities;
using FluentAssertions;
using Moq;
using RepositoryContracts;
using ServiceContracts.DTO;
using ServiceContracts.Interface;
using Services;

namespace XUnitTest.ServiceTest
{
    public class STocksServiceTest
    {

        private readonly IStockService _stocksService;
        private readonly IFixture _fixture;

        private readonly IStocksRepositories _stocksRepository;
        private readonly Mock<IStocksRepositories> _stocksRepositoryMock;

        public STocksServiceTest()
        {
           _fixture = new Fixture();
            _stocksRepositoryMock = new Mock<IStocksRepositories>();
            _stocksRepository = _stocksRepositoryMock.Object;

            _stocksService = new StocksService(_stocksRepository);
        }


        #region CreateBuyOrder


        // 1. When you supply BuyOrderRequest as null, it should throw ArgumentNullException.

        [Fact]
        public async Task CreateBuyOrder_NullBuyOrder_ToBeArgumentNullException()
        {
            // Arrange 
            BuyOrderRequest? buyOrderRequest = null;

            //Mock
            BuyOrder buyOrderFixture = _fixture.Build<BuyOrder>()
             .Create();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrderFixture);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();


        }

        // 2. When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.

        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint buyOrderQuantity)
        {
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                                                .With(temp => temp.Quantity, buyOrderQuantity)
                                                    .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // 3. When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.


        [Theory]
        [InlineData(100001)]
        public async Task CreateBuyOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderQuantity)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.Quantity, buyOrderQuantity)
             .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();



        }


        // 4. When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint buyOrderPrice)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.Price, buyOrderPrice)
             .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }


        // 5. When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Theory]
        [InlineData(10001)]
        public async Task CreateBuyOrder_PriceIsGreaterThanMaximum_ToBeArgumentException(uint buyOrderPrice)
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.Price, buyOrderPrice)
             .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        // 6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.

        [Fact]
        public async Task CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.StockSymbol, null as string)
             .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }


        // 7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.

        [Fact]
        public async Task CreateBuyOrder_DateOfOrderIsLessThanYear2000_01_01_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
             .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // 8. If you supply all valid values, it should be successful and return an object of BuyOrderRequest type with auto-generated BuyOrderID (guid).

        [Fact]
        public async Task CreateBuyOrder_ProperDetails_ToBeSuccessful()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _fixture.Build<BuyOrderRequest>()
             .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();
            _stocksRepositoryMock.Setup(temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            BuyOrderResponse buyOrderResponseFromCreate = await _stocksService.CreateBuyOrder(buyOrderRequest);

            //Assert
            buyOrder.BuyOrderID = buyOrderResponseFromCreate.BuyOrderID;
            BuyOrderResponse buyOrderResponse_expected = buyOrder.ToBuyOrderResponse();
           buyOrderResponseFromCreate.BuyOrderID.Should().NotBe(Guid.Empty);
          //  buyOrderResponseFromCreate.Should().Be(buyOrderResponse_expected);

        }


        #endregion




        #region CreateSellOrder

        // 1. When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateSellOrder_SellOrderIsNull_ToBeArgumentNullException()
        {
            // Arrange 
            SellOrderRequest? sellOrderRequest = null;

            //Mock
            SellOrder sellOrderFixture = _fixture.Build<SellOrder>()
             .Create();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrderFixture);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }


        // 2. When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.

        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_QuantityIsLessThanMinimum_ToBeArgumentException(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.Quantity, sellOrderQuantity)
             .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        // 3. When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException

        [Theory]
        [InlineData(100001)]
        public async Task CreateSellOrder_QuantityIsGreaterThanMaximum_ToBeArgumentException(uint sellOrderQuantity)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.Quantity, sellOrderQuantity)
             .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();


        }

        // 4. When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_PriceIsLessThanMinimum_ToBeArgumentException(uint sellOrderPrice)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.Price, sellOrderPrice)
             .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // 5. When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Theory]
        [InlineData(10001)]
        public async Task CreateSellorder_PriceIsGreaterThanMaximum_ToBeArgumentException(double buyOrderPrice)
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.Price, buyOrderPrice)
             .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        // 6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.StockSymbol, null as string)
             .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        // 7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_DateOfOrderIsLessThanYear2000_01_01_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
             .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1999-12-31"))
             .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();


        }

        // 8. If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).

        [Fact]
        public async Task  CreateSellOrder_ProperDetails_ToBeSuccessful()
        {
            SellOrderRequest? sellOrderRequest = _fixture.Build<SellOrderRequest>()
     .Create();


            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();
            _stocksRepositoryMock.Setup(temp => temp.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            SellOrderResponse sellOrderResponseFromCreate = await _stocksService.CreateSellOrder(sellOrderRequest);

            //Assert
            sellOrder.SellOrderID = sellOrderResponseFromCreate.SellOrderID;
            SellOrderResponse sellOrderResponse_expected = sellOrder.ToSellOrderResponse();
            sellOrderResponseFromCreate.SellOrderID.Should().NotBe(Guid.Empty);
            sellOrderResponseFromCreate.Should().Be(sellOrderResponse_expected);

        }





        #endregion


        #region GetBuyOrders

        // 1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetBuyOrders_EmptyList_ToBeEmpty()
        {
            //Arrange
            List<BuyOrder> buyOrders = new List<BuyOrder>();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrders);

            //Act
            List<BuyOrderResponse> buyOrdersFromGet = await _stocksService.GetBuyOrders();

            //Assert
            Assert.Empty(buyOrdersFromGet);
        }

        // 2. When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.

        [Fact]
        public async Task GetAllBuyOrders_ProperDetails_ToBeSuccessful()
        {
            //Arrange
            List<BuyOrder> buyOrder_requests = new List<BuyOrder>() {
                                     _fixture.Build<BuyOrder>()
                                     .With(temp => temp.BuyOrderID , Guid.NewGuid())
                                     .Create(),
                                         _fixture.Build<BuyOrder>()
                                         .With(temp => temp.BuyOrderID , Guid.NewGuid())
                                         .Create()
                                                   };

            List<BuyOrderResponse> buyOrders_list_expected = buyOrder_requests.Select(temp => temp.ToBuyOrderResponse()).ToList();
            List<BuyOrderResponse> buyOrder_response_list_from_add = new List<BuyOrderResponse>();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders()).ReturnsAsync(buyOrder_requests);

            //Act
            List<BuyOrderResponse> buyOrders_list_from_get = await _stocksService.GetBuyOrders();


            //Assert
            buyOrders_list_from_get.Should().BeEquivalentTo(buyOrders_list_expected);

        }


        #endregion




        #region GetSellOrders

        // 1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task  GetAllSellOrders_EmptyList_ToBeEmpty()
        {
            //Arrange
            List<SellOrder> sellOrders = new List<SellOrder>();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrders);

            //Act
            List<SellOrderResponse> sellOrdersFromGet = await _stocksService.GetSellOrders();

            //Assert
            Assert.Empty(sellOrdersFromGet);
        }


        // 2. When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async Task GetAllSellorders_ProperDetails_ToBeSuccessful()
        {

            //Arrange
            List<SellOrder> sellOrder_requests = new List<SellOrder>() {
                            _fixture.Build<SellOrder>().Create(),
                            _fixture.Build<SellOrder>().Create()
                                        };

            List<SellOrderResponse> sellOrders_list_expected = sellOrder_requests.Select(temp => temp.ToSellOrderResponse()).ToList();
            List<SellOrderResponse> sellOrder_response_list_from_add = new List<SellOrderResponse>();

            //Mock
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders()).ReturnsAsync(sellOrder_requests);

            //Act
            List<SellOrderResponse> sellOrders_list_from_get = await _stocksService.GetSellOrders();


            //Assert
            sellOrders_list_from_get.Should().BeEquivalentTo(sellOrders_list_expected);
        }


        #endregion


    }
}