using JJMasterData.Commons.Language;
using JJMasterData.Core.Html;

namespace JJMasterData.Core.WebComponents;

internal class DataExpLog
{
    private readonly string _name;

    public DataExpLog(string name)
    {
        _name = name;
    }

    internal HtmlElement GetHtmlProcess()
    {
        var div = new HtmlElement(HtmlTag.Div);
        
        div.WithCssClass("text-center");
        
        div.AppendElement(GetLoading());

        div.AppendElement(HtmlTag.Div, div =>
        {
            div.WithAttribute("id", "divMsgProcess")
                .WithCssClass("text-center")
                .WithAttribute("style", "display:none;");
            
            div.AppendElement(GetProgressData());

            div.AppendElement(HtmlTag.Br);

            div.AppendText(Translate.Key("Exportation started on"));

            div.AppendElement(HtmlTag.Span, span =>
            {
                span.WithAttribute("id", "lblStartDate");
            });
            
            div.AppendElement(HtmlTag.Br);
            div.AppendElement(HtmlTag.Br);
            div.AppendElement(HtmlTag.Br);

            div.AppendElement(HtmlTag.A, a =>
            {
                a.WithAttribute("href",
                    $"javascript:JJDataExp.stopProcess('{_name}','{Translate.Key("Stopping Processing...")}')");
                a.AppendElement(HtmlTag.Span, span =>
                {
                    span.WithCssClass("fa fa-stop");
                });
                a.AppendText("&nbsp;" + Translate.Key("Stop the exportation."));
            });
        });
        
        div.AppendScript($"JJDataExp.startProcess('{_name}')");
        
        return div;
    }

    private static HtmlElement GetLoading()
    {
        return new HtmlElement(HtmlTag.Div)
            .WithAttribute("id", "divProcess")
            .WithAttribute("style", "text-align:center;")
            .AppendHiddenInput("current_uploadaction", string.Empty)
            .AppendElement(HtmlTag.Div, div =>
            {
                div.WithAttribute("id", "impSpin");
                div.WithAttribute("style", "position: relative; height: 80px");
            });
    }

    private HtmlElement GetProgressData()
    {
        return new HtmlElement(HtmlTag.Div)
            .AppendElement(HtmlTag.Div, div =>
            {
                div.WithAttribute("id", "divStatus");
                div.WithCssClass("text-center");
                div.AppendElement(HtmlTag.Span, span => { span.WithAttribute("id", "lblResumeLog"); });
            })
            .AppendElement(HtmlTag.Div, div =>
            {
                div.WithAttribute("style", "display:none;width:50%");
                div.WithCssClass(BootstrapHelper.CenterBlock);
                div.AppendElement(HtmlTag.Div, div =>
                {
                    div.WithCssClass("progress");
                    div.AppendElement(HtmlTag.Div, div =>
                    {
                        div.WithCssClass("progress-bar");
                        div.WithAttribute("role", "progressbar");
                        div.WithAttribute("style", "width: 0;");
                        div.WithAttribute("aria-valuemin", "0");
                        div.WithAttribute("aria-valuemax", "100");
                        div.AppendText("0%");
                    });
                });
            });
    }
}