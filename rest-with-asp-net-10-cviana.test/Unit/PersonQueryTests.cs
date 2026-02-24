using FluentAssertions;
using rest_with_asp_net_10_cviana.Repositories.QueryBuilders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace rest_with_asp_net_10_cviana.test.Unit
{
    public class PersonQueryTests
    {
        [Fact]
        [DisplayName("Validade query for paged search on page #1")]
        public void PersonQueryForPagedSearchPageOne()
        {
            int page = 1;
            int pageSize = 10;
            string sortDirection = "desc";
            string name = "someone";

            var query = new PersonQueryBuilder().BuildQueries(name, sortDirection, pageSize, page);
            query.query.Should().Be("SELECT * FROM person AS p WHERE 1 = 1 AND (p.first_name LIKE '%someone%' OR p.last_name LIKE '%someone%') ORDER BY p.first_name DESC OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;");
            query.sort.Should().Be(sortDirection.ToUpper());
            query.offset.Should().Be(0);
            query.size.Should().Be(pageSize);
            query.countQuery.Should().Be("SELECT COUNT(*) FROM person AS p WHERE 1 = 1 AND (p.first_name LIKE '%someone%' OR p.last_name LIKE '%someone%');");
        }

        [Fact]
        [DisplayName("Validade query for paged search on page #2")]
        public void PersonQueryForPagedSearchPageTwo()
        {
            int page = 2;
            int pageSize = 10;
            string sortDirection = "asc";
            string name = "someone";

            var query = new PersonQueryBuilder().BuildQueries(name, sortDirection, pageSize, page);
            query.query.Should().Be("SELECT * FROM person AS p WHERE 1 = 1 AND (p.first_name LIKE '%someone%' OR p.last_name LIKE '%someone%') ORDER BY p.first_name ASC OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY;");
            query.sort.Should().Be(sortDirection.ToUpper());
            query.offset.Should().Be(10);
            query.size.Should().Be(pageSize);
            query.countQuery.Should().Be("SELECT COUNT(*) FROM person AS p WHERE 1 = 1 AND (p.first_name LIKE '%someone%' OR p.last_name LIKE '%someone%');");
        }

        [Fact]
        [DisplayName("Validade query for paged search with null name")]
        public void PersonQueryForPagedSearchWithNullName()
        {
            int page = 3;
            int pageSize = 18;
            string sortDirection = "asc";

            var query = new PersonQueryBuilder().BuildQueries(null, sortDirection, pageSize, page);
            query.query.Should().Be("SELECT * FROM person AS p WHERE 1 = 1 ORDER BY p.first_name ASC OFFSET 36 ROWS FETCH NEXT 18 ROWS ONLY;");
            query.sort.Should().Be(sortDirection.ToUpper());
            query.offset.Should().Be(36);
            query.size.Should().Be(pageSize);
            query.countQuery.Should().Be("SELECT COUNT(*) FROM person AS p WHERE 1 = 1;");
        }

        [Fact]
        [DisplayName("Validade query for paged search with empty name")]
        public void PersonQueryForPagedSearchWithEmptyName()
        {
            int page = 1;
            int pageSize = 20;
            string sortDirection = "asc";

            var query = new PersonQueryBuilder().BuildQueries("", sortDirection, pageSize, page);
            query.query.Should().Be("SELECT * FROM person AS p WHERE 1 = 1 ORDER BY p.first_name ASC OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY;");
            query.sort.Should().Be(sortDirection.ToUpper());
            query.offset.Should().Be(0);
            query.size.Should().Be(pageSize);
            query.countQuery.Should().Be("SELECT COUNT(*) FROM person AS p WHERE 1 = 1;");
        }
    }
}
