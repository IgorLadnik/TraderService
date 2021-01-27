using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TraderServiceTest
{
    [TestClass]
    public class GqlControllerTests : IntegrationTest
    {
        const string query =
            @"query Traders {
              tradersQuery {
                traders(
                  isDeleted: false 
                    sortBy: '!Birthdate'
    	            pageSize: 2
    	            currentPage: 0) {
                    id
                    firstName
                    lastName
                    birthdate
                    email
                    password
                    isDeleted
                    cryptocurrencies
                        {
                            id
                            currency
                            symbol
                        }
                    }
                }
            }";

        const string mutation =
            @"mutation TraderMutation {
                traderMutation {
                    createTraders(
                      tradersInput: 
                      [
                        {
                            firstName: 'Lior'
                            lastName: 'Levy'
                            birthdate: '1950-01-01'
                            email: 'llevy@trader.com'
                            password: 'lll'
                            isDeleted: false
                            cryptocurrencies: [{ id: 0 }{ id: 1 }]
   	                    }
                        {
                            firstName: 'Ann'
                            lastName: 'Linders'
                            birthdate: '1980-01-01'
                            email: 'annl@trader.com'
                            password: 'lll'
                            isDeleted: false
                            cryptocurrencies: [{ id: 0 }{ id: 0 }]
   	                    }
                      ]
                    ) {
                        status
                        message
                    }
                }
            }";

        [TestMethod]
        public async Task TestQuery()
        {
            var result = await Execute(query);
        }

        [TestMethod]
        public async Task TestMutation()
        {
            var result = await Execute(mutation);
        }
    }
}
