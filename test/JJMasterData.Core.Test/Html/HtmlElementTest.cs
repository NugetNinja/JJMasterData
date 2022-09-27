using JJMasterData.Core.DataDictionary;
using JJMasterData.Core.Html;

namespace JJMasterData.Core.Test.Html;

public class HtmlElementTest
{
    [Theory]
    [InlineData(HtmlTag.Div)]
    [InlineData(HtmlTag.Span)]
    public void RenderTagWithCloseTag_Test(HtmlTag tag)
    {
        var builder = new HtmlBuilder();
        builder.StartElement(tag);
        var result = builder.RenderHtml();

        string formattedTag = tag.ToString().ToLower();
        Assert.Equal($"<{formattedTag}></{formattedTag}>",result);
    }
    
    [Theory]
    [InlineData(HtmlTag.Br)]
    public void RenderTagSelfClosed_Test(HtmlTag tag)
    {
        var builder = new HtmlBuilder();
        builder.StartElement(tag);
        var result = builder.RenderHtml();

        string formattedTag = tag.ToString().ToLower();
        Assert.Equal($"<{formattedTag}/>",result);
    }
}