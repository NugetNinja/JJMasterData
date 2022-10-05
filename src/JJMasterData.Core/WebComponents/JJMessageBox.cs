﻿using System.Text;
using JJMasterData.Commons.Language;
using JJMasterData.Core.Html;

namespace JJMasterData.Core.WebComponents;

/// <summary>
/// Exibe uma caixa de mensagem (dialog)
/// </summary>
public class JJMessageBox : JJBaseView
{
    private string _Text;
    public string Text
    {
        get => _Text;
        
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _Text = value.Replace("'","`");
            }
            else
            {
                _Text = value;
            }
            
        }
    }
    
    public string Title { get; set; }
    public MessageIcon Icon { get; set; }
    public MessageSize Size { get; set; }

    public JJMessageBox(string text, MessageIcon icon)
    {
        Text = text;
        Icon = icon;
        Size = MessageSize.Default;;
        Title = Icon switch
        {
            MessageIcon.Error => "Erro",
            MessageIcon.Warning => "Aviso",
            MessageIcon.Info => "Info",
            _ => "Mensagem",
        };
    }

    public JJMessageBox(string text, string title, MessageIcon icon, MessageSize size)
    {
        Title = title;
        Text = text;
        Icon = icon;
        Size = size;
    }

    public JJMessageBox()
    {
    }

    internal override HtmlElement GetHtmlElement()
    {
        var html = new HtmlElement(HtmlTag.Script)
            .WithAttribute("type", "text/javascript")
            .WithAttribute("lang", "javascript")
            .AppendText(GetScript());

        return html;
    }

    public string GetScript()
    {
        var javaScript = new StringBuilder();
        
        string msg = Translate.Key(Text);
        javaScript.AppendLine("$(document).ready(function() {");
        javaScript.Append("\t\t\t");
        javaScript.Append("messageBox.show('");
        javaScript.Append(Translate.Key(Title));
        javaScript.Append("','");
        javaScript.Append(msg.Replace("<br>", "\\r\\n").Replace("\r\n", ""));
        javaScript.Append("', ");
        javaScript.Append((int)Icon);
        javaScript.Append(", ");
        javaScript.Append((int)Size);
        javaScript.AppendLine(");");
        javaScript.Append("\t\t");
        javaScript.AppendLine("});");

        return javaScript.ToString();
    }


}
