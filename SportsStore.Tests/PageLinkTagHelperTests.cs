using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

using Moq;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
	public class PageLinkTagHelperTests
	{
		[Fact]
		public void Can_Generage_Page_Links()
		{
			var urlHelper = new Mock<IUrlHelper>();
			var urlHelperFactory = new Mock<IUrlHelperFactory>();
			PageLinkTagHelper helper = null;
			TagHelperContext context = null;
			var content = new Mock<TagHelperContent>();
			TagHelperOutput output = null;

			try
			{
				#region Arrange
				urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
						 .Returns("Test/Page1")
						 .Returns("Test/Page2")
						 .Returns("Test/Page3");

				urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())).Returns(urlHelper.Object);

				helper = new PageLinkTagHelper(urlHelperFactory.Object)
				{
					PageModel = new PagingInfo
					{
						CurrentPage = 2,
						TotalItems = 28,
						ItemsPerPage = 10
					},
					PageAction = "Test"
				};

				context = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");

				output = new TagHelperOutput("div", new TagHelperAttributeList(), 
					(cache, encoder) => Task.FromResult(content.Object));
				#endregion

				#region Act
				helper.Process(context, output);
				#endregion

				#region Assert
				Assert.Equal(@"<a href=""Test/Page1"">1</a>"
					+ @"<a href=""Test/Page2"">2</a>"
					+ @"<a href=""Test/Page3"">3</a>", output.Content.GetContent());
				#endregion
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
